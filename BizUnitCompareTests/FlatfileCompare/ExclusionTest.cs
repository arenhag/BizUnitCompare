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
