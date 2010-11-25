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
using System.Diagnostics;
using System.IO;
using BizUnit;
using BizUnitCompare.FlatfileCompare;
using NUnit.Framework;

namespace BizUnitCompareTests
{
	[TestFixture]
	internal class BizUnitCompareTest
	{
		private static FileStream PrepareFileSystem(BizUnitFlatfileCompareConfiguration configuration, string fileToCreatePath)
		{
			CleanFileSystem(configuration);
			return File.Create(fileToCreatePath);
		}

		private static void CleanFileSystem(BizUnitFlatfileCompareConfiguration configuration)
		{
			foreach (string filePath in Directory.GetFiles(configuration.SearchDirectory, configuration.Filter))
			{
				File.Delete(filePath);
			}
		}

		[Test]
		public void GetFoundFileDirectoryNotFoundException()
		{
			Context context = new Context();
			BizUnitFlatfileCompareConfiguration configuration = new BizUnitFlatfileCompareConfiguration();

			configuration.SearchDirectory = @"c:\thisFolderShouldnotExist";
			configuration.Filter = "*.should.not.be.found";
			configuration.Timeout = 1000;
			Assert.Throws<DirectoryNotFoundException>(delegate { BizUnitCompare.BizUnitCompare.GetFoundFilePath(context, configuration); });
		}

		[Test]
		public void GetFoundFileFindFile()
		{
			Context context = new Context();
			BizUnitFlatfileCompareConfiguration configuration = new BizUnitFlatfileCompareConfiguration();

			configuration.SearchDirectory = Directory.GetCurrentDirectory();
			configuration.Filter = "*.test";
			configuration.Timeout = 1000;

			string fileToCreatePath = Directory.GetCurrentDirectory() + @"\test.test";

			FileStream fileStream = PrepareFileSystem(configuration, fileToCreatePath);
			fileStream.Dispose();

			string foundFile = BizUnitCompare.BizUnitCompare.GetFoundFilePath(context, configuration);
			Assert.AreEqual(fileToCreatePath, foundFile);
		}

		[Test]
		public void GetFoundFileLockedFile()
		{
			Context context = new Context();
			BizUnitFlatfileCompareConfiguration configuration = new BizUnitFlatfileCompareConfiguration();

			configuration.SearchDirectory = Directory.GetCurrentDirectory();
			configuration.Filter = "*.test";
			configuration.Timeout = 1000;

			string fileToCreatePath = Directory.GetCurrentDirectory() + @"\test.test";

			FileStream fileStream = PrepareFileSystem(configuration, fileToCreatePath);
			Assert.Throws<FileNotFoundException>(delegate { BizUnitCompare.BizUnitCompare.GetFoundFilePath(context, configuration); });
			fileStream.Dispose();
		}

		[Test]
		public void GetFoundFilePathArgumentException()
		{
			Context context = new Context();
			BizUnitFlatfileCompareConfiguration configuration = new BizUnitFlatfileCompareConfiguration();

			configuration.SearchDirectory = @"";
			configuration.Filter = "*.should.not.be.found";
			configuration.Timeout = 1000;
			Assert.Throws<ArgumentException>(delegate { BizUnitCompare.BizUnitCompare.GetFoundFilePath(context, configuration); });
			Assert.Throws<ArgumentNullException>(delegate { BizUnitCompare.BizUnitCompare.GetFoundFilePath(context, null); });
			Assert.Throws<ArgumentNullException>(delegate { BizUnitCompare.BizUnitCompare.GetFoundFilePath(null, configuration); });
			Assert.Throws<ArgumentNullException>(delegate { BizUnitCompare.BizUnitCompare.GetFoundFilePath(null, null); });
		}

		[Test]
		public void GetFoundFilePathFileNotFound()
		{
			Context context = new Context();
			BizUnitFlatfileCompareConfiguration configuration = new BizUnitFlatfileCompareConfiguration();

			configuration.SearchDirectory = Directory.GetCurrentDirectory();
			configuration.Filter = "*.should.not.be.found";
			configuration.Timeout = 200;
			Assert.Throws<FileNotFoundException>(delegate { BizUnitCompare.BizUnitCompare.GetFoundFilePath(context, configuration); });
		}

		[Test]
		public void GetFoundFileTimeout()
		{
			Context context = new Context();
			BizUnitFlatfileCompareConfiguration configuration = new BizUnitFlatfileCompareConfiguration();

			configuration.SearchDirectory = @"c:\";
			configuration.Filter = "*.should.not.be.found";
			configuration.Timeout = 1000;

			Stopwatch stopwatch = new Stopwatch();
			try
			{
				stopwatch.Start();
				BizUnitCompare.BizUnitCompare.GetFoundFilePath(context, configuration);
				stopwatch.Stop();
			}
// ReSharper disable EmptyGeneralCatchClause
			catch (Exception)
// ReSharper restore EmptyGeneralCatchClause
			{
			}
			long millisecondsPassed = stopwatch.ElapsedMilliseconds;

			Assert.GreaterOrEqual(millisecondsPassed, configuration.Timeout);
			Assert.LessOrEqual(millisecondsPassed, configuration.Timeout + 120); // 120 is added since the thread sleep is set to 100 ms, 20 ms for added execution time
		}
	}
}