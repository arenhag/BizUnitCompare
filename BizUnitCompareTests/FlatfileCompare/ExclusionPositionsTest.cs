using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace BizUnitCompareTests.FlatfileCompare
{
	[TestFixture]
	public class ExclusionPositionsTest
	{
		[Test]
		public void EndPosition()
		{
			BizUnitCompare.FlatfileCompare.ExclusionPositions testInstance = new BizUnitCompare.FlatfileCompare.ExclusionPositions();
			testInstance.EndPosition = 10;

			Assert.AreEqual(10, testInstance.EndPosition);
		}

		[Test]
		public void StartPosition()
		{
			BizUnitCompare.FlatfileCompare.ExclusionPositions testInstance = new BizUnitCompare.FlatfileCompare.ExclusionPositions();
			testInstance.StartPosition = 6;

			Assert.AreEqual(6, testInstance.StartPosition);
		}
	}
}
