using System;

namespace TakeHome.Implementation.Services
{
    using Interfaces;


    /// <summary>
    /// Module to have common Queue operarions (common  to in memory and on disk queue)
    /// </summary>
	public class CommonQueueOperation: ICommonQueueOperation
    {
        int _instanceCount;
        int _size;
        private IFileOperation _fileOperation;
        private IJsonOperation<HashMap<string, string>> _HashMapJsonOperation;

        /// <summary>
        /// CommonQueueOperation Constructor to initialize the variables
        /// </summary>
        /// <param name="instanceCount"> number of total queue instances </param>
        /// <param name="size"> Maximum allowable instances of Queue can be created </param>
        /// <param name="fileOperation"> Instance for File operation </param>
        /// <param name="HashMapJsonOperation"> Instance for json operation </param>
        public CommonQueueOperation(int instanceCount, int size, IFileOperation fileOperation, IJsonOperation<HashMap<string, string>> HashMapJsonOperation)
		{
            this._instanceCount = instanceCount;
            this._size = size;
            this._fileOperation = fileOperation;
            this._HashMapJsonOperation = HashMapJsonOperation;

        }

        /// <summary>
        /// Construst unique id for each queue instance, on the bases of the number of instances        /// </summary>
        /// <returns> unique queue id for a instance </returns>
        public string CreateQueueID()
        {
            if (_instanceCount >= _size) _instanceCount = 0;
            string id = _instanceCount.ToString();
            string allQueueInstanceData = _fileOperation.ReadFile();
            HashMap<string, string> hs = GetHashMap(allQueueInstanceData);
            hs.RemoveIndex(_instanceCount);
            _fileOperation.WriteFile(_HashMapJsonOperation.SerializeObject(hs));
            return id;
        }

        /// <summary>
        /// Checking if the Dequeue operation is done on an empty Queue
        /// </summary>
        /// <param name="_count"> Size of Queue - number of elements in queue </param>
        /// <exception cref="InvalidOperationException"> Throws exception when count is 0 </exception>
        public void CheckInvalidOperation(int count)
        {
            if (count == 0)
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Serializing string to HashMap
        /// </summary>
        /// <param name="serializedString">File content</param>
        /// <returns> HashMap of instance </returns>
        public HashMap<string, string> GetHashMap(string serializedString)
        {
            HashMap<string, string> hs = _HashMapJsonOperation.DeSerializeObject(serializedString);
            if (hs == null) return new HashMap<string, string>();
            else return hs;
        }


        public void ValidateInMemorySize(int inMemory)
        {
            if (inMemory <= 0)
                throw new InvalidOperationException();
        }

    

        public string GetFilePath()
        {
            throw new NotImplementedException();
        }
    }
}

