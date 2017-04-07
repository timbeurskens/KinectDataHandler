using KinectDataHandler.Linear3DTools;
using Microsoft.Kinect;

namespace KinectDataHandler
{
    internal class KinectLink
    {
        //sensor and body frame reader
        private readonly KinectSensor _sensor;
        private BodyFrameReader _reader;

        public delegate void BodyDataAvailableListener(Body b);
        public event BodyDataAvailableListener BodyDataAvailable;

        public bool IsOpen => _sensor.IsOpen;
        public bool IsAvailable => _sensor.IsAvailable;

        public KinectLink()
        {
            _sensor = KinectSensor.GetDefault();
        }

        public void Open()
        {
            _sensor.Open();

            _reader = _sensor.BodyFrameSource.OpenReader();
            _reader.FrameArrived += _reader_FrameArrived;
        }

        private void _reader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            using (var frame = e.FrameReference.AcquireFrame())
            {
                var p = KinectMathAdapter.Plane3DFromVector4(frame.FloorClipPlane);

                //TODO: handle floor plane

                var bodies = new Body[frame.BodyCount];
                frame.GetAndRefreshBodyData(bodies);
                foreach (var body in bodies)
                {
                    if (!body.IsTracked) continue;
                    BodyDataAvailable?.BeginInvoke(body, ar => { }, this);
                }
            }
        }

        public void Close()
        {
            _sensor.Close();
        }
    }
}
