﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KinectDataHandler.SpecialStructures
{
    internal class FloodQueue<T> : IEnumerable<T>, ICloneable
    {
        private readonly int _capacity;
        private readonly List<T> _hiddenQueue;

        public delegate void FloodQueueCapacityReachedDelegate(FloodQueue<T> queue);

        public event FloodQueueCapacityReachedDelegate FloodQueueCapacityReached;

        public FloodQueue(int capacity)
        {
            _capacity = capacity;
            _hiddenQueue = new List<T>(capacity);
        }

        public void Add(T element)
        {
            _hiddenQueue.Insert(0, element);
            if (_hiddenQueue.Count <= _capacity) return;
            OnFloodQueueCapacityReached(this);
            RemoveLast();
        }

        public void AddAll(ICollection<T> elements)
        {
            foreach (var element in elements)
            {
                Add(element);
            }
        }

        public void RemoveLast()
        {
            if(_hiddenQueue.Count > 0)
                _hiddenQueue.Remove(_hiddenQueue.Last());
        }

        public void RemoveFirst()
        {
            if (_hiddenQueue.Count > 0)
                _hiddenQueue.Remove(_hiddenQueue.First());
        }

        public void Clear()
        {
            _hiddenQueue.Clear();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _hiddenQueue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _hiddenQueue.GetEnumerator();
        }

        public int Count => _hiddenQueue.Count;

        protected virtual void OnFloodQueueCapacityReached(FloodQueue<T> queue)
        {
            FloodQueueCapacityReached?.Invoke(queue);
        }

        public object Clone()
        {
            var nfq = new FloodQueue<T>(_capacity);
            nfq.AddAll(_hiddenQueue);
            return nfq;
        }
    }
}
