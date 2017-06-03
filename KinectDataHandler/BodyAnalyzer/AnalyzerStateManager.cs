using System;
using ExternalCommunicationLibrary;
using KinectDataHandler.Linear3DTools;
using Microsoft.Kinect;

namespace KinectDataHandler.BodyAnalyzer
{
    internal class AnalyzerStateManager
    {
        private SquatCompoundBodyAnalyzer _squatCompoundBodyAnalyzer;
        public BodyAnalyzer<string> BodySerializer;
        private readonly Server _server;
        
        public AnalyzerStateManager(KinectLink kl)
        {
            kl.BodyDataAvailable += Kl_BodyDataAvailable;
            kl.FloorPlaneAvailable += Kl_FloorPlaneAvailable;
        }

        public AnalyzerStateManager(KinectLink kl, Server s) : this(kl)
        {
            _server = s;
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

            _squatCompoundBodyAnalyzer.PassBody(b);
            BodySerializer.PassBody(b);
            Console.WriteLine(_squatCompoundBodyAnalyzer.GetValue());
            Console.WriteLine(_squatCompoundBodyAnalyzer.GetProgressiveAnalyzer().GetProgress());
        }

        private void _squatCompoundBodyAnalyzer_ValueComputed1(ProgressiveBodyAnalyzerState value)
        {
            Console.WriteLine(value);
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

            _server?.Send(new SimpleMessage(MessageType.Body, BodySerializer.GetValue()));
        }

        public void Reset()
        {
            _squatCompoundBodyAnalyzer?.Reset();
            BodySerializer?.Reset();
        }
    }
}