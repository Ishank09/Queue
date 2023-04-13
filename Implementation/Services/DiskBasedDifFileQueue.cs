using System;
using Newtonsoft.Json.Linq;
namespace TakeHome.Implementation.Services
{
    using Interfaces;

    /// <summary>
    /// Module for implementing basic Queue operations from IQueueOperation for disk based queue. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public class DiskBasedDifFileQueue<T> : IQueueOperation<T>
    {
        private IFileOperation _fileOperation;
        private IJsonOperation<JArray> _jarrayJsonOperation;
        private IJsonOperation<T> _genericJsonOperation;
        private string _id;
        private string _filePath;
        private int _top;

        /// <summary>
        /// Constructor to initialize the DiskBaseQueue Class
        /// </summary>
        /// <param name="filePath"> File path where the disk based queue will be stored </param>
        /// <param name="id"> id of queue (unique to a instance of queue) </param>
        public DiskBasedDifFileQueue(string filePath, string id)
        {
            this._filePath = filePath;
            _fileOperation = new FileOperation(this._filePath);
            _jarrayJsonOperation = new JsonOperation<JArray>();
            _genericJsonOperation = new JsonOperation<T>();
            _top = 0;
            _id = id;

        }
        
        /// <summary>
        /// Get Queue data from serialized string
        /// </summary>
        /// <param name="queueData"> sereliazed queuedata</param>
        /// <returns> JArray of Queue data/element </returns>
        private JArray GetDeSerializedQueueData(string queueData)
        {
            if (queueData == null || queueData.Equals(string.Empty))
            {
                return new JArray();
            }
            else
            {
                return _jarrayJsonOperation.DeSerializeObject(queueData);
            }
        }

        /// <summary>
        /// Get JToken of a queue element
        /// </summary>
        /// <param name="item"> Queue element </param>
        /// <returns> JToken of queue element </returns>
        private JToken GetSerializedElement(T item)
        {
            return JToken.FromObject(item);

        }

        /// <summary>
        /// Serialize JArray into string
        /// </summary>
        /// <param name="item"> JArray </param>
        /// <returns> serialized form of JArray </returns>
        private string GetSerializedJArray(JArray item)
        {
            return _jarrayJsonOperation.SerializeObject(item);

        }
        

        /// <summary>
        /// Deserialize string to queue element
        /// </summary>
        /// <param name="queueData"> string form of queue element </param>
        /// <returns> Queue element </returns>
        private T GetDeSerializedQueueElement(string queueData)
        {
            return _genericJsonOperation.DeSerializeObject(queueData);
        }

        /// <summary>
        /// Getting Queue data as JArray
        /// </summary>
        /// <returns> JArray of Queue Data </returns>
        private JArray GetJsonArrayQueueData()
        {
            string allQueueInstanceData = _fileOperation.ReadFile();
            JArray deSerializedQueueData = GetDeSerializedQueueData(allQueueInstanceData);
            return deSerializedQueueData;
        }

        /// <summary>
        /// Queue Dequeue method for disk/file. Remove (last inserted) element.
        /// </summary>
        /// <returns> Queue Element dequeued </returns>
        public T Dequeue()
        {
            try
            {
                JArray deSerializedQueueData = GetJsonArrayQueueData();
                JToken diskQueueHead = deSerializedQueueData[_top];
                if (diskQueueHead == null)
                {
                    throw new InvalidOperationException("Disk Queue empty");
                }
                //deSerializedQueueData.RemoveAt(0);

                T newObj;
                if (diskQueueHead.Type == JTokenType.Object)
                    newObj = GetDeSerializedQueueElement(diskQueueHead.ToString());
                else
                    newObj = diskQueueHead.Value<T>();
                _top += 1;

                //string eee = GetSerializedJArray(deSerializedQueueData);
                //hs = UpdateHashMap(hs, eee);
                //_fileOperation.WriteFile(_HashMapJsonOperation.SerializeObject(hs));

                return newObj;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur while writing to the file
                Console.WriteLine($"Error in file operations: {ex.Message}");
            }
            return default(T);
        }

        /// <summary>
        /// Queue Enqueue menthod for disk/file. It used json file for storing the Queue data.
        /// Each Queue instantiation will have differnt KeyValue in HashMap with unique id.
        /// </summary>
        /// <param name="item"> Queue element to enqueue</param>
        public void Enqueue(T item)
        {
            try
            {
                JArray deSerializedQueueData = GetJsonArrayQueueData();
                JToken queueElementToken = GetSerializedElement(item);
                deSerializedQueueData.Add(queueElementToken);
                string fileData = GetSerializedJArray(deSerializedQueueData);
                _fileOperation.WriteFile(fileData);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur while writing to the file
                Console.WriteLine($"Error in file operations: {ex.Message}");
            }
        }

        /// <summary>
        /// Peek top most element
        /// </summary>
        /// <returns> top most element of the queue </returns>
        public T Peek()
        {
            throw new NotImplementedException();
        }
    }
}

