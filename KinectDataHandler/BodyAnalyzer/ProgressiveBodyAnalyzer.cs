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
        protected ProgressiveBodyAnalyzerState State = ProgressiveBodyAnalyzerState.Idle;

        protected ProgressiveBodyAnalyzer(Body b) : base(b, false)
        {
        }

        protected ProgressiveBodyAnalyzer(ulong trackingId) : base(trackingId, false)
        {
        }

        public override ProgressiveBodyAnalyzerState GetValue()
        {
            return State;
        }

        protected override void DoReset()
        {
            State = ProgressiveBodyAnalyzerState.Idle;
        }
    }
}
