using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using BizUnit;
using System.Xml;
using System.IO;
using System.Diagnostics;

namespace BizUnitCompareTests.FlatfileCompare
{
	[TestFixture]
	class FlatfileCompareTest
	{
		string testFilesPath = Directory.GetCurrentDirectory() + @"\FlatfileCompare\testFiles\";
		XmlDocument config = new XmlDocument();
		XmlNode configPart;
		Context context;

		[SetUp]
		[TearDown]
		public void SetUp()
		{
			CleanFileSystem();
			config.Load(@"FlatfileCompare\testConfig.xml");
			configPart = config.SelectSingleNode("/TestStep");

			context = new Context();
			context.Add("searchDirectory", testFilesPath);
			context.Add("deleteFile", "false");
			context.Add("goalFile", testFilesPath + "test.goal");
			context.Add("filter", "test.test");
		}

		[Test]
		public void ExecuteFoundFileNotDeleted()
		{
			context.Add("deleteFile", "false", true);
			ExecuteNoException();
			Assert.IsTrue(File.Exists(context.GetValue("searchDirectory") + context.GetValue("filter")));
		}

		[Test]
		public void ExecuteFoundFileDeleted()
		{
			context.Add("deleteFile", "true", true);
			ExecuteNoException();
			Assert.IsFalse(File.Exists(context.GetValue("searchDirectory") + context.GetValue("filter")));
		}

		[Test]
		public void ExecuteFileNotFoundException()
		{
			BizUnitCompare.FlatfileCompare.FlatfileCompare testInstance = new BizUnitCompare.FlatfileCompare.FlatfileCompare();

			StreamWriter goalWriter = new StreamWriter(PrepareFileSystem(testFilesPath + "test.goal"));

			StringBuilder expectedData = new StringBuilder();
			expectedData.AppendLine("this is the input");
			expectedData.AppendLine("should not be touched");

			goalWriter.Write(expectedData.ToString());
			goalWriter.Dispose();

			Assert.Throws<FileNotFoundException>(delegate { testInstance.Execute(configPart, context); });
		}

		[Test]
		public void ExecuteApplicationException_1()
		{
			BizUnitCompare.FlatfileCompare.FlatfileCompare testInstance = new BizUnitCompare.FlatfileCompare.FlatfileCompare();

			StreamWriter testWriter = new StreamWriter(PrepareFileSystem(testFilesPath + "test.test"));
			StreamWriter goalWriter = new StreamWriter(PrepareFileSystem(testFilesPath + "test.goal"));

			StringBuilder outputData = new StringBuilder();
			outputData.AppendLine("this is the input");
			outputData.AppendLine("should not be touched - too long");

			StringBuilder expectedData = new StringBuilder();
			expectedData.AppendLine("this is the input");
			expectedData.AppendLine("should not be touched");

			testWriter.Write(outputData.ToString());
			goalWriter.Write(expectedData.ToString());

			testWriter.Dispose();
			goalWriter.Dispose();

			Assert.Throws<ApplicationException>(delegate { testInstance.Execute(configPart, context); });
		}

		[Test]
		public void ExecuteApplicationException_2()
		{
			BizUnitCompare.FlatfileCompare.FlatfileCompare testInstance = new BizUnitCompare.FlatfileCompare.FlatfileCompare();

			StreamWriter testWriter = new StreamWriter(PrepareFileSystem(testFilesPath + "test.test"));
			StreamWriter goalWriter = new StreamWriter(PrepareFileSystem(testFilesPath + "test.goal"));

			StringBuilder outputData = new StringBuilder();
			outputData.AppendLine("this is the input");
			outputData.AppendLine("should not be touched");

			StringBuilder expectedData = new StringBuilder();
			expectedData.AppendLine("thIs is the input");
			expectedData.AppendLine("should not be touched");

			testWriter.Write(outputData.ToString());
			goalWriter.Write(expectedData.ToString());

			testWriter.Dispose();
			goalWriter.Dispose();

			Assert.Throws<ApplicationException>(delegate { testInstance.Execute(configPart, context); });
		}

		[Test]
		public void ExecuteNoException()
		{
			BizUnitCompare.FlatfileCompare.FlatfileCompare testInstance = new BizUnitCompare.FlatfileCompare.FlatfileCompare();

			StreamWriter testWriter = new StreamWriter(PrepareFileSystem(testFilesPath + "test.test"));
			StreamWriter goalWriter = new StreamWriter(PrepareFileSystem(testFilesPath + "test.goal"));

			StringBuilder outputData = new StringBuilder();
			outputData.AppendLine("this is the input");
			outputData.AppendLine("should not be touched");

			StringBuilder expectedData = new StringBuilder();
			expectedData.AppendLine("this is the input");
			expectedData.AppendLine("should not be touched");

			testWriter.Write(outputData.ToString());
			goalWriter.Write(expectedData.ToString());

			testWriter.Dispose();
			goalWriter.Dispose();

			Assert.DoesNotThrow(delegate { testInstance.Execute(configPart, context); });
		}

		private static FileStream PrepareFileSystem(string fileToCreatePath)
		{
			Directory.CreateDirectory(new FileInfo(fileToCreatePath).DirectoryName);
			return File.Create(fileToCreatePath);
		}
		
		private void CleanFileSystem()
		{
			foreach (var file in Directory.GetFiles(testFilesPath))
			{
				File.Delete(file);
			}
		}
	}
}
