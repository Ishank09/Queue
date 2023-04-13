namespace TakeHome.Implementation.Interfaces
{
    /// <summary>
    /// Interface for having operations on file
    /// </summary>
	public interface IFileOperation
	{
        public void IsFile();
        public string ReadFile();
        public void WriteFile(string data);
    }
}

