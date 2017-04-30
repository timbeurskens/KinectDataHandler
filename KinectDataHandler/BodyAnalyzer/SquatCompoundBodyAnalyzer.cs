using System;
using System.Collections.ObjectModel;
using KinectDataHandler.Linear3DTools;
using Microsoft.Kinect;

namespace KinectDataHandler.BodyAnalyzer
{
    //TODO: Pass floorplane to constant analyzer
    public class SquatCompoundBodyAnalyzer : CompoundBodyAnalyzer
    {
        public SquatCompoundBodyAnalyzer(Body b) : base(b)
        {
            
        }

        public SquatCompoundBodyAnalyzer(ulong trackingId) : base(trackingId)
        {
            
        }

        public override Collection<ConstantBodyAnalyzer> GetConstantAnalyzers()
        {

            return new Collection<ConstantBodyAnalyzer>
            {
                new FootKneeConstantBodyAnalyzer(TrackingId)
                {
                    Treshold = Math.PI / 7
                }
            };
        }

        public override ProgressiveBodyAnalyzer GetProgressiveAnalyzer()
        {
            return new KneeAngleProgressiveBodyAnalyzer(TrackingId, Math.PI * 0.9, 10, Math.PI / 2);
        }
    }
}