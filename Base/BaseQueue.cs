// Copyright (c) Docugami, Inc. All rights reserved.

namespace TakeHome.Base
{
    public abstract class BaseQueue<T> : IQueue<T>
    {
        protected BaseQueue(int maxInMemory)
        {
            this.MaxInMemory = maxInMemory;
        }

        public abstract int Count { get; }

        public abstract int InMemoryCount { get; }

        public abstract int OnDiskCount { get; }

        public int MaxInMemory { get; }

        public abstract T Dequeue();

        public abstract void Enqueue(T item);

        public abstract T Peek();
    }
}
