using System;
using Microsoft.Kinect;

namespace KinectDataHandler.BodyAnalyzer
{
    /// <summary>
    /// ConstantBodyAnalyzer
    /// Whether a given property is constant (bounded)
    /// Iff the check fails, the OnValueComputed event is invoked.
    /// </summary>
    public abstract class ConstantBodyAnalyzer : BodyAnalyzer<bool>
    {
        private bool _isValid = true;

        protected ConstantBodyAnalyzer(Body b) : base(b)
        {
        }

        protected ConstantBodyAnalyzer(ulong trackingId) : base(trackingId)
        {
        }

        public override bool HandleBody(Body b)
        {
            var nw = CheckBody(b);

            if (SoftResetActive && nw)
            {
                Reset();
            }

            if (_isValid && !nw)
            {
                OnValueComputed(false);
            }

            _isValid = _isValid && CheckBody(b);
            return true;
        }

        public override bool GetValue()
        {
            return _isValid;
        }

        protected override void DoReset()
        {
            _isValid = true;
        }

        protected abstract bool CheckBody(Body b);
    }
}