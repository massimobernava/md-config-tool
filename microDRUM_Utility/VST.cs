﻿using System;
using System.Collections.Generic;
using System.Text;
using Jacobi.Vst.Interop.Host;
using Jacobi.Vst.Core;

namespace microDrum
{
    public class VST
    {
        public VstPluginContext pluginContext = null;
        public event EventHandler<VSTStreamEventArgs> StreamCall=null;
        
        EditParametersForm edit = new EditParametersForm();

        internal void Dispose()
        {
            edit.Close();
            if(pluginContext!=null) pluginContext.Dispose();
        
        }
        public void MIDI_NoteOn(byte Note, byte Velocity)
        {
            byte Cmd = 0x90;
            //NoteMapForm.Map(ref Cmd,ref Note);
            MIDI(Cmd, Note, Velocity);
        }

        public void MIDI_CC(byte Number, byte Value)
        {
            /*byte[] midiData = new byte[4];
            midiData[0] = 176; //0xB0
            midiData[1] = Number;
            midiData[2] = Value;
            midiData[3] = 0;    // Reserved, unused 

            VstMidiEvent vse = new VstMidiEvent( 0,
               0,
                0,
                 midiData,
                 0,
               127);

            VstEvent[] ve = new VstEvent[1];
            ve[0] = vse;

            pluginContext.PluginCommandStub.ProcessEvents(ve);*/

            byte Cmd = 0xB0;
            //NoteMapForm.Map(ref Cmd, ref Number);
            MIDI(Cmd, Number, Value);
        }

        private void MIDI(byte Cmd,byte Val1,byte Val2)
        {
            byte[] midiData = new byte[4];
            midiData[0] = Cmd;
            midiData[1] = Val1;
            midiData[2] = Val2;
            midiData[3] = 0;    // Reserved, unused 

            VstMidiEvent vse = new VstMidiEvent(/*DeltaFrames*/ 0,
                /*NoteLength*/ 0,
                /*NoteOffset*/  0,
                 midiData,
                /*Detune*/        0,
                /*NoteOffVelocity*/ 127);

            VstEvent[] ve = new VstEvent[1];
            ve[0] = vse;

            pluginContext.PluginCommandStub.ProcessEvents(ve);
        }
        internal void ShowEditParameters()
        {
            edit.AddParameters(pluginContext);
            edit.Show();
        }
        internal void Stream_ProcessCalled(object sender, VSTStreamEventArgs e)
        {
            if (StreamCall != null) StreamCall(sender, e);
        }
    }
}
