using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Kinect;

namespace KinectDataHandler.BodyAnalyzer
{
    public abstract class CompoundBodyAnalyzer : BodyAnalyzer<ProgressiveBodyAnalyzerState>
    {
        protected ProgressiveBodyAnalyzerState State = ProgressiveBodyAnalyzerState.Idle;
        protected Collection<ConstantBodyAnalyzer> ConstantAnalyzers;
        protected ProgressiveBodyAnalyzer ProgressiveBodyAnalyzer;

        protected CompoundBodyAnalyzer(Body b) : base(b, false)
        {
            
        }

        protected CompoundBodyAnalyzer(ulong trackingId) : base(trackingId, false)
        {
            
        }
        

        public abstract Collection<ConstantBodyAnalyzer> GetConstantAnalyzers();
        public abstract ProgressiveBodyAnalyzer GetProgressiveAnalyzer();

        protected void UpdateCompoundState()
        {
            if (State == ProgressiveBodyAnalyzerState.Failed || State == ProgressiveBodyAnalyzerState.Success) return;

            if (!GetConstant())
            {
                State = ProgressiveBodyAnalyzerState.Failed;
                OnValueComputed(State);
            }
            else
            {
                var s = ProgressiveBodyAnalyzer.GetValue();
                if (s == State) return;
                State = s;
                OnValueComputed(State);
            }
        }

        public override bool HandleBody(Body b)
        {
            if (ConstantAnalyzers == null)
            {
                ConstantAnalyzers = GetConstantAnalyzers();
                
            }
            if (ProgressiveBodyAnalyzer == null)
            {
                
                ProgressiveBodyAnalyzer = GetProgressiveAnalyzer();
            }
            var result = ConstantAnalyzers.Select(analyzer => analyzer.PassBody(b)).Aggregate(true, (current, r) => current && r);
            var pr = ProgressiveBodyAnalyzer.PassBody(b);
            result = result && pr;
            UpdateCompoundState();
            return result;
        }

        protected bool GetConstant()
        {
            return ConstantAnalyzers.Aggregate(true, (current, c) => current && c.GetValue());
        }

        public override ProgressiveBodyAnalyzerState GetValue()
        {
            return State;
        }

        protected override void DoReset()
        {
            foreach (var analyzer in ConstantAnalyzers)
            {
                analyzer.Reset();
            }
            ProgressiveBodyAnalyzer.Reset();
            State = ProgressiveBodyAnalyzerState.Idle;
        }
    }
}
