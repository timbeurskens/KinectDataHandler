using System;
using System.Collections.Generic;
using System.Linq;
using KinectDataHandler.BodyAnalyzer;
using Microsoft.Kinect;

namespace KinectDataHandler.Linear3DTools
{
    internal class KinectMathAdapter
    {
        public static IPoint3D Point3DFromCameraSpacePoint(CameraSpacePoint csp)
        {
            return new Vector3D(csp.X, csp.Y, csp.Z);
        }

        public static Vector3D Vector3DFromJointTuple(JointTuple t, Body b)
        {
            return t.GetVector3D(b);
        }

        public static Plane3D Plane3DFromVector4(Vector4 v4)
        {
            return new Plane3D(v4.X, v4.Y, v4.Z, v4.W);
        }
    }

    internal class JointTriple
    {
        public JointType Joint1 { get; }
        public JointType Joint2 { get; }
        public JointType Joint3 { get; }

        private JointTuple _t1, _t2;

        public JointTriple(JointType joint1, JointType joint2, JointType joint3)
        {
            Joint2 = joint2;
            Joint1 = joint1;
            Joint3 = joint3;

            InitializeTuples();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="j1">tuple j1 such that exactly one element in j1 overlaps with j2</param>
        /// <param name="j2">tuple j2 such that exactly one element in j2 overlaps with j1</param>
        public JointTriple(JointTuple j1, JointTuple j2)
        {
            if (j1.Joint1 == j2.Joint1)
            {
                Joint2 = j1.Joint1;
                Joint1 = j1.Joint2;
                Joint3 = j2.Joint2;
            } else if (j1.Joint1 == j2.Joint2)
            {
                Joint2 = j1.Joint1;
                Joint1 = j1.Joint2;
                Joint3 = j2.Joint1;
            } else if (j1.Joint2 == j2.Joint2)
            {
                Joint2 = j1.Joint2;
                Joint1 = j1.Joint1;
                Joint3 = j2.Joint1;
            }
            else
            {
                throw new ArgumentException("No overlapping joint types");
            }

            if (Joint1 == Joint3)
            {
                throw new ArgumentException("Joints overlap completely");
            }

            InitializeTuples();
        }

        private void InitializeTuples()
        {
            _t1 = new JointTuple(Joint2, Joint1);   
            _t2 = new JointTuple(Joint2, Joint3);
        }

        public double GetAngle(Body b)
        {
            var v1 = _t1.GetVector3D(b);
            var v2 = _t2.GetVector3D(b);

            return v1.Angle(v2);
        }

        public static double GetTotalJointAngle(Body b, IEnumerable<JointTriple> jList)
        {
            return jList.Sum(jc => jc.GetAngle(b));
        }

        public static double GetAverageJointAngle(Body b, IEnumerable<JointTriple> jList)
        {
            return jList.Average(jc => jc.GetAngle(b));
        }
    }

    internal class JointTuple
    {
        public JointType Joint1 { get; }
        public JointType Joint2 { get; }

        public JointTuple(JointType joint1, JointType joint2)
        {
            Joint2 = joint2;
            Joint1 = joint1;
        }

        public double GetLength(Body b)
        {
            var v = GetVector3D(b);

            return v.Length();
        }

        public Vector3D GetVector3D(Body b)
        {
            Joint j1, j2;
            if (!b.Joints.TryGetValue(Joint1, out j1) || !b.Joints.TryGetValue(Joint2, out j2))
                throw new NotTrackedException("Could not retrieve one or more joints.");

            if (j1.TrackingState != TrackingState.Tracked || j2.TrackingState != TrackingState.Tracked)
                throw new NotTrackedException("One or more joints are not tracked");

            var p1 = KinectMathAdapter.Point3DFromCameraSpacePoint(j1.Position);
            var p2 = KinectMathAdapter.Point3DFromCameraSpacePoint(j2.Position);

            return Vector3D.FromPoints(p1, p2);
        }

        public static double GetTotalJointDistance(Body b, IEnumerable<JointTuple> jcList)
        {
            return jcList.Sum(jc => jc.GetLength(b));
        }

        public static double GetAverageJointDistance(Body b, IEnumerable<JointTuple> jList)
        {
            return jList.Average(jc => jc.GetLength(b));
        }
    }
}
