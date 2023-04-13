using System;
using Newtonsoft.Json.Linq;
using TakeHome.Implementation.Models;

namespace TakeHome.Implementation.Services
{
    using Interfaces;

    /// <summary>
    /// Module for implementing basic Queue operations from IQueueOperation for disk based queue. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public class DiskBasedQueue<T>: IQueueOperation<T>
	{
        private IFileOperation _fileOperation;
        private IJsonOperation<HashMap<string, string>> _HashMapJsonOperation;
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
        public DiskBasedQueue(string filePath, string id )
		{
            this._filePath = filePath;
            _fileOperation = new FileOperation(this._filePath);
            _HashMapJsonOperation = new JsonOperation<HashMap<string, string>>();
            _jarrayJsonOperation = new JsonOperation<JArray>();
            _genericJsonOperation = new JsonOperation<T>();
            _top = 0;
            _id = id;

        }

        /// <summary>
        /// Serializing string to HashMap
        /// </summary>
        /// <param name="serializedString">File content</param>
        /// <returns> HashMap of instance </returns>
        private HashMap<string, string> GetHashMap(string serializedString)
        {
            HashMap<string, string> hs = _HashMapJsonOperation.DeSerializeObject(serializedString);
            if (hs == null) return new HashMap<string, string>();
            else return hs;
        }

        /// <summary>
        /// Get queue instance from HashMap by queue id
        /// </summary>
        /// <param name="queueInstances"> HashMap containing data of all queue instances </param>
        /// <returns> Sereialized verion od queue data </returns>
        private string GetQueueInstance(HashMap<string, string> queueInstances)
        {
            if (queueInstances == null)
            {
                return string.Empty;
            }
            string queueData = queueInstances.Get(this._id);

            return (queueData == null) ? string.Empty : queueData;
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
        /// Updating HashMap - adding the new instance of queue in the hasjtableß
        /// </summary>
        /// <param name="hs"> HashMap </param>
        /// <param name="ele"> New Queue instance </param>
        /// <returns> updated HashMap </returns>
        private HashMap<string, string> UpdateHashMap(HashMap<string, string> hs, string ele)
        {
            hs.Add(this._id, ele);
            return hs;
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
        private KeyValuePair<JArray,HashMap<string,string>> GetJsonArrayQueueData()
        {
            string allQueueInstanceData = _fileOperation.ReadFile();
            HashMap<string, string> hs = GetHashMap(allQueueInstanceData);
            string queueData = GetQueueInstance(hs);
            JArray deSerializedQueueData = GetDeSerializedQueueData(queueData);
            KeyValuePair < JArray,HashMap<string, string> >  response = new KeyValuePair<JArray, HashMap<string, string>>();
            response.Key = deSerializedQueueData;
            response.Value = hs;
            return response;
        }

        /// <summary>
        /// Queue Dequeue method for disk/file. Remove (last inserted) element.
        /// </summary>
        /// <returns> Queue Element dequeued </returns>
        public T Dequeue()
        {
            try
            {
                KeyValuePair<JArray, HashMap<string, string>> response = GetJsonArrayQueueData();
                JArray deSerializedQueueData = response.Key;
                HashMap<string, string> hs = response.Value;
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
                KeyValuePair<JArray, HashMap<string, string>> response = GetJsonArrayQueueData();
                JArray deSerializedQueueData = response.Key;
                HashMap<string, string> hs = response.Value;
                JToken queueElementToken = GetSerializedElement(item);
                deSerializedQueueData.Add(queueElementToken);
                string eee = GetSerializedJArray(deSerializedQueueData);
                hs = UpdateHashMap(hs, eee);
                string dsde = _HashMapJsonOperation.SerializeObject(hs);
                _fileOperation.WriteFile(dsde);
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
        /// <exception cref="InvalidOperationException"> throw exception when queue is empty </exception>
        public T Peek()
        {
            KeyValuePair<JArray, HashMap<string, string>> response = GetJsonArrayQueueData();
            JArray deSerializedQueueData = response.Key;
            JToken firstItem = deSerializedQueueData.First;
            if (firstItem == null)
            {
                throw new InvalidOperationException("Disk Queue empty");
            }

            T newObj;
            if (firstItem.Type == JTokenType.Object)
                newObj = GetDeSerializedQueueElement(firstItem.ToString());
            else
                newObj = firstItem.Value<T>();
            return newObj;
        }
    }
}

