using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace KinectDataHandler.BodyAnalyzer
{
    public abstract class ConstantBodyAnalyzer : BodyAnalyzer<bool>
    {
        private bool _isValid = true;

        protected ConstantBodyAnalyzer(Body b) : base(b)
        {

        }

        protected ConstantBodyAnalyzer(ulong trackingId) : base(trackingId)
        {

        }

        protected override bool HandleBody(Body b)
        {
            var nw = CheckBody(b);

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
