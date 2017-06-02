using System;
using System.Runtime.Remoting.Messaging;
using KinectDataHandler.Linear3DTools;
using Microsoft.Kinect;

namespace KinectDataHandler
{
    internal class KinectLink
    {
        //sensor and body frame reader
        private readonly KinectSensor _sensor;
        private BodyFrameReader _reader;

        public delegate void FloorPlaneAvailableListener(Plane3D p);

        public event FloorPlaneAvailableListener FloorPlaneAvailable;

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
                var fp = KinectMathAdapter.Plane3DFromVector4(frame.FloorClipPlane);

                OnFloorPlaneAvailable(fp);

                var bodies = new Body[frame.BodyCount];
                frame.GetAndRefreshBodyData(bodies);
                foreach (var body in bodies)
                {
                    if (!body.IsTracked) continue;
                    BodyDataAvailable?.BeginInvoke(body, EndAsyncEvent, null);
                }
            }
        }

        private static void EndAsyncEvent(IAsyncResult ar)
        {
            var r = ar as AsyncResult;
            var m = r?.AsyncDelegate as EventHandler;
            m?.EndInvoke(r);
        }

        public void Close()
        {
            _sensor.Close();
        }

        protected virtual void OnFloorPlaneAvailable(Plane3D p)
        {
            FloorPlaneAvailable?.BeginInvoke(p, EndAsyncEvent, null);
        }
    }
}