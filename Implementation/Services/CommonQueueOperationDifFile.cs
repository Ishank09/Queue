using System;
using System.IO;
using TakeHome.Implementation.Interfaces;

namespace TakeHome.Implementation.Services
{
    using Constants;
	public class CommonQueueOperationDifFile : ICommonQueueOperation
    {
        string _filePath;
        string _directoryPath = QueueConstants._directoryPath;
        public CommonQueueOperationDifFile()
        {
            string id = Guid.NewGuid().ToString();
            string fileName = $"{DateTime.Now:yyyyHHmmss}_{id}.json";
            this._filePath = Path.Combine(_directoryPath, fileName);
            CheckDirectory();
        }
        private void CheckDirectory()
        {
            if (!Directory.Exists(_directoryPath))
            {
                Directory.CreateDirectory(_directoryPath);
            }
        }

        public void CheckInvalidOperation(int count)
        {
            if (count == 0)
            {
                throw new InvalidOperationException();
            }
        }

        public string CreateQueueID()
        {
            return _filePath;
        }

     

        public void ValidateInMemorySize(int inMemory)
        {
            if (inMemory <= 0)
                throw new InvalidOperationException();
        }

        public string GetFilePath()
        {
            return this._filePath;
        }
    }
}

