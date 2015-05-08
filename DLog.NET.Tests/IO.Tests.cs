using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DLog.NET.Tests
{
    [TestClass]
    public class IOTests
    {
        [TestMethod]
        public void When_Provided_LogFile_Structure_Does_Not_Exist()
        {
            //ARRANGE
            string path = @"C:\Logs\Test\TestFile.txt";

            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.DirectoryName != null && Directory.Exists(fileInfo.DirectoryName)) 
                Directory.Delete(fileInfo.DirectoryName,true);

            DLogger logger = new DLogger();

            //ACT
            logger.AddTargetFile(path);
            logger.AddTargetFile(fileInfo);
            File.Delete(path);

            //ASSERT
            Assert.IsTrue(logger.TargetFiles.Count > 0);
        }

        [TestMethod]
        public void When_Two_Same_Files_Are_Provided_Then_No_Duplicates()
        {
            //ARRANGE
            string path = @"C:\Logs\Test\TestFile.txt";

            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.DirectoryName != null && Directory.Exists(fileInfo.DirectoryName))
                Directory.Delete(fileInfo.DirectoryName, true);

            DLogger logger = new DLogger();

            //ACT
            logger.AddTargetFile(path);
            logger.AddTargetFile(fileInfo);
            File.Delete(path);

            //ASSERT
            Assert.IsTrue(logger.TargetFiles.Count == 1);

        }

        [TestMethod]
        public void When_File_Was_Deleted_Try_Writing_Log()
        {
            //ARRANGE
            string path = @"C:\Logs\Test\TestFile.txt";

            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.DirectoryName != null && Directory.Exists(fileInfo.DirectoryName))
                Directory.Delete(fileInfo.DirectoryName, true);

            DLogger logger = new DLogger();

            //ACT
            logger.AddTargetFile(path);
            logger.AddTargetFile(fileInfo);

            File.Delete(path);
            logger.Write("Test");

            //ASSERT
            Assert.IsTrue(File.ReadAllText(path).Contains("Test"));
            File.Delete(path);
        }

    }
}
