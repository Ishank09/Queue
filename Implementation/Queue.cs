
namespace TakeHome.Implementation
{
    using System.IO;
    using Base;
    using Interfaces;
    using Services;
    using Constants;


    /// <summary>
    /// Generic Queue Class, implementing Enqueue(T), Dequeue(), Peek(), Count, OnDiskCount, InMemoryCount
    /// </summary>
    /// <typeparam name="T"> generic type </typeparam>
    public class Queue<T> : BaseQueue<T>
    {
        private int _maxInMemorySize;
        private int _count;
        private int _inMemoryCount;
        private int _onDiskCount;
        private string _filePath;
        private IFileOperation _fileOperation;
        private IQueueOperation<T> _inMemoryQueue;
        private IQueueOperation<T> _diskBasedQueue;
        private ICommonQueueOperation _commonQueueOperation;
        private static readonly object _operationLock = new object();


        /// <summary>
        /// Queue class constructor to initialize in memory queue pointers and other variables.
        /// </summary>
        /// <param name="maxInMemory"> mximum size of the in memory queue </param>
        public Queue(int maxInMemory) : base(maxInMemory)
        {
            this._maxInMemorySize = maxInMemory;
            this._count = 0;
            this._inMemoryCount = 0;
            this._onDiskCount = 0;
            _commonQueueOperation = new CommonQueueOperationDifFile();
            this._filePath = _commonQueueOperation.CreateQueueID();

            _fileOperation = new FileOperation(this._filePath);
            _fileOperation.IsFile();
            _inMemoryQueue = new InMemoryQueue<T>();
            _commonQueueOperation.ValidateInMemorySize(maxInMemory);
            //IncrementInstanceCount();
            _diskBasedQueue = new DiskBasedDifFileQueue<T>(this._filePath, string.Empty);
        }


        /// <summary>
        /// Increment On Disk Count
        /// </summary>
        private void IncrementOnDiskCount()
        {
            this._onDiskCount++;
        }

        /// <summary>
        /// Increment In Memory Count
        /// </summary>
        private void IncrementInMemoryCount()
        {
            this._inMemoryCount++;
        }

        /// <summary>
        /// Decrement On Disk Count
        /// </summary>
        private void DecrementOnDiskCount()
        {
            this._onDiskCount--;
        }

        /// <summary>
        /// Decrement In Memory Count
        /// </summary>
        private void DecrementInMemoryCount()
        {
            this._inMemoryCount--;
        }

        /// <summary>
        /// Increment count
        /// </summary>
        private void IncrementCount()
        {
            this._count++;
        }

        /// <summary>
        /// Decrement count
        /// </summary>
        private void DecrementCount()
        {
            this._count--;
        }

        /// <summary>
        /// Destructor for the Queue class, so that.Net garbege collector deletes the files craeated when not in use.
        /// </summary>
        ~Queue()
        {
            if (File.Exists(this._filePath))
            {
                // Delete the file
                File.Delete(this._filePath);
            }
        }

        /// <summary>
        /// Inmemory Queue size
        /// </summary>
        public override int InMemoryCount => _inMemoryCount;

        /// <summary>
        /// On Disk Queue size
        /// </summary>
        public override int OnDiskCount => _onDiskCount;

        /// <summary>
        /// Total Queue size
        /// </summary>
        public override int Count => _count;

        /// <summary>
        /// Parent method to dequeue, decide on max in memory size to decide where to dequeue.
        /// </summary>
        /// <returns> element dequeued </returns>
        public override T Dequeue()
        {
            T deQueuedElement;
            _commonQueueOperation.CheckInvalidOperation(this._count);
            deQueuedElement = _inMemoryQueue.Dequeue(); 
            if (this._onDiskCount > 0)
            {
                T diskElement = _diskBasedQueue.Dequeue();
                _inMemoryQueue.Enqueue(diskElement);
                DecrementOnDiskCount();
            }
            else
            {
                DecrementInMemoryCount();
            }
            DecrementCount();
            return deQueuedElement;
        }

        /// <summary>
        /// Parent method to enqueue, decide on max in memory size to decide where to enqueue
        /// </summary>
        /// <param name="item">element to enqueue</param>
        public override void Enqueue(T item)
        {
            if (this._count >= this._maxInMemorySize)
            {
                lock (_operationLock)
                {
                    _diskBasedQueue.Enqueue(item);
                    IncrementOnDiskCount();
                }
            }
            else
            {
                _inMemoryQueue.Enqueue(item);
                IncrementInMemoryCount();
            }
            IncrementCount();
        }

        /// <summary>
        /// Peeking/ giving the first element in the queue (top element)
        /// </summary>
        /// <returns> Top/first element of queue </returns>
        public override T Peek()
        {
            _commonQueueOperation.CheckInvalidOperation(this._count);
            return _inMemoryQueue.Peek();
           
        }


    }
}