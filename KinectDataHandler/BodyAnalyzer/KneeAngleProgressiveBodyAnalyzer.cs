using System;
using KinectDataHandler.Linear3DTools;
using Microsoft.Kinect;

namespace KinectDataHandler.BodyAnalyzer
{
    public class KneeAngleProgressiveBodyAnalyzer : ProgressiveBodyAnalyzer
    {
        private double _startTreshold, _endTreshold, _stepSize;
        private int _numSteps, _position;
        private readonly JointTriple _kneeLeft = new JointTriple(JointType.AnkleLeft, JointType.KneeLeft, JointType.HipLeft);
        private readonly JointTriple _kneeRight = new JointTriple(JointType.AnkleRight, JointType.KneeRight, JointType.HipRight);

        public KneeAngleProgressiveBodyAnalyzer(Body b, double start, int steps, double end) : base(b)
        {
            _startTreshold = start;
            _numSteps = steps;
            _endTreshold = end;
            _stepSize = Math.Abs(_endTreshold - _startTreshold) / _numSteps;
            _position = 0;
        }

        public KneeAngleProgressiveBodyAnalyzer(ulong trackingId, double start, int steps, double end) : base(trackingId)
        {
            _startTreshold = start;
            _numSteps = steps;
            _endTreshold = end;
            _stepSize = Math.Abs(_endTreshold - _startTreshold) / _numSteps;
            _position = 0;
        }

        private double Map(double p)
        {
            return (p - _startTreshold) / (_endTreshold - _startTreshold);
        }

        protected override bool HandleBody(Body b)
        {
            //for now, take average of left, right
            try
            {
                var leftAngle = _kneeLeft.GetAngle(b);
                var rightAngle = _kneeRight.GetAngle(b);
                var avgAngle = (leftAngle + rightAngle) / 2;
                var p = Map(avgAngle);
                var step = (int) Math.Floor(p * _numSteps);
                UpdateAnalyzerState(step);
                Console.WriteLine(_position);
            }
            catch (NotTrackedException)
            {
                return false;
            }
            
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <modifies>_position, State</modifies>
        /// <param name="step"></param>
        private void UpdateAnalyzerState(int step)
        {
            switch (State)
            {
                case ProgressiveBodyAnalyzerState.Success:
                case ProgressiveBodyAnalyzerState.Failed:
                    return;
                case ProgressiveBodyAnalyzerState.Idle:
                    if (step >= _numSteps)
                    {
                        State = ProgressiveBodyAnalyzerState.Halfway;
                        _position = _numSteps;
                        OnValueComputed(State);
                    }
                    else if(step > 0)
                    {
                        if (step > _position)
                        {
                            _position = step;
                        }
                        else if (step < _position - 1)
                        {
                            _position = 0;
                            State = ProgressiveBodyAnalyzerState.Failed;
                            OnValueComputed(State);
                        }
                    }
                    break;
                case ProgressiveBodyAnalyzerState.Halfway:
                    if (step <= 0)
                    {
                        State = ProgressiveBodyAnalyzerState.Success;
                        _position = 0;
                        OnValueComputed(State);
                    }
                    else if(step < _numSteps)
                    {
                        if (step < _position)
                        {
                            _position = step;
                        }
                        else if (step > _position + 1)
                        {
                            _position = 0;
                            State = ProgressiveBodyAnalyzerState.Failed;
                            OnValueComputed(State);
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void DoReset()
        {
            base.DoReset();
            _position = 0;
        }
    }
}
