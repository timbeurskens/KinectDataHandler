﻿using System;
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
        public ProgressiveBodyAnalyzerState State = ProgressiveBodyAnalyzerState.Idle;
        protected double StartTreshold, EndTreshold;
        protected int NumSteps, Position, CurrentStep;

        protected ProgressiveBodyAnalyzer(Body b, double start, int steps, double end) : base(b, false)
        {
            StartTreshold = start;
            NumSteps = steps;
            EndTreshold = end;
            Position = 0;
        }

        protected ProgressiveBodyAnalyzer(ulong trackingId, double start, int steps, double end) : base(trackingId, false)
        {
            StartTreshold = start;
            NumSteps = steps;
            EndTreshold = end;
            Position = 0;
        }

        public override ProgressiveBodyAnalyzerState GetValue()
        {
            return State;
        }

        protected override void DoReset()
        {
            State = ProgressiveBodyAnalyzerState.Idle;
            Position = 0;
            CurrentStep = 0;
        }

        private void UpdateAnalyzerState()
        {
            Console.WriteLine("update");
            switch (State)
            {
                case ProgressiveBodyAnalyzerState.Success:
                case ProgressiveBodyAnalyzerState.Failed:
                    return;
                case ProgressiveBodyAnalyzerState.Started:
                    if (CurrentStep >= NumSteps)
                    {
                        State = ProgressiveBodyAnalyzerState.Halfway;
                        Position = NumSteps;
                        OnValueComputed(State);
                    }
                    else if (CurrentStep > 0)
                    {
                        if (CurrentStep > Position)
                        {
                            Position = CurrentStep;
                        }
                        else if (CurrentStep < Position - 1)
                        {
                            Position = 0;
                            State = ProgressiveBodyAnalyzerState.Failed;
                            OnValueComputed(State);
                        }
                    }
                    break;
                case ProgressiveBodyAnalyzerState.Idle:
                    if (CurrentStep > 0)
                    {
                        State = ProgressiveBodyAnalyzerState.Started;
                        OnValueComputed(State);
                    }
                    break;
                case ProgressiveBodyAnalyzerState.Halfway:
                    if (CurrentStep <= 0)
                    {
                        State = ProgressiveBodyAnalyzerState.Success;
                        Position = 0;
                        OnValueComputed(State);
                    }
                    else if (CurrentStep < NumSteps)
                    {
                        if (CurrentStep < Position)
                        {
                            Position = CurrentStep;
                        }
                        else if (CurrentStep > Position + 1)
                        {
                            Position = 0;
                            State = ProgressiveBodyAnalyzerState.Failed;
                            OnValueComputed(State);
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override bool PassBody(Body b)
        {
            var result = base.PassBody(b);
            UpdateAnalyzerState();
            return result;
        }

        protected double Map(double p)
        {
            return (p - StartTreshold) / (EndTreshold - StartTreshold);
        }

        public double GetProgress()
        {
            switch (State)
            {
                case ProgressiveBodyAnalyzerState.Failed:
                    return 0;
                case ProgressiveBodyAnalyzerState.Success:
                    return 1;
                case ProgressiveBodyAnalyzerState.Halfway:
                    return 0.5 + (1.0 - Position / (double)NumSteps) / 2.0;
                case ProgressiveBodyAnalyzerState.Idle:
                    return 0;
                case ProgressiveBodyAnalyzerState.Started:
                    return Position / (double)NumSteps / 2.0;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
