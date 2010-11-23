#region License
/*
Copyright (c) 2010, Fredrik Arenhag
All rights reserved.

Redistribution and use in source and binary forms, with or without modification,
are permitted provided that the following conditions are met:
*	Redistributions of source code must retain the above copyright notice,
	this list of conditions and the following disclaimer.
*	Redistributions in binary form must reproduce the above copyright notice,
	this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
*	Neither the name of the FREDRIK ARENHAG nor the names of its contributors may be used to endorse or promote products
	derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY,
OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA,
OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY
OF SUCH DAMAGE.
*/
#endregion

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