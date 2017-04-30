using System;
using Microsoft.Kinect;

namespace KinectDataHandler.BodyAnalyzer
{
    public abstract class BodyAnalyzer<T>
    {
        private bool _singleEvent = true;
        private bool _onValueComputedEventFired;
        protected internal ulong TrackingId;
        

        public delegate void ValueComputedDelegate(T value);

        public event ValueComputedDelegate ValueComputed;

        protected BodyAnalyzer(Body b)
        {
            TrackingId = b.TrackingId;
        }

        protected BodyAnalyzer(ulong trackingId)
        {
            TrackingId = trackingId;
        }

        protected BodyAnalyzer(Body b, bool singleEvent) : this(b)
        {
            _singleEvent = singleEvent;
        }

        protected BodyAnalyzer(ulong trackingId, bool singleEvent) : this(trackingId)
        {
            _singleEvent = singleEvent;
        }

        public void SetSingleEvent(bool v) => _singleEvent = v;

        public bool PassBody(Body b)
        {
            if (b.TrackingId != TrackingId) return false;
            return b.IsTracked && HandleBody(b);
        }

        public abstract bool HandleBody(Body b);
        public abstract T GetValue();
        protected abstract void DoReset();

        protected virtual void OnValueComputed(T value)
        {
            if (_onValueComputedEventFired) return;
            ValueComputed?.Invoke(value);
            _onValueComputedEventFired = _singleEvent;
        }

        protected virtual void ForceOnValueComputed(T value)
        {
            ValueComputed?.Invoke(value);
        }

        protected virtual void ResetOnValueComputedEvent()
        {
            _onValueComputedEventFired = false;
        }

        public void Reset()
        {
            ResetOnValueComputedEvent();
            DoReset();
        }
        
    }
}
