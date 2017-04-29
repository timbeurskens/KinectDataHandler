using System;
using System.Security.Permissions;
using Microsoft.Kinect;

namespace KinectDataHandler.BodyAnalyzer
{
    internal class AnalyzerStateManager
    {
        private BodyAnalyzer<double> _lengthAnalyzer;
        public BodyAnalyzer<string> BodySerializer;
        private readonly FeedbackComLink _com = new FeedbackComLink();

        public AnalyzerStateManager(KinectLink kl)
        {
            kl.BodyDataAvailable += Kl_BodyDataAvailable;
            _com.Open();
        }

        private void Kl_BodyDataAvailable(Microsoft.Kinect.Body b)
        {
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

            _lengthAnalyzer.PassBody(b);
            BodySerializer.PassBody(b);
        }

        private void _bodySerializer_ValueComputed(string value)
        {
            Console.WriteLine(BodySerializer.GetValue());
            _com.SendToAll(BodySerializer.GetValue());
        }

        private void _lengthAnalyzer_ValueComputed(double value)
        {
            Console.WriteLine("Body length: " + value);
            //_lengthAnalyzer.Reset();
        }
    }
}
