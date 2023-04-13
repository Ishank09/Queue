using TakeHome.Implementation.Models;

namespace TakeHome.Implementation.Services
{
    using Interfaces;

    /// <summary>
    /// Module to implement in memory queue
    /// </summary>
    /// <typeparam name="T"> type of queue lement </typeparam>
	public class InMemoryQueue<T>: IQueueOperation<T>
	{
        private NodeWrapper<T> head;
        private NodeWrapper<T> tail;

        /// <summary>
        /// Constructor for inmemory queue
        /// </summary>
        public InMemoryQueue()
		{
            this.head = null;
            this.tail = null;
        }

        /// <summary>
        /// Queue Dequeue method for disk/file. Remove first/ last inserted element.
        /// </summary>
        /// <returns>Queue element to be dequeued</returns>
        public T Dequeue()
        {
            T deQueueElement;
            deQueueElement = head.data;
            head = head.next;
            return deQueueElement;
        }

        /// <summary>
        /// Queue Enqueue menthod for in memory. It uses NodeWrapper Class to store and point to next node.
        /// element by sequence.
        /// </summary>
        /// <param name="item"> Queue Element to enqueue </param>
        public void Enqueue(T item)
        {
            NodeWrapper<T> currentNode = new NodeWrapper<T>(item);
            if (this.head is null)
            {
                this.head = currentNode;
                this.tail = currentNode;
            }
            else
            {
                this.tail.next = currentNode;
                tail = currentNode;
            }
        }

        /// <summary>
        /// Returning top element
        /// </summary>
        /// <returns> top element of queue </returns>
        public T Peek()
        {
            return head.data;
        }
    }
}

