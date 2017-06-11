using System;
using ExternalCommunicationLibrary;
using ExternalCommunicationLibrary.Messages;
using KinectDataHandler.Linear3DTools;
using KinectDataHandler.Properties;
using Microsoft.Kinect;

namespace KinectDataHandler.BodyAnalyzer
{
    /// <summary>
    /// todo: reset tracking id
    /// todo: handle control commands
    /// </summary>
    internal class AnalyzerStateManager
    {
        private SquatCompoundBodyAnalyzer _squatCompoundBodyAnalyzer;
        public BodyAnalyzer<string> BodySerializer;

        private readonly Server _server;
        private const int FrameTransmitInterval = 2;
        private int _frameTransmitCounter;

        private bool _setNextBody;

        public AnalyzerStateManager(KinectLink kl)
        {
            kl.BodyDataAvailable += Kl_BodyDataAvailable;
            kl.FloorPlaneAvailable += Kl_FloorPlaneAvailable;
        }

        public AnalyzerStateManager(KinectLink kl, Server s) : this(kl)
        {
            _server = s;
            _server.MessageAvailable += _server_MessageAvailable;
        }

        public void BodyReset()
        {
            _setNextBody = true;
        }

        private void _server_MessageAvailable(Message m)
        {
            if (m.GetMessageType() != MessageType.Control) return;

            var message = m as ControlMessage;
            var command = message?.Command;

            if (command == null) return;

            switch (command.Type)
            {
                case CommandType.ExerciseSelect:
                    //only squat
                    break;
                case CommandType.ExerciseStart:
                    //start exercise:
                    StartSession();
                    break;
                case CommandType.ExerciseStop:
                    //stop exercise:
                    //disable analyzers
                    StopSession();
                    break;
                case CommandType.ExerciseTimeLimit:
                    break;
                case CommandType.ExerciseSessionLength:
                    break;
                case CommandType.ExerciseSeriesLength:
                    break;
                case CommandType.ExerciseBreakLength:
                    break;
                case CommandType.BodyReset:
                    //reset body ids
                    BodyReset();
                    break;
                default:
                    break;
            }
        }

        public void StartSession()
        {
            if (!EnableAll()) return;
            Console.WriteLine(Resources.AnalyzerStateManager_StartSession_Session_started);
            _server.Send(new ControlMessage(new Command(CommandType.SessionStarted, 0, 0)));
        }

        public void StopSession()
        {
            if (!DisableAll()) return;
            Console.WriteLine(Resources.AnalyzerStateManager_StopSession_Session_stopped);
            _server.Send(new ControlMessage(new Command(CommandType.SessionStopped, 0, 0)));
        }

        private bool EnableAll()
        {
            if (BodySerializer == null || _squatCompoundBodyAnalyzer == null) return false;
            BodySerializer.Enable();
            _squatCompoundBodyAnalyzer.Enable();
            return true;
        }

        private bool DisableAll()
        {
            if (BodySerializer == null || _squatCompoundBodyAnalyzer == null) return false;
            BodySerializer.Disable();
            BodySerializer.Reset();
            _squatCompoundBodyAnalyzer.Disable();
            _squatCompoundBodyAnalyzer.Reset();
            return true;
        }

        private void Kl_FloorPlaneAvailable(Plane3D p)
        {
            if (_squatCompoundBodyAnalyzer != null) _squatCompoundBodyAnalyzer.FloorPlane3D = p;
        }

        private void Kl_BodyDataAvailable(Body b)
        {
            if (_squatCompoundBodyAnalyzer == null)
            {
                _squatCompoundBodyAnalyzer = new SquatCompoundBodyAnalyzer(b, Math.PI * 0.9, Math.PI / 2, 10,
                    Math.PI / 7);
                _squatCompoundBodyAnalyzer.ValueComputed += _squatCompoundBodyAnalyzer_ValueComputed1;
            }

            if (BodySerializer == null)
            {
                BodySerializer = new BodySerializer(b);
                BodySerializer.ValueComputed += _bodySerializer_ValueComputed;
            }

            _squatCompoundBodyAnalyzer?.PassBody(b);
            
//            Console.WriteLine(_squatCompoundBodyAnalyzer.GetValue());
//            Console.WriteLine(_squatCompoundBodyAnalyzer.GetProgressiveAnalyzer().GetProgress());
//            Console.WriteLine(_squatCompoundBodyAnalyzer.GetProgressiveAnalyzer().Active);

            if (_frameTransmitCounter == 0)
            {
                BodySerializer?.PassBody(b);
            }
            _frameTransmitCounter = (_frameTransmitCounter + 1) % FrameTransmitInterval;

            if (_setNextBody && _squatCompoundBodyAnalyzer != null && BodySerializer != null)
            {
                Console.WriteLine("Tracking set to: " + b.TrackingId);
                _squatCompoundBodyAnalyzer.SetTrackingId(b.TrackingId);
                BodySerializer.SetTrackingId(b.TrackingId);
                _setNextBody = false;
            }
        }

        private void _squatCompoundBodyAnalyzer_ValueComputed1(ProgressiveBodyAnalyzerState value)
        {
            Console.WriteLine(value);

            var c = new Command(CommandType.ExerciseStatus, (int) value, 0);
            _server?.Send(new ControlMessage(c));

            switch (value)
            {
                case ProgressiveBodyAnalyzerState.Success:
                    _squatCompoundBodyAnalyzer.Reset();
                    break;
                case ProgressiveBodyAnalyzerState.Failed:
                    _squatCompoundBodyAnalyzer.SoftReset();
                    break;
            }
        }

        private void _bodySerializer_ValueComputed(string value)
        {
            //Console.WriteLine(BodySerializer.GetValue());

            _server?.Send(new BodyMessage(BodySerializer.GetValue()));
        }

        public void Reset()
        {
            _squatCompoundBodyAnalyzer?.Reset();
            BodySerializer?.Reset();
        }
    }
}