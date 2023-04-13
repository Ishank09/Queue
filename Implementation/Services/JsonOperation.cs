using Newtonsoft.Json;

namespace TakeHome.Implementation.Services
{
    using Interfaces;

    /// <summary>
    /// Class for Json operations like serialize and deserialize
    /// </summary>
    /// <typeparam name="T"> generic type to deserialize to </typeparam>
	public class JsonOperation<T> : IJsonOperation<T>
	{

        /// <summary>
        /// Deserializatin code
        /// </summary>
        /// <param name="element"> Queue Element </param>
        /// <returns> T1 </returns>
        public T DeSerializeObject(string element)
        {
            T ds = (T)JsonConvert.DeserializeObject(element, typeof(T));
            return ds;
        }

        /// <summary>
        /// Serialize the Queue element. First store as a Model, and then serialize.
        /// </summary>
        /// <param name="element"> element to serialize </param>
        /// <returns> string. </returns>
        public string SerializeObject(T element)
        {
            return JsonConvert.SerializeObject(element);
        }
    }
}

