using KinectDataHandler.Linear3DTools;
using Microsoft.Kinect;

namespace KinectDataHandler.BodyAnalyzer
{
    public class BodySerializer : BodyAnalyzer<string>
    {
        private string _serializedBody;

        public BodySerializer(Body b) : base(b)
        {
        }

        public BodySerializer(ulong trackingId) : base(trackingId)
        {
        }

        /// <summary>
        /// Handles the passed body
        /// </summary>
        /// <param name="b">Body object instance</param>
        /// <pre>b.isTracked == true && b.trackingId == trackingId</pre>
        /// <returns></returns>
        public override bool HandleBody(Body b)
        {
            var result = true;

            _serializedBody = "[BEGIN_BODY]\n";

            foreach (var j in b.Joints)
            {
                if (j.Value.TrackingState != TrackingState.Tracked)
                {
                    result = false;
                }
                _serializedBody += j.Key + ":";
                _serializedBody += KinectMathAdapter.Point3DFromCameraSpacePoint(j.Value.Position) + "\n";
            }

            _serializedBody += "[END_BODY]\n";

            if (!result) return false;
            OnValueComputed(_serializedBody);
            ResetOnValueComputedEvent();

            return true;
        }

        public override string GetValue()
        {
            return _serializedBody;
        }

        protected override void DoReset()
        {
            //throw new NotImplementedException();
        }
    }
}