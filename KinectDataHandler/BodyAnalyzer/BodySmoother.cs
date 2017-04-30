using System.Collections.Generic;
using KinectDataHandler.SpecialStructures;
using Microsoft.Kinect;

namespace KinectDataHandler.BodyAnalyzer
{
    class BodySmoother : BodyAnalyzer<IReadOnlyDictionary<JointType, Joint>>
    {
        private FloodQueue<IDictionary<JointType, Joint>> _jointQueue = new FloodQueue<IDictionary<JointType, Joint>>(3);

        public BodySmoother(Body b) : base(b)
        {

        }

        public BodySmoother(ulong trackingId) : base(trackingId)
        {

        }

        protected override bool HandleBody(Body b)
        {
            _jointQueue.Add(b.Joints as IDictionary<JointType, Joint>);
            return true;
        }

        public override IReadOnlyDictionary<JointType, Joint> GetValue()
        {
            IDictionary<JointType, Joint> result = new Dictionary<JointType, Joint>();

            double parts = _jointQueue.Count;
            foreach (var element in _jointQueue)
            {
                foreach (var joint in element)
                {
                    var j = new Joint {Position = new CameraSpacePoint {X = 0}};

                }
            }

            return (IReadOnlyDictionary<JointType, Joint>) result;
        }

        protected override void DoReset()
        {
            _jointQueue.Clear();
        }
    }
}
