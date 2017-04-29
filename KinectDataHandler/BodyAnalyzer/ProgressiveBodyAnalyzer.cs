using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace KinectDataHandler.BodyAnalyzer
{
    /// <summary>
    /// ProgressiveBodyAnalyzer
    /// A given property must progress to a given value.
    /// No fallbacks above a given treshold are allowed.
    /// OnValueComputedEvent returns state changes
    /// </summary>
    public abstract class ProgressiveBodyAnalyzer : BodyAnalyzer<ProgressiveBodyAnalyzerState>
    {
        private ProgressiveBodyAnalyzerState _state = ProgressiveBodyAnalyzerState.Idle;

        protected ProgressiveBodyAnalyzer(Body b) : base(b, false)
        {
        }

        protected ProgressiveBodyAnalyzer(ulong trackingId) : base(trackingId, false)
        {
        }

        public override ProgressiveBodyAnalyzerState GetValue()
        {
            return _state;
        }

        protected override void DoReset()
        {
            _state = ProgressiveBodyAnalyzerState.Idle;
        }
    }
}
