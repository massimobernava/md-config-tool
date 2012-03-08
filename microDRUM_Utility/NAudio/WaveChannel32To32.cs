using System;

namespace NAudio.Wave
{
    /// <summary>
    /// Represents Channel for the WaveMixerStream
    /// 32 bit output and 32 bit input
    /// It's output is always stereo
    /// The input stream can be panned
    /// </summary>
    public class WaveChannel32To32 : AudioEffectWaveStream, ISampleNotifier
    {
        private WaveStream sourceStream;
        private readonly WaveFormat waveFormat;
        private readonly long length;
        private readonly int destBytesPerSample;
        private readonly int sourceBytesPerSample;
        private volatile float volume;
        private volatile float pan;
        private long position;

        /// <summary>
        /// Creates a new WaveChannel32To32
        /// </summary>
        /// <param name="sourceStream">the source stream</param>
        /// <param name="volume">stream volume (1 is 0dB)</param>
        /// <param name="pan">pan control (-1 to 1)</param>
        public WaveChannel32To32(WaveStream sourceStream, float volume, float pan)
        {
            PadWithZeroes = true;
            if (sourceStream.WaveFormat.Encoding != WaveFormatEncoding.Pcm)
                throw new ApplicationException("Only PCM supported");
            if (sourceStream.WaveFormat.BitsPerSample != 32)
                throw new ApplicationException("Only 32 bit audio supported");

            // always outputs stereo 32 bit
            waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sourceStream.WaveFormat.SampleRate, 2);
            destBytesPerSample = 8; // includes stereo factoring

            this.sourceStream = sourceStream;
            this.volume = volume;
            this.pan = pan;
            sourceBytesPerSample = sourceStream.WaveFormat.Channels * sourceStream.WaveFormat.BitsPerSample / 8;

            length = SourceToDest(sourceStream.Length);
            position = 0;
        }

        private long SourceToDest(long sourceBytes)
        {
            return (sourceBytes / sourceBytesPerSample) * destBytesPerSample;
        }

        private long DestToSource(long destBytes)
        {
            return (destBytes / destBytesPerSample) * sourceBytesPerSample;
        }

        /// <summary>
        /// Creates a WaveChannel32To32 with default settings
        /// </summary>
        /// <param name="sourceStream">The source stream</param>
        public WaveChannel32To32(WaveStream sourceStream)
            :
            this(sourceStream, 1.0f, 0.0f)
        {
        }

        /// <summary>
        /// Gets the block alignment for this WaveStream
        /// </summary>
        public override int BlockAlign
        {
            get
            {
                return (int)SourceToDest(sourceStream.BlockAlign);
            }
        }

        /// <summary>
        /// Returns the stream length
        /// </summary>
        public override long Length
        {
            get
            {
                return length;
            }
        }

        /// <summary>
        /// Gets or sets the current position in the stream
        /// </summary>
        public override long Position
        {
            get
            {
                return position;
            }
            set
            {
                lock (this)
                {
                    // make sure we don't get out of sync
                    value -= (value % BlockAlign);
                    if (value < 0)
                    {
                        sourceStream.Position = 0;
                    }
                    else
                    {
                        sourceStream.Position = DestToSource(value);
                    }
                    // source stream may not have accepted the reposition we gave it
                    position = SourceToDest(sourceStream.Position);
                }
            }
        }

        byte[] sourceBuffer;

        /// <summary>
        /// Helper function to avoid creating a new buffer every read
        /// </summary>
        byte[] GetSourceBuffer(int bytesRequired)
        {
            if (sourceBuffer == null || sourceBuffer.Length < bytesRequired)
            {
                sourceBuffer = new byte[bytesRequired];
            }
            return sourceBuffer;
        }

        /// <summary>
        /// Reads bytes from this wave stream
        /// </summary>
        /// <param name="destBuffer">The destination buffer</param>
        /// <param name="offset">Offset into the destination buffer</param>
        /// <param name="numBytes">Number of bytes read</param>
        /// <returns>Number of bytes read.</returns>
        public override int Read(byte[] destBuffer, int offset, int numBytes)
        {
            int bytesWritten = 0;
            // 1. fill with silence
            if (position < 0)
            {
                bytesWritten = (int)Math.Min(numBytes, 0 - position);
                for (int n = 0; n < bytesWritten; n++)
                    destBuffer[n + offset] = 0;
            }
            if (bytesWritten < numBytes)
            {
                if (sourceStream.WaveFormat.Channels == 1)
                {
                    int sourceBytesRequired = (numBytes - bytesWritten);
                    byte[] sourceBuffer = GetSourceBuffer(sourceBytesRequired);
                    int read = sourceStream.Read(sourceBuffer, 0, sourceBytesRequired);
                    MonoToStereo(destBuffer, offset + bytesWritten, sourceBuffer, read);
                    bytesWritten += (read * 2);
                }
                else
                {
                    int sourceBytesRequired = (numBytes - bytesWritten);
                    byte[] sourceBuffer = GetSourceBuffer(sourceBytesRequired);
                    int read = sourceStream.Read(sourceBuffer, 0, sourceBytesRequired);
                    AdjustVolume(destBuffer, offset + bytesWritten, sourceBuffer, read);
                    bytesWritten += read;
                }
            }
            // 3. Fill out with zeroes
            if (PadWithZeroes && bytesWritten < numBytes)
            {
                Array.Clear(destBuffer, offset + bytesWritten, numBytes - bytesWritten);
                bytesWritten = numBytes;
            }
            position += bytesWritten;
            return bytesWritten;
        }

