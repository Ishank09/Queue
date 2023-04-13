// Copyright (c) Docugami, Inc. All rights reserved.

namespace TakeHome.Base
{
    public interface IQueue<T>
    {
        /// <summary>
        /// The total number of items in the queue.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Adds an item to the queue.
        /// </summary>
        void Enqueue(T item);

        /// <summary>
        /// Removes an item from the queue and returns it.
        /// </summary>
        T Dequeue();

        /// <summary>
        /// Returns the first item in the queue.
        /// </summary>
        T Peek();
    }
}