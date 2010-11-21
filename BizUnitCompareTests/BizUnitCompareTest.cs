using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using NUnit.Framework;
using System.IO;

namespace BizUnitCompareTests
{
	[TestFixture]
	class BizUnitCompareTest
	{
		[Test]
		public void GetFoundFilePathFileNotFound()
		{
			BizUnit.Context context = new BizUnit.Context();
			BizUnitCompare.FlatfileCompare.BizUnitFlatfileCompareConfiguration configuration = new BizUnitCompare.FlatfileCompare.BizUnitFlatfileCompareConfiguration();
			
			configuration.SearchDirectory = Directory.GetCurrentDirectory();
			configuration.Filter = "*.should.not.be.found";
			configuration.Timeout = 200;
			Assert.Throws<FileNotFoundException>(delegate { BizUnitCompare.BizUnitCompare.GetFoundFilePath(context, configuration); });

		}

		[Test]
		public void GetFoundFilePathArgumentException()
		{
			BizUnit.Context context = new BizUnit.Context();
			BizUnitCompare.FlatfileCompare.BizUnitFlatfileCompareConfiguration configuration = new BizUnitCompare.FlatfileCompare.BizUnitFlatfileCompareConfiguration();

			configuration.SearchDirectory = @"";
			configuration.Filter = "*.should.not.be.found";
			configuration.Timeout = 1000;
			Assert.Throws<ArgumentException>(delegate { BizUnitCompare.BizUnitCompare.GetFoundFilePath(context, configuration); });
			Assert.Throws<ArgumentNullException>(delegate { BizUnitCompare.BizUnitCompare.GetFoundFilePath(context, null); });
			Assert.Throws<ArgumentNullException>(delegate { BizUnitCompare.BizUnitCompare.GetFoundFilePath(null, configuration); });
			Assert.Throws<ArgumentNullException>(delegate { BizUnitCompare.BizUnitCompare.GetFoundFilePath(null, null); });
		}

		[Test]
		public void GetFoundFileDirectoryNotFoundException()
		{
			BizUnit.Context context = new BizUnit.Context();
			BizUnitCompare.FlatfileCompare.BizUnitFlatfileCompareConfiguration configuration = new BizUnitCompare.FlatfileCompare.BizUnitFlatfileCompareConfiguration();

			configuration.SearchDirectory = @"c:\thisFolderShouldnotExist";
			configuration.Filter = "*.should.not.be.found";
			configuration.Timeout = 1000;
			Assert.Throws<DirectoryNotFoundException>(delegate { BizUnitCompare.BizUnitCompare.GetFoundFilePath(context, configuration); });
		}

		[Test]
		public void GetFoundFileTimeout()
		{
			BizUnit.Context context = new BizUnit.Context();
			BizUnitCompare.FlatfileCompare.BizUnitFlatfileCompareConfiguration configuration = new BizUnitCompare.FlatfileCompare.BizUnitFlatfileCompareConfiguration();

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
			catch (Exception)
			{
			}
			long millisecondsPassed = stopwatch.ElapsedMilliseconds;

			Assert.GreaterOrEqual(millisecondsPassed, configuration.Timeout);
			Assert.LessOrEqual(millisecondsPassed, configuration.Timeout + 120); // 120 is added since the thread sleep is set to 100 ms, 20 ms for added execution time
		}

		[Test]
		public void GetFoundFileFindFile()
		{
			BizUnit.Context context = new BizUnit.Context();
			BizUnitCompare.FlatfileCompare.BizUnitFlatfileCompareConfiguration configuration = new BizUnitCompare.FlatfileCompare.BizUnitFlatfileCompareConfiguration();

			configuration.SearchDirectory = Directory.GetCurrentDirectory();
			configuration.Filter = "*.test";
			configuration.Timeout = 1000;

			string fileToCreatePath = Directory.GetCurrentDirectory() + @"\test.test"; ;
			
			FileStream fileStream = PrepareFileSystem(configuration, fileToCreatePath);
			fileStream.Dispose();

			string foundFile = BizUnitCompare.BizUnitCompare.GetFoundFilePath(context, configuration);
			Assert.AreEqual(fileToCreatePath, foundFile);
		}

		[Test]
		public void GetFoundFileLockedFile()
		{
			BizUnit.Context context = new BizUnit.Context();
			BizUnitCompare.FlatfileCompare.BizUnitFlatfileCompareConfiguration configuration = new BizUnitCompare.FlatfileCompare.BizUnitFlatfileCompareConfiguration();

			configuration.SearchDirectory = Directory.GetCurrentDirectory();
			configuration.Filter = "*.test";
			configuration.Timeout = 1000;

			string fileToCreatePath = Directory.GetCurrentDirectory() + @"\test.test"; ;

			FileStream fileStream = PrepareFileSystem(configuration, fileToCreatePath);
			Assert.Throws<FileNotFoundException>(delegate { BizUnitCompare.BizUnitCompare.GetFoundFilePath(context, configuration); });
			fileStream.Dispose();
		}

		private static FileStream PrepareFileSystem(BizUnitCompare.FlatfileCompare.BizUnitFlatfileCompareConfiguration configuration, string fileToCreatePath)
		{
			CleanFileSystem(configuration);
			return File.Create(fileToCreatePath);
		}

		private static void CleanFileSystem(BizUnitCompare.FlatfileCompare.BizUnitFlatfileCompareConfiguration configuration)
		{
			foreach (var filePath in Directory.GetFiles(configuration.SearchDirectory, configuration.Filter))
			{
				File.Delete(filePath);
			}
		}
	}
}
