using KinectDataHandler.Linear3DTools;
using Microsoft.Kinect;

namespace KinectDataHandler.BodyAnalyzer
{
    public class FootKneeConstantBodyAnalyzer : ConstantBodyAnalyzer
    {
        public Plane3D FloorPlane3D { get; set; }
        public double Treshold { get; set; }
        private readonly JointTuple _left = new JointTuple(JointType.FootLeft, JointType.KneeLeft);
        private readonly JointTuple _right = new JointTuple(JointType.FootRight, JointType.KneeRight);

        public FootKneeConstantBodyAnalyzer(Body b) : base(b)
        {
        }

        public FootKneeConstantBodyAnalyzer(ulong trackingId) : base(trackingId)
        {
        }

        protected override bool CheckBody(Body b)
        {
            if (FloorPlane3D == null) return true;

            var n = FloorPlane3D.Normal();

            var angleL = 0.0;
            var angleR = 0.0;

            try
            {
                var vjtl = _left.GetVector3D(b);
                angleL = n.Angle(vjtl);
            }
            catch (NotTrackedException)
            {
            }

            try
            {
                var vjtr = _right.GetVector3D(b);
                angleR = n.Angle(vjtr);
            }
            catch (NotTrackedException)
            {
            }

            return angleL < Treshold && angleR < Treshold;
        }
    }
}