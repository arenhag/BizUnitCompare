#region License

// Copyright (c) 2010, Fredrik Arenhag
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// *	Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// *	Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// *	Neither the name of FREDRIK ARENHAG nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
// INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
// IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY,
// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA,
// OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

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
			_config.Load(@"FlatfileCompare\testConfig.xml");
			_configPart = _config.SelectSingleNode("/TestStep");

			_context = new Context();
			_context.Add("searchDirectory", _testFilesPath);
			_context.Add("deleteFile", "false");
			_context.Add("goalFile", _testFilesPath + "test.goal");
			_context.Add("filter", "test.test");
		}

		#endregion

		private readonly string _testFilesPath = Directory.GetCurrentDirectory() + @"\FlatfileCompare\testFiles\";
		private readonly XmlDocument _config = new XmlDocument();
		private XmlNode _configPart;
		private Context _context;

		private static FileStream PrepareFileSystem(string fileToCreatePath)
		{
			Directory.CreateDirectory(new FileInfo(fileToCreatePath).DirectoryName);
			return File.Create(fileToCreatePath);
		}

		private void CleanFileSystem()
		{
			Directory.CreateDirectory(_testFilesPath);
			foreach (string file in Directory.GetFiles(_testFilesPath))
			{
				File.Delete(file);
			}
		}

		[Test]
		public void ExecuteApplicationException1()
		{
			BizUnitCompare.FlatfileCompare.FlatfileCompare testInstance = new BizUnitCompare.FlatfileCompare.FlatfileCompare();

			StreamWriter testWriter = new StreamWriter(PrepareFileSystem(_testFilesPath + "test.test"));
			StreamWriter goalWriter = new StreamWriter(PrepareFileSystem(_testFilesPath + "test.goal"));

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

			Assert.Throws<ApplicationException>(delegate { testInstance.Execute(_configPart, _context); });
		}

		[Test]
		public void ExecuteApplicationException2()
		{
			BizUnitCompare.FlatfileCompare.FlatfileCompare testInstance = new BizUnitCompare.FlatfileCompare.FlatfileCompare();

			StreamWriter testWriter = new StreamWriter(PrepareFileSystem(_testFilesPath + "test.test"));
			StreamWriter goalWriter = new StreamWriter(PrepareFileSystem(_testFilesPath + "test.goal"));

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

			Assert.Throws<ApplicationException>(delegate { testInstance.Execute(_configPart, _context); });
		}

		[Test]
		public void ExecuteFileNotFoundException()
		{
			BizUnitCompare.FlatfileCompare.FlatfileCompare testInstance = new BizUnitCompare.FlatfileCompare.FlatfileCompare();

			StreamWriter goalWriter = new StreamWriter(PrepareFileSystem(_testFilesPath + "test.goal"));

			StringBuilder expectedData = new StringBuilder();
			expectedData.AppendLine("this is the input");
			expectedData.AppendLine("should not be touched");

			goalWriter.Write(expectedData.ToString());
			goalWriter.Dispose();

			Assert.Throws<FileNotFoundException>(delegate { testInstance.Execute(_configPart, _context); });
		}

		[Test]
		public void ExecuteFoundFileDeleted()
		{
			_context.Add("deleteFile", "true", true);
			ExecuteNoException();
			Assert.IsFalse(File.Exists(_context.GetValue("searchDirectory") + _context.GetValue("filter")));
		}

		[Test]
		public void ExecuteFoundFileNotDeleted()
		{
			_context.Add("deleteFile", "false", true);
			ExecuteNoException();
			Assert.IsTrue(File.Exists(_context.GetValue("searchDirectory") + _context.GetValue("filter")));
		}

		[Test]
		public void ExecuteNoException()
		{
			BizUnitCompare.FlatfileCompare.FlatfileCompare testInstance = new BizUnitCompare.FlatfileCompare.FlatfileCompare();

			StreamWriter testWriter = new StreamWriter(PrepareFileSystem(_testFilesPath + "test.test"));
			StreamWriter goalWriter = new StreamWriter(PrepareFileSystem(_testFilesPath + "test.goal"));

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

			Assert.DoesNotThrow(delegate { testInstance.Execute(_configPart, _context); });
		}
	}
}