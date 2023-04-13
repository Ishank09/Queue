using System;
using System.IO;
using System.Security;

namespace TakeHome.Implementation.Services
{
    using Interfaces;

    /// <summary>
    /// File operation module
    /// </summary>
	public class FileOperation: IFileOperation
	{
        private static readonly object _fileLock = new object();
        public string _filePath;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filePath">path of file</param>
        public FileOperation(string filePath)
        {
            this._filePath = filePath;
        }

        /// <summary>
        /// Checking if a given file is present or not for a queue object, if not, then create a file.
        /// </summary>
        /// <exception cref="IOException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="SecurityException"></exception>
        /// <exception cref="Exception"></exception>
        public void IsFile()
        {
            try
            {
                if (!File.Exists(this._filePath))
                {
                    using (FileStream fs = new FileStream(this._filePath, FileMode.CreateNew)) { }

                }
            }
            catch (IOException ex)
            {
                // Handle the exception
                throw new IOException(($"I/O exception, more information: {ex.Message} {AppDomain.CurrentDomain.BaseDirectory}"));
            }
            catch (UnauthorizedAccessException ex)
            {
                // Handle the exception
                throw new UnauthorizedAccessException($"Unauthorized access error, more information: {ex.Message}");
            }
            catch (SecurityException ex)
            {
                // Handle the exception
                throw new SecurityException($"Security error, more information: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other unhandled exceptions
                throw new Exception($"Unhandled/Unknown exception occured, more information: {ex.Message}");
            }
        }

        /// <summary>
        /// Read the queue storing file
        /// </summary>
        /// <returns> All file content as string </returns>
        public string ReadFile()
        {
            lock (_fileLock)
            {
                string fileData;
                using (var streamReader = new StreamReader(this._filePath))
                {
                    fileData = streamReader.ReadToEnd();
                }
                return fileData;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void WriteFile(string data)
        {
            lock (_fileLock)
            {
                using (var streamWriter = new StreamWriter(this._filePath))
                {
                    streamWriter.Write(data);
                }
            }

        }
      
    }
}

