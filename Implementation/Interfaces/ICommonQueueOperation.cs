namespace TakeHome.Implementation.Interfaces
{
    using Services;
    /// <summary>
    /// Interface for Common operations of queue (common to in memory and on dick queue)
    /// </summary>
	public interface ICommonQueueOperation
	{
        public string CreateQueueID();
        public void CheckInvalidOperation(int count);
        public void ValidateInMemorySize(int inMemory);
        public string GetFilePath();
    }
}

