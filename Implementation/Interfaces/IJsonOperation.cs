namespace TakeHome.Implementation.Interfaces
{
    /// <summary>
    /// Interface for json operations like serialize and deserialze objects
    /// </summary>
    /// <typeparam name="T"> generic type to convert/ deserialize to </typeparam>
	public interface IJsonOperation<T>
	{
        public T DeSerializeObject(string item);
        public string SerializeObject(T item);
    }
}

