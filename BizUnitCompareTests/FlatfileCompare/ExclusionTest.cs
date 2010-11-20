using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace BizUnitCompareTests.FlatfileCompare
{
	[TestFixture]
	class ExclusionTest
	{
		[Test]
		public void RowIdentifyingRegularExpression()
		{
			BizUnitCompare.FlatfileCompare.Exclusion testInstance = new BizUnitCompare.FlatfileCompare.Exclusion();
			string testValue = "testExpression";
			testInstance.RowIdentifyingRegularExpression = testValue;

			Assert.AreEqual(testValue, testInstance.RowIdentifyingRegularExpression);
		}

		[Test]
		public void ExclusionPositions()
		{
			List<BizUnitCompare.FlatfileCompare.ExclusionPositions> testInstance = new List<BizUnitCompare.FlatfileCompare.ExclusionPositions>();
			BizUnitCompare.FlatfileCompare.ExclusionPositions testValue = new BizUnitCompare.FlatfileCompare.ExclusionPositions();

			testValue.StartPosition = 10;
			testValue.EndPosition = 20;

			testInstance.Add(testValue);

			Assert.AreEqual(10, testInstance[0].StartPosition);
			Assert.AreEqual(20, testInstance[0].EndPosition);
		}

		[Test]
		public void Constructor()
		{
			BizUnitCompare.FlatfileCompare.Exclusion testInstance = new BizUnitCompare.FlatfileCompare.Exclusion();

			Assert.IsNotNull(testInstance.ExclusionPositions);
		}

		[Test]
		public void ExclusionBitMap()
		{
			BizUnitCompare.FlatfileCompare.Exclusion testInstance = new BizUnitCompare.FlatfileCompare.Exclusion();
			
			BizUnitCompare.FlatfileCompare.ExclusionPositions testValue = new BizUnitCompare.FlatfileCompare.ExclusionPositions();
			testValue.StartPosition = 1;
			testValue.EndPosition = 3;
			testInstance.ExclusionPositions.Add(testValue);

			testValue = new BizUnitCompare.FlatfileCompare.ExclusionPositions();
			testValue.StartPosition = 7;
			testValue.EndPosition = 10;
			testInstance.ExclusionPositions.Add(testValue);

			bool[] expectedVal = new bool[10];
			expectedVal[0] = true;
			expectedVal[1] = true;
			expectedVal[2] = true;
			expectedVal[6] = true;
			expectedVal[7] = true;
			expectedVal[8] = true;
			expectedVal[9] = true;
			bool[] retVal = testInstance.ExclusionBitMap;

			Assert.AreEqual(expectedVal,retVal);
		}
	}
}
