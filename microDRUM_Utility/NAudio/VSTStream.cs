using System;
using System.Collections.Generic;
using System.Text;
using NAudio.Wave;
using Jacobi.Vst.Interop.Host;
using Jacobi.Vst.Core;

namespace microDrum
{
    public class VSTStreamEventArgs : EventArgs
    {
        public float MaxL = float.MinValue;
        public float MaxR = float.MaxValue;

        public VSTStreamEventArgs(float maxL, float maxR)
        {
            MaxL = maxL;
            MaxR = maxR;
        }
    }

    class VSTStream : /*IWaveProvider*/WaveStream /*WaveProvider32*/
    {
        public VstPluginContext pluginContext = null;
        public event EventHandler<VSTStreamEventArgs> ProcessCalled;

        private int BlockSize = 0;
        VstAudioBuffer[] inputBuffers;
        VstAudioBuffer[] outputBuffers;
        float[] output;

        private void RaiseProcessCalled(float maxL, float maxR)
        {
            EventHandler<VSTStreamEventArgs> handler = ProcessCalled;

            if (handler != null)
            {
                handler(this, new VSTStreamEventArgs(maxL, maxR));
            }
        }

        private void UpdateBlockSize(int blockSize)
        {
            BlockSize = blockSize;

            int outputCount = pluginContext.PluginInfo.AudioOutputCount;

            VstAudioBufferManager outputMgr = new VstAudioBufferManager(outputCount, blockSize);

            pluginContext.PluginCommandStub.SetSampleRate(44800f);
            pluginContext.PluginCommandStub.SetBlockSize(blockSize);

            inputBuffers = new VstAudioBuffer[0];
            outputBuffers = outputMgr.ToArray();


            output = new float[2 * blockSize];

        }
        private float[] ProcessReplace(int blockSize)
        {
            if (blockSize != BlockSize) UpdateBlockSize(blockSize);

            try
            {
                //pluginContext.PluginCommandStub.MainsChanged(true);
                pluginContext.PluginCommandStub.StartProcess();
                pluginContext.PluginCommandStub.ProcessReplacing(inputBuffers, outputBuffers);
                pluginContext.PluginCommandStub.StopProcess();
                //pluginContext.PluginCommandStub.MainsChanged(false);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

            int indexOutput = 0;

            float maxL = float.MinValue;
            float maxR = float.MinValue;

            for (int j = 0; j < BlockSize; j++)
            {
                output[indexOutput] = outputBuffers[0][j];
                output[indexOutput + 1] = outputBuffers[1][j];// outputBuffers[1][j];

                maxL = Math.Max(maxL, output[indexOutput]);
                maxR = Math.Max(maxR, output[indexOutput + 1]);
                indexOutput += 2;
                /*if (outputBuffers[0][j] != 0.0f)
                {
                    int qqq = 0;
                }*/

            }
            RaiseProcessCalled(maxL, maxR);
            //outputMgr.ClearAllBuffers();


            return output;

        }

        public /*override*/ int Read(float[] buffer, int offset, int sampleCount)
        {
            // CALL VST PROCESS HERE WITH BLOCK SIZE OF sampleCount
            float[] tempBuffer = ProcessReplace(sampleCount / 2);

            //for (int i = 0; i < buffer.Length; i++)
            //    buffer[i] = 0.0f;
            // Copying Vst buffer inside Audio buffer, no conversion needed for WaveProvider32
            for (int i = 0; i < sampleCount; i++)
                buffer[i + offset] = tempBuffer[i];

            return sampleCount;
        }


        private WaveFormat waveFormat;

        public void SetWaveFormat(int sampleRate, int channels)
        {
            this.waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channels);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            WaveBuffer waveBuffer = new WaveBuffer(buffer);
            int samplesRequired = count / 4;
            int samplesRead = Read(waveBuffer.FloatBuffer, offset / 4, samplesRequired);
            return samplesRead * 4;
        }
        public override WaveFormat WaveFormat
        {
            get { return waveFormat; }
        }


        public override long Length
        {
            get { return long.MaxValue; }
        }

        public override long Position
        {
            get
            {
                return 0;
            }
            set
            {
                long x = value;
                //throw new NotImplementedException();
            }
        }
    }
}
