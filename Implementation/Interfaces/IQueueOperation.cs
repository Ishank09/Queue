namespace TakeHome.Implementation.Interfaces
{
    /// <summary>
    /// Interface for Queue operation - common
    /// </summary>
    /// <typeparam name="T"> generic queue type </typeparam>
	public interface IQueueOperation<T>
	{
        public void Enqueue(T item);
        public T Dequeue();
        public T Peek();
    }
}

