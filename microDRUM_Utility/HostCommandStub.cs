using System;
using System.Collections.Generic;
using System.Text;
using Jacobi.Vst.Core.Host;
using Jacobi.Vst.Core;

namespace microDrum
{
    class HostCommandStub:IVstHostCommandStub
    {
        public string Directory;

        #region IVstHostCommandStub Membri di

        public IVstPluginContext PluginContext { get; set; }

        #endregion

        #region IVstHostCommands20 Membri di

        public bool BeginEdit(int index)
        {
            return false;
        }

        public VstCanDoResult CanDo(VstHostCanDo cando)
        {
            return VstCanDoResult.Unknown;
        }

        public bool CloseFileSelector(VstFileSelect fileSelect)
        {
            throw new NotImplementedException();
        }

        public bool EndEdit(int index)
        {
            return false;
        }

        public VstAutomationStates GetAutomationState()
        {
            throw new NotImplementedException();
        }

        public int GetBlockSize()
        {
            return 512;
        }

        public string GetDirectory()
        {
            return Directory;
        }

        public int GetInputLatency()
        {
            throw new NotImplementedException();
        }

        public VstHostLanguage GetLanguage()
        {
            throw new NotImplementedException();
        }

        public int GetOutputLatency()
        {
            throw new NotImplementedException();
        }

        public VstProcessLevels GetProcessLevel()
        {
            //throw new NotImplementedException();
            return VstProcessLevels.Unknown;
        }

        public string GetProductString()
        {
            return "ProductString";
        }

        public float GetSampleRate()
        {
            return 44100f;
        }

        public VstTimeInfo GetTimeInfo(VstTimeInfoFlags filterFlags)
        {/*
            vstTimeInfo.samplePos = 0.0;
            vstTimeInfo.sampleRate = fSampleRate;
            vstTimeInfo.nanoSeconds = 0.0;
            vstTimeInfo.ppqPos = 0.0;
            vstTimeInfo.tempo = 120.0;
            vstTimeInfo.barStartPos = 0.0;
            vstTimeInfo.cycleStartPos = 0.0;
            vstTimeInfo.cycleEndPos = 0.0;
            vstTimeInfo.timeSigNumerator = 4;
            vstTimeInfo.timeSigDenominator = 4;
            vstTimeInfo.smpteOffset = 0;
            vstTimeInfo.smpteFrameRate = 1;
            vstTimeInfo.samplesToNextClock = 0;
            vstTimeInfo.flags = 0;
            */
            return null;
        }

        public string GetVendorString()
        {
            return "VendorString";
        }

        public int GetVendorVersion()
        {
            return 2400;
        }

        public bool IoChanged()
        {
            throw new NotImplementedException();
        }

        public bool OpenFileSelector(VstFileSelect fileSelect)
        {
            throw new NotImplementedException();
        }

        public bool ProcessEvents(VstEvent[] events)
        {
            //throw new NotImplementedException();
            return false;
        }

        public bool SizeWindow(int width, int height)
        {
            //throw new NotImplementedException();
            return false;
        }

        public bool UpdateDisplay()
        {
            return true;
        }

        #endregion

        #region IVstHostCommands10 Membri di

        public int GetCurrentPluginID()
        {
            return PluginContext.PluginInfo.PluginID;
        }

        public int GetVersion()
        {
            return 2400;
        }

        public void ProcessIdle()
        {
            return;
        }

        public void SetParameterAutomated(int index, float value)
        {
            //throw new NotImplementedException();
        }

        public VstCanDoResult CanDo(string cando)
        {
            //throw new NotImplementedException();
            return VstCanDoResult.Unknown;
        }

        
        #endregion
    }
}
