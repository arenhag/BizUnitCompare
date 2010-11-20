using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace BizUnitCompareTests.FlatfileCompare
{
	[TestFixture]
	public class BizUnitFlatfileCompareConfigurationTest
	{
		[Test]
		public void FilterProperty()
		{
			string testValue = "testValue";
			BizUnitCompare.FlatfileCompare.BizUnitFlatfileCompareConfiguration testInstance = new BizUnitCompare.FlatfileCompare.BizUnitFlatfileCompareConfiguration();
			testInstance.Filter = testValue;
			Assert.AreEqual(testValue, testInstance.Filter);
		}

		[Test]
		public void SearchDirectoryProperty()
		{
			string testValue = "testValue";
			BizUnitCompare.FlatfileCompare.BizUnitFlatfileCompareConfiguration testInstance = new BizUnitCompare.FlatfileCompare.BizUnitFlatfileCompareConfiguration();
			testInstance.SearchDirectory = testValue;
			Assert.AreEqual(testValue, testInstance.SearchDirectory);
		}

		[Test]
		public void TimeoutProperty()
		{
			uint testValue = 10000;
			BizUnitCompare.FlatfileCompare.BizUnitFlatfileCompareConfiguration testInstance = new BizUnitCompare.FlatfileCompare.BizUnitFlatfileCompareConfiguration();
			testInstance.Timeout = testValue;
			Assert.AreEqual(testValue, testInstance.Timeout);
		}

		[Test]
		public void GoalFilePathProperty()
		{
			string testValue = "testValue";
			BizUnitCompare.FlatfileCompare.BizUnitFlatfileCompareConfiguration testInstance = new BizUnitCompare.FlatfileCompare.BizUnitFlatfileCompareConfiguration();
			testInstance.GoalFilePath = testValue;
			Assert.AreEqual(testValue, testInstance.GoalFilePath);
		}

		[Test]
		public void DeleteFileProperty()
		{
			bool testValue = false;
			BizUnitCompare.FlatfileCompare.BizUnitFlatfileCompareConfiguration testInstance = new BizUnitCompare.FlatfileCompare.BizUnitFlatfileCompareConfiguration();
			testInstance.DeleteFile = testValue;
			Assert.AreEqual(testValue, testInstance.DeleteFile);
		}

		[Test]
		public void ExclusionProperty()
		{
			List<BizUnitCompare.FlatfileCompare.Exclusion> testList = new List<BizUnitCompare.FlatfileCompare.Exclusion>();
			BizUnitCompare.FlatfileCompare.Exclusion testValue = new BizUnitCompare.FlatfileCompare.Exclusion();
			BizUnitCompare.FlatfileCompare.ExclusionPositions testExclusionPositions = new BizUnitCompare.FlatfileCompare.ExclusionPositions();
			testExclusionPositions.StartPosition = 10;
			testExclusionPositions.EndPosition = 20;
			testValue.ExclusionPositions.Add(testExclusionPositions);
			testList.Add(testValue);
			BizUnitCompare.FlatfileCompare.BizUnitFlatfileCompareConfiguration testInstance = new BizUnitCompare.FlatfileCompare.BizUnitFlatfileCompareConfiguration();
			testInstance.Exclusions = testList;
			Assert.AreEqual(10, testInstance.Exclusions[0].ExclusionPositions[0].StartPosition);
			Assert.AreEqual(20, testInstance.Exclusions[0].ExclusionPositions[0].EndPosition);
		}

		[Test]
		public void Constructor()
		{
			BizUnitCompare.FlatfileCompare.BizUnitFlatfileCompareConfiguration testInstance = new BizUnitCompare.FlatfileCompare.BizUnitFlatfileCompareConfiguration();

			Assert.IsNotNull(testInstance.Exclusions);
		}
		
	}
}
