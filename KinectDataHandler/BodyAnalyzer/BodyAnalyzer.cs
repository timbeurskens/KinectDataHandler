using Microsoft.Kinect;

namespace KinectDataHandler.BodyAnalyzer
{
    public abstract class BodyAnalyzer<T>
    {
        private bool _singleEvent = true;
        private bool _onValueComputedEventFired;

        public bool Active { get; private set; }
        protected bool SoftResetActive;
        protected internal ulong TrackingId;


        public delegate void ValueComputedDelegate(T value);

        public event ValueComputedDelegate ValueComputed;

        protected BodyAnalyzer(Body b) : this(b.TrackingId)
        {
        }

        protected BodyAnalyzer(ulong trackingId)
        {
            TrackingId = trackingId;
        }

        public virtual void SetTrackingId(ulong trackingId)
        {
            TrackingId = trackingId;
        }

        protected BodyAnalyzer(Body b, bool singleEvent) : this(b.TrackingId, singleEvent)
        {
        }

        protected BodyAnalyzer(ulong trackingId, bool singleEvent) : this(trackingId)
        {
            _singleEvent = singleEvent;
        }

        public void SetSingleEvent(bool v) => _singleEvent = v;

        public virtual bool PassBody(Body b)
        {
            if (!Active) return false;
            
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

        protected virtual void ResetSoftResetState()
        {
            SoftResetActive = false;
        }

        public void Reset()
        {
            ResetSoftResetState();
            ResetOnValueComputedEvent();
            DoReset();
        }

        public void Enable()
        {
            Active = true;
        }

        public void Disable()
        {
            Active = false;
        }

        public virtual void SoftReset()
        {
            SoftResetActive = true;
        }
    }
}