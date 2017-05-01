using System;
using System.Collections.ObjectModel;
using KinectDataHandler.Linear3DTools;
using Microsoft.Kinect;

namespace KinectDataHandler.BodyAnalyzer
{
    public class SquatCompoundBodyAnalyzer : CompoundBodyAnalyzer
    {
        private readonly FootKneeConstantBodyAnalyzer _footKneeConstantBodyAnalyzer;
        private readonly KneeAngleProgressiveBodyAnalyzer _kneeAngleProgressiveBodyAnalyzer;

        public Plane3D FloorPlane3D
        {
            get { return _footKneeConstantBodyAnalyzer.FloorPlane3D; }
            set { _footKneeConstantBodyAnalyzer.FloorPlane3D = value; }
        }

        public SquatCompoundBodyAnalyzer(Body b) : base(b)
        {
            _footKneeConstantBodyAnalyzer = new FootKneeConstantBodyAnalyzer(TrackingId)
            {
                Treshold = Math.PI / 7
            };
            _kneeAngleProgressiveBodyAnalyzer = new KneeAngleProgressiveBodyAnalyzer(TrackingId, Math.PI * 0.9, 10, Math.PI / 2);
        }

        public SquatCompoundBodyAnalyzer(ulong trackingId) : base(trackingId)
        {
            _footKneeConstantBodyAnalyzer = new FootKneeConstantBodyAnalyzer(TrackingId)
            {
                Treshold = Math.PI / 7
            };
            _kneeAngleProgressiveBodyAnalyzer = new KneeAngleProgressiveBodyAnalyzer(TrackingId, Math.PI * 0.9, 10, Math.PI / 2);
        }

        public override Collection<ConstantBodyAnalyzer> GetConstantAnalyzers()
        {

            return new Collection<ConstantBodyAnalyzer>
            {
                _footKneeConstantBodyAnalyzer
            };
        }

        public override ProgressiveBodyAnalyzer GetProgressiveAnalyzer()
        {
            return _kneeAngleProgressiveBodyAnalyzer;
        }
    }
}