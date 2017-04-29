using System;
using System.Collections.Generic;
using System.Linq;
using KinectDataHandler.Linear3DTools;
using KinectDataHandler.SpecialStructures;
using Microsoft.Kinect;

namespace KinectDataHandler.BodyAnalyzer
{
    internal class BodyLengthAnalyzer : BodyAnalyzer<double>
    {
        private readonly FloodQueue<double> _fq = new FloodQueue<double>(25);

        public BodyLengthAnalyzer(Body b) : base(b)
        {
            _fq.FloodQueueCapacityReached += _fq_FloodQueueCapacityReached;
        }

        private void _fq_FloodQueueCapacityReached(FloodQueue<double> queue)
        {
            OnValueComputed(GetValue());
            ResetOnValueComputedEvent();
        }

        public BodyLengthAnalyzer(ulong trackingId) : base(trackingId)
        {
            _fq.FloodQueueCapacityReached += _fq_FloodQueueCapacityReached;
        }

        /// <summary>
        /// Passes the body object to the analyzer to calculate body length
        /// </summary>
        /// <param name="b">Body object</param>
        /// <modifies>_n, _bodyLength</modifies>
        /// <returns>True when body data is correctly handled, false otherwise</returns>
        protected override bool HandleBody(Body b)
        {
            try
            {
                _fq.Add(GetBodyLength(b));
            }
            catch (NotTrackedException e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the measured height of a given body
        /// </summary>
        /// <param name="b">Input body</param>
        /// <returns>Height of body</returns>
        private static double GetBodyLength(Body b)
        {
            var jcUpper = new List<JointTuple>
            {
                new JointTuple(JointType.Head, JointType.Neck),
                new JointTuple(JointType.Neck, JointType.SpineShoulder),
                new JointTuple(JointType.SpineShoulder, JointType.SpineMid),
                new JointTuple(JointType.SpineMid, JointType.SpineBase)
            };

            var jcLowerLeft = new List<JointTuple>
            {
                new JointTuple(JointType.HipLeft, JointType.KneeLeft),
                new JointTuple(JointType.KneeLeft, JointType.AnkleLeft)
            };

            var jcLowerRight = new List<JointTuple>
            {
                new JointTuple(JointType.HipRight, JointType.KneeRight),
                new JointTuple(JointType.KneeRight, JointType.AnkleRight)
            };

            var upperLength = JointTuple.GetTotalJointDistance(b, jcUpper);

            //average left right
            var lowerLength = (JointTuple.GetTotalJointDistance(b, jcLowerLeft) + JointTuple.GetTotalJointDistance(b, jcLowerRight)) / 2;

            return upperLength + lowerLength;
        }

        public double GetBodyLength()
        {
            var t = (FloodQueue<double>)_fq.Clone();
            return t.Average();
        }

        public override double GetValue()
        {
            return GetBodyLength();
        }

        protected override void DoReset()
        {
            _fq.Clear();
        }
    }
}
