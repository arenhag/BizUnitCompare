using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace BizUnitCompareTests.FlatfileCompare
{
	[TestFixture]
	class FlatfileCleanerTest
	{
		private readonly string documentPath = Directory.GetCurrentDirectory() + @"\test.test";

		[SetUp]
		[TearDown]
		public void SetUpAndTearDown()
		{
			File.Delete(documentPath);
		}

		[Test]
		public void RemoveExclusions()
		{
			StringBuilder inputData = new StringBuilder();
			inputData.AppendLine("this is the input");
			inputData.AppendLine("should not be touched");

			StringBuilder expectedData = new StringBuilder();
			expectedData.AppendLine("thisistheinput");
			expectedData.AppendLine("should not be touched");

			StreamWriter writer = File.CreateText(documentPath);
			writer.Write(inputData.ToString());
			writer.Dispose();

			List<BizUnitCompare.FlatfileCompare.Exclusion> exclusions = new List<BizUnitCompare.FlatfileCompare.Exclusion>();
			BizUnitCompare.FlatfileCompare.Exclusion exclusion = new BizUnitCompare.FlatfileCompare.Exclusion();
			exclusion.RowIdentifyingRegularExpression = "this";

			BizUnitCompare.FlatfileCompare.ExclusionPositions position = new BizUnitCompare.FlatfileCompare.ExclusionPositions();
			position.StartPosition = 5;
			position.EndPosition = 5;
			exclusion.ExclusionPositions.Add(position);
			
			position = new BizUnitCompare.FlatfileCompare.ExclusionPositions();
			position.StartPosition = 8;
			position.EndPosition = 8;
			exclusion.ExclusionPositions.Add(position);
			
			position = new BizUnitCompare.FlatfileCompare.ExclusionPositions();
			position.StartPosition = 12;
			position.EndPosition = 12;
			exclusion.ExclusionPositions.Add(position);

			exclusions.Add(exclusion);

			MemoryStream returnStream = BizUnitCompare.FlatfileCompare.FlatfileCleaner.RemoveExclusions(documentPath, exclusions);
			StreamReader returnReader = new StreamReader(returnStream);
			string returnString = returnReader.ReadToEnd();
			returnStream.Dispose();

			Assert.AreEqual(expectedData.ToString(), returnString);
		}
	}
}
