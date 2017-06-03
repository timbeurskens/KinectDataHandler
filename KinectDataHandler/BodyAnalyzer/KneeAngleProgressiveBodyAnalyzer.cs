using System;
using KinectDataHandler.Linear3DTools;
using Microsoft.Kinect;

namespace KinectDataHandler.BodyAnalyzer
{
    public class KneeAngleProgressiveBodyAnalyzer : ProgressiveBodyAnalyzer
    {
        private readonly JointTriple _kneeLeft = new JointTriple(JointType.AnkleLeft, JointType.KneeLeft,
            JointType.HipLeft);

        private readonly JointTriple _kneeRight = new JointTriple(JointType.AnkleRight, JointType.KneeRight,
            JointType.HipRight);

        public KneeAngleProgressiveBodyAnalyzer(Body b, double start, int steps, double end)
            : base(b, start, steps, end)
        {
        }

        public KneeAngleProgressiveBodyAnalyzer(ulong trackingId, double start, int steps, double end)
            : base(trackingId, start, steps, end)
        {
        }

        public override bool HandleBody(Body b)
        {
            //for now, take average of left, right
            try
            {
                var leftAngle = _kneeLeft.GetAngle(b);
                var rightAngle = _kneeRight.GetAngle(b);
                var avgAngle = (leftAngle + rightAngle) / 2;
                var p = Map(avgAngle);
                CurrentStep = (int) Math.Floor(p * NumSteps);

                //Console.WriteLine(_position);
            }
            catch (NotTrackedException)
            {
                return false;
            }

            return true;
        }
    }
}