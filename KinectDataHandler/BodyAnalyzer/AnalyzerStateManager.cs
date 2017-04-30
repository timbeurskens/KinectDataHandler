using System;
using KinectDataHandler.Linear3DTools;
using Microsoft.Kinect;

namespace KinectDataHandler.BodyAnalyzer
{
    internal class AnalyzerStateManager
    {
        private SquatCompoundBodyAnalyzer _squatCompoundBodyAnalyzer;
        private KneeAngleProgressiveBodyAnalyzer _angleProgressive;
        private FootKneeConstantBodyAnalyzer _footKneeConstantBody;
        private BodyAnalyzer<double> _lengthAnalyzer;
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
            if (_footKneeConstantBody != null) _footKneeConstantBody.FloorPlane3D = p;
            if (_squatCompoundBodyAnalyzer != null) _squatCompoundBodyAnalyzer.FloorPlane = p;
        }

        private void Kl_BodyDataAvailable(Body b)
        {
            if (_squatCompoundBodyAnalyzer == null)
            {
                _squatCompoundBodyAnalyzer = new SquatCompoundBodyAnalyzer(b);
                _squatCompoundBodyAnalyzer.ValueComputed += _squatCompoundBodyAnalyzer_ValueComputed1;
            }

            if (_angleProgressive == null)
            {
                _angleProgressive = new KneeAngleProgressiveBodyAnalyzer(b, Math.PI * 0.9, 10, Math.PI / 2);
                _angleProgressive.ValueComputed += _angleProgressive_ValueComputed;
            }

            if (_lengthAnalyzer == null)
            {
                _lengthAnalyzer = new BodyLengthAnalyzer(b);
                _lengthAnalyzer.ValueComputed += _lengthAnalyzer_ValueComputed;
            }

            if (BodySerializer == null)
            {
                BodySerializer = new BodySerializer(b);
                BodySerializer.ValueComputed += _bodySerializer_ValueComputed;
            }

            if (_footKneeConstantBody == null)
            {
                _footKneeConstantBody = new FootKneeConstantBodyAnalyzer(b) {Treshold = Math.PI / 8};
                _footKneeConstantBody.ValueComputed += _footKneeConstantBody_ValueComputed;
            }

            _squatCompoundBodyAnalyzer.PassBody(b);
            Console.WriteLine(_squatCompoundBodyAnalyzer.GetValue());
//            _lengthAnalyzer.PassBody(b);
//            BodySerializer.PassBody(b);
//            _footKneeConstantBody.PassBody(b);
//            _angleProgressive.PassBody(b);

            //Console.WriteLine(_footKneeConstantBody.GetValue());
        }

        private void _squatCompoundBodyAnalyzer_ValueComputed1(ProgressiveBodyAnalyzerState value)
        {
            Console.WriteLine(value);
        }
        
        private void _angleProgressive_ValueComputed(ProgressiveBodyAnalyzerState value)
        {
            Console.WriteLine(value.ToString());
        }

        private void _footKneeConstantBody_ValueComputed(bool value)
        {
            //Console.WriteLine("CONSTANT FAILED");
        }

        private void _bodySerializer_ValueComputed(string value)
        {
            //Console.WriteLine(BodySerializer.GetValue());
            _com.SendToAll(BodySerializer.GetValue());
        }

        private void _lengthAnalyzer_ValueComputed(double value)
        {
            //Console.WriteLine("Body length: " + value);
            //_lengthAnalyzer.Reset();
        }

        public void Reset()
        {
            _squatCompoundBodyAnalyzer?.Reset();
            _footKneeConstantBody?.Reset();
            BodySerializer?.Reset();
            _lengthAnalyzer?.Reset();
            _angleProgressive?.Reset();
        }
    }
}
