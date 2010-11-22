using System;
using System.IO;
using System.Text;
using System.Xml;
using BizUnit;
using NUnit.Framework;

namespace BizUnitCompareTests.FlatfileCompare
{
	[TestFixture]
	internal class FlatfileCompareTest
	{
		#region Setup/Teardown

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

		#endregion

		private readonly string testFilesPath = Directory.GetCurrentDirectory() + @"\FlatfileCompare\testFiles\";
		private readonly XmlDocument config = new XmlDocument();
		private XmlNode configPart;
		private Context context;

		private static FileStream PrepareFileSystem(string fileToCreatePath)
		{
			Directory.CreateDirectory(new FileInfo(fileToCreatePath).DirectoryName);
			return File.Create(fileToCreatePath);
		}

		private void CleanFileSystem()
		{
			Directory.CreateDirectory(testFilesPath);
			foreach (string file in Directory.GetFiles(testFilesPath))
			{
				File.Delete(file);
			}
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
		public void ExecuteFoundFileDeleted()
		{
			context.Add("deleteFile", "true", true);
			ExecuteNoException();
			Assert.IsFalse(File.Exists(context.GetValue("searchDirectory") + context.GetValue("filter")));
		}

		[Test]
		public void ExecuteFoundFileNotDeleted()
		{
			context.Add("deleteFile", "false", true);
			ExecuteNoException();
			Assert.IsTrue(File.Exists(context.GetValue("searchDirectory") + context.GetValue("filter")));
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
	}
}