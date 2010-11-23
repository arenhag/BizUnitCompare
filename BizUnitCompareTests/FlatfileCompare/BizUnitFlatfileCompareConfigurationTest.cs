﻿#region License
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
