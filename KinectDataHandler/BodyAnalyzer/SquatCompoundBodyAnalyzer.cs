using System;
using System.Collections.ObjectModel;
using KinectDataHandler.Linear3DTools;
using Microsoft.Kinect;

namespace KinectDataHandler.BodyAnalyzer
{
    /// <summary>
    /// Analyzes the body to detect squat exercises.
    /// Assumptions:
    /// A squat can be detected by computing the knee angle.
    /// Squat corrections that exceed a specified treshold can be considered faulty
    /// A squat correction is either moving up when the desired direction is down or -
    ///     - vice versa.
    /// 
    /// Constraints:
    /// The knee joint must be positioned above the foot joint
    /// (an allowed deviation can be specified)
    /// </summary>
    public class SquatCompoundBodyAnalyzer : CompoundBodyAnalyzer
    {
        //define seperate components
        private readonly FootKneeConstantBodyAnalyzer _footKneeConstantBodyAnalyzer;
        private readonly KneeAngleProgressiveBodyAnalyzer _kneeAngleProgressiveBodyAnalyzer;

        //FootKneeConstantBodyAnalyzer requires a FloorPlane
        public Plane3D FloorPlane3D
        {
            get { return _footKneeConstantBodyAnalyzer.FloorPlane3D; }
            set { _footKneeConstantBodyAnalyzer.FloorPlane3D = value; }
        }

        public SquatCompoundBodyAnalyzer(Body b, double startTreshold, double goalTreshold, int stepCount, double stabilityTreshold) : base(b)
        {
            _footKneeConstantBodyAnalyzer = new FootKneeConstantBodyAnalyzer(TrackingId)
            {
                Treshold = stabilityTreshold
            };
            _kneeAngleProgressiveBodyAnalyzer = new KneeAngleProgressiveBodyAnalyzer(TrackingId, startTreshold, stepCount, goalTreshold);
        }

        public SquatCompoundBodyAnalyzer(ulong trackingId, double startTreshold, double goalTreshold, int stepCount, double stabilityTreshold) : base(trackingId)
        {
            _footKneeConstantBodyAnalyzer = new FootKneeConstantBodyAnalyzer(TrackingId)
            {
                Treshold = stabilityTreshold
            };
            _kneeAngleProgressiveBodyAnalyzer = new KneeAngleProgressiveBodyAnalyzer(TrackingId, startTreshold, stepCount, goalTreshold);
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