using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Kinect;

namespace KinectDataHandler.BodyAnalyzer
{
    /// <summary>
    /// 
    /// </summary>
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
            if (SoftResetActive && ProgressiveBodyAnalyzer.GetValue() == ProgressiveBodyAnalyzerState.Idle)
            {
                Reset();
            }

            if (State == ProgressiveBodyAnalyzerState.Failed || State == ProgressiveBodyAnalyzerState.Success) return;
            
            if (!GetConstant() && ProgressiveBodyAnalyzer.GetValue() != ProgressiveBodyAnalyzerState.Idle)
            {
                UpdateState(ProgressiveBodyAnalyzerState.Failed);
            }
            else
            {
                var s = ProgressiveBodyAnalyzer.GetValue();
                if (s == State) return;
                UpdateState(s);
            }
        }

        public override bool HandleBody(Body b)
        {
            if (ConstantAnalyzers == null)
            {
                ConstantAnalyzers = GetConstantAnalyzers();
                foreach (var a in ConstantAnalyzers)
                {
                    a.Enable();
                }
            }

            if (ProgressiveBodyAnalyzer == null)
            {
                ProgressiveBodyAnalyzer = GetProgressiveAnalyzer();
                ProgressiveBodyAnalyzer.Enable();
            }

            var result = ConstantAnalyzers.Select(analyzer => analyzer.PassBody(b))
                .Aggregate(true, (current, r) => current && r);
            var pr = ProgressiveBodyAnalyzer.PassBody(b);
            result = result && pr;

            UpdateCompoundState();

            return result;
        }

        private void UpdateState(ProgressiveBodyAnalyzerState s)
        {
            State = s;
            OnValueComputed(State);
        }

        protected bool GetConstant()
        {
            return ConstantAnalyzers.Aggregate(true, (current, c) => current && c.GetValue());
        }

        public override ProgressiveBodyAnalyzerState GetValue()
        {
            return State;
        }

        public override void SoftReset()
        {
            base.SoftReset();
            foreach(var analyzer in ConstantAnalyzers)
            {
                analyzer.SoftReset();
            }
            ProgressiveBodyAnalyzer.SoftReset();
        }

        public override void SetTrackingId(ulong trackingId)
        {
            base.SetTrackingId(trackingId);
            
            ProgressiveBodyAnalyzer?.SetTrackingId(trackingId);

            if (ConstantAnalyzers == null) return;

            foreach (var analyzer in ConstantAnalyzers)
            {
                analyzer.SetTrackingId(trackingId);
            }
        }

        protected override void DoReset()
        {
            foreach (var analyzer in ConstantAnalyzers)
            {
                analyzer?.Reset();
            }
            ProgressiveBodyAnalyzer?.Reset();
            UpdateState(ProgressiveBodyAnalyzerState.Idle);
        }
    }
}