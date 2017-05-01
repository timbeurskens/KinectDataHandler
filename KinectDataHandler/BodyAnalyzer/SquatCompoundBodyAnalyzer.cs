﻿using System;
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