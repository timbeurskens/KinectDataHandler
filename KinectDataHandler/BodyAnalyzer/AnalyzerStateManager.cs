using System;
using KinectDataHandler.Linear3DTools;
using Microsoft.Kinect;

namespace KinectDataHandler.BodyAnalyzer
{
    internal class AnalyzerStateManager
    {
        private SquatCompoundBodyAnalyzer _squatCompoundBodyAnalyzer;
        public BodyAnalyzer<string> BodySerializer;
        private readonly FeedbackComLink _com = new FeedbackComLink();

        public AnalyzerStateManager(KinectLink kl)
        {
            kl.BodyDataAvailable += Kl_BodyDataAvailable;
            kl.FloorPlaneAvailable += Kl_FloorPlaneAvailable;
            _com.Open();
        }

        private void Kl_FloorPlaneAvailable(Plane3D p)
        {
            if (_squatCompoundBodyAnalyzer != null) _squatCompoundBodyAnalyzer.FloorPlane3D = p;
        }

        private void Kl_BodyDataAvailable(Body b)
        {
            if (_squatCompoundBodyAnalyzer == null)
            {
                _squatCompoundBodyAnalyzer = new SquatCompoundBodyAnalyzer(b, Math.PI * 0.9, Math.PI / 2, 10, Math.PI / 7);
                _squatCompoundBodyAnalyzer.ValueComputed += _squatCompoundBodyAnalyzer_ValueComputed1;
            }

            if (BodySerializer == null)
            {
                BodySerializer = new BodySerializer(b);
                BodySerializer.ValueComputed += _bodySerializer_ValueComputed;
            }

            _squatCompoundBodyAnalyzer.PassBody(b);
            Console.WriteLine(_squatCompoundBodyAnalyzer.GetValue());
            Console.WriteLine(_squatCompoundBodyAnalyzer.GetProgressiveAnalyzer().GetProgress());
        }

        private void _squatCompoundBodyAnalyzer_ValueComputed1(ProgressiveBodyAnalyzerState value)
        {
            //reset on fail or success
            //            if (value == ProgressiveBodyAnalyzerState.Success || value == ProgressiveBodyAnalyzerState.Failed)
            //            {
            //                _squatCompoundBodyAnalyzer.Reset();
            //            }
            //            Console.WriteLine(value);
        }

        private void _bodySerializer_ValueComputed(string value)
        {
            //Console.WriteLine(BodySerializer.GetValue());
            _com.SendToAll(BodySerializer.GetValue());
        }

        public void Reset()
        {
            _squatCompoundBodyAnalyzer?.Reset();
            BodySerializer?.Reset();
        }
    }
}