        /// <summary>
        /// If true, Read always returns the number of bytes requested
        /// </summary>
        public bool PadWithZeroes { get; set; }

        /// <summary>
        /// Converts Mono to stereo, adjusting volume and pan
        /// </summary>
        private unsafe void MonoToStereo(byte[] destBuffer, int offset, byte[] sourceBuffer, int bytesRead)
        {
            //DA SISTEMARE
            fixed (byte* pDestBuffer = &destBuffer[offset],
                pSourceBuffer = &sourceBuffer[0])
            {
                float* pfDestBuffer = (float*)pDestBuffer;
                short* psSourceBuffer = (short*)pSourceBuffer;

                // implement better panning laws. 
                float leftVolume = (pan <= 0) ? volume : (volume * (1 - pan) / 2.0f);
                float rightVolume = (pan >= 0) ? volume : (volume * (pan + 1) / 2.0f);
                leftVolume = leftVolume / 32768f;
                rightVolume = rightVolume / 32768f;
                int samplesRead = bytesRead / 2;
                BlockEffects(samplesRead);
                for (int n = 0; n < samplesRead; n++)
                {
                    pfDestBuffer[n * 2] = psSourceBuffer[n] * leftVolume;
                    pfDestBuffer[n * 2 + 1] = psSourceBuffer[n] * rightVolume;
                    ApplyEffects(ref pfDestBuffer[n * 2], ref pfDestBuffer[n * 2 + 1]);
                    if (Sample != null) RaiseSample(pfDestBuffer[n * 2], pfDestBuffer[n * 2 + 1]);
                }
            }
        }

        /// <summary>
        /// Converts stereo to stereo
        /// </summary>
        private unsafe void AdjustVolume(byte[] destBuffer, int offset, byte[] sourceBuffer, int bytesRead)
        {
            fixed (byte* pDestBuffer = &destBuffer[offset],
                pSourceBuffer = &sourceBuffer[0])
            {
                float* pfDestBuffer = (float*)pDestBuffer;

                // implement better panning laws. 
                float leftVolume = (pan <= 0) ? volume : (volume * (1 - pan) / 2.0f);
                float rightVolume = (pan >= 0) ? volume : (volume * (pan + 1) / 2.0f);

                leftVolume = leftVolume / (float)0xffffffff;;
                rightVolume = rightVolume / (float)0xffffffff;;
                //float leftVolume = (volume * (1 - pan) / 2.0f) / 32768f;
                //float rightVolume = (volume * (pan + 1) / 2.0f) / 32768f;

                int samplesRead = bytesRead / 4;
                BlockEffects(samplesRead / 2);
                for (int n = 0; n < samplesRead; n += 2)
                {
                    int leftBuffer = (pSourceBuffer[n * 4]) | (pSourceBuffer[(n * 4) + 1] << 8) | (pSourceBuffer[(n * 4) + 2] << 16) + (pSourceBuffer[(n * 4) + 3] << 24);
                    int rightBuffer = (pSourceBuffer[(n + 1) * 4]) | (pSourceBuffer[(n + 1) * 4 + 1] << 8) | (pSourceBuffer[(n + 1) * 4 + 2] << 16) + (pSourceBuffer[(n + 1) * 4 + 3] << 24);
                    pfDestBuffer[n] = leftBuffer * leftVolume;
                    pfDestBuffer[n + 1] = rightBuffer * rightVolume;
                    ApplyEffects(ref pfDestBuffer[n], ref pfDestBuffer[n + 1]);
                    if (Sample != null) RaiseSample(pfDestBuffer[n], pfDestBuffer[n + 1]);
                }
            }
        }

        /// <summary>
        /// <see cref="WaveStream.WaveFormat"/>
        /// </summary>
        public override WaveFormat WaveFormat
        {
            get
            {
                return waveFormat;
            }
        }

        /// <summary>
        /// Volume of this channel. 1.0 = full scale
        /// </summary>
        public float Volume
        {
            get
            {
                return volume;
            }
            set
            {
                volume = value;
            }
        }

        /// <summary>
        /// Pan of this channel (from -1 to 1)
        /// </summary>
        public float Pan
        {
            get
            {
                return pan;
            }
            set
            {
                pan = value;
            }
        }

        /// <summary>
        /// Determines whether this channel has any data to play
        /// to allow optimisation to not read, but bump position forward
        /// </summary>
        public override bool HasData(int count)
        {
            // Check whether the source stream has data.
            bool sourceHasData = sourceStream.HasData(count);

            if (sourceHasData)
            {
                if (position + count < 0)
                    return false;
                return (position < length) && (volume != 0);
            }
            return false;
        }

        /// <summary>
        /// Disposes this WaveStream
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (sourceStream != null)
                {
                    sourceStream.Dispose();
                    sourceStream = null;
                }
            }
            else
            {
                System.Diagnostics.Debug.Assert(false, "WaveChannel32 was not Disposed");
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Block
        /// </summary>
        public event EventHandler Block;

        private void RaiseBlock()
        {
            var handler = Block;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Sample
        /// </summary>
        public event EventHandler<SampleEventArgs> Sample;

        // reuse the same object every time to avoid making lots of work for the garbage collector
        private SampleEventArgs sampleEventArgs = new SampleEventArgs(0, 0);

        /// <summary>
        /// Raise the sample event (no check for null because it has already been done)
        /// </summary>
        private void RaiseSample(float left, float right)
        {
            sampleEventArgs.Left = left;
            sampleEventArgs.Right = right;
            Sample(this, sampleEventArgs);
        }
    }
}
