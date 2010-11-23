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

namespace BizUnitCompareTests.XmlCompare
{
	[TestFixture]
	public class BizUnitXmlCompareConfigurationTest
	{
		[Test]
		public void FilterProperty()
		{
			string testValue = "testValue";
			BizUnitCompare.XmlCompare.BizUnitXmlCompareConfiguration testInstance = new BizUnitCompare.XmlCompare.BizUnitXmlCompareConfiguration();
			testInstance.Filter = testValue;
			Assert.AreEqual(testValue, testInstance.Filter);
		}

		[Test]
		public void SearchDirectoryProperty()
		{
			string testValue = "testValue";
			BizUnitCompare.XmlCompare.BizUnitXmlCompareConfiguration testInstance = new BizUnitCompare.XmlCompare.BizUnitXmlCompareConfiguration();
			testInstance.SearchDirectory = testValue;
			Assert.AreEqual(testValue, testInstance.SearchDirectory);
		}

		[Test]
		public void TimeoutProperty()
		{
			uint testValue = 10000;
			BizUnitCompare.XmlCompare.BizUnitXmlCompareConfiguration testInstance = new BizUnitCompare.XmlCompare.BizUnitXmlCompareConfiguration();
			testInstance.Timeout = testValue;
			Assert.AreEqual(testValue, testInstance.Timeout);
		}

		[Test]
		public void GoalFilePathProperty()
		{
			string testValue = "testValue";
			BizUnitCompare.XmlCompare.BizUnitXmlCompareConfiguration testInstance = new BizUnitCompare.XmlCompare.BizUnitXmlCompareConfiguration();
			testInstance.GoalFilePath = testValue;
			Assert.AreEqual(testValue, testInstance.GoalFilePath);
		}

		[Test]
		public void DeleteFileProperty()
		{
			bool testValue = false;
			BizUnitCompare.XmlCompare.BizUnitXmlCompareConfiguration testInstance = new BizUnitCompare.XmlCompare.BizUnitXmlCompareConfiguration();
			testInstance.DeleteFile = testValue;
			Assert.AreEqual(testValue, testInstance.DeleteFile);
		}

		[Test]
		public void IgnoreChildOrderProperty()
		{
			bool testValue = false;
			BizUnitCompare.XmlCompare.BizUnitXmlCompareConfiguration testInstance = new BizUnitCompare.XmlCompare.BizUnitXmlCompareConfiguration();
			testInstance.IgnoreChildOrder = testValue;
			Assert.AreEqual(testValue, testInstance.IgnoreChildOrder);
		}

		[Test]
		public void IgnoreCommentsProperty()
		{
			bool testValue = true;
			BizUnitCompare.XmlCompare.BizUnitXmlCompareConfiguration testInstance = new BizUnitCompare.XmlCompare.BizUnitXmlCompareConfiguration();
			testInstance.IgnoreComments = testValue;
			Assert.AreEqual(testValue, testInstance.IgnoreComments);
		}

		[Test]
		public void ElementsToExcludeProperty()
		{
			List<string> testValue = new List<string>();
			testValue.Add("this is an item");
			BizUnitCompare.XmlCompare.BizUnitXmlCompareConfiguration testInstance = new BizUnitCompare.XmlCompare.BizUnitXmlCompareConfiguration();
			testInstance.ElementsToExclude = testValue;
			Assert.AreEqual(testValue, testInstance.ElementsToExclude);
		}

		[Test]
		public void AttributesToExcludeProperty()
		{
			List<BizUnitCompare.XmlCompare.Attribute> testValue = new List<BizUnitCompare.XmlCompare.Attribute>();
			BizUnitCompare.XmlCompare.Attribute testAttribute = new BizUnitCompare.XmlCompare.Attribute();
			testAttribute.Name = "name";
			testAttribute.ParentElementXPath = "xpath";
			testValue.Add(testAttribute);
			BizUnitCompare.XmlCompare.BizUnitXmlCompareConfiguration testInstance = new BizUnitCompare.XmlCompare.BizUnitXmlCompareConfiguration();
			testInstance.AttributesToExclude = testValue;
			Assert.AreEqual("name", testInstance.AttributesToExclude[0].Name);
			Assert.AreEqual("xpath", testInstance.AttributesToExclude[0].ParentElementXPath);
		}

		[Test]
		public void StringsToSearchAndReplaceProperty()
		{
			List<BizUnitCompare.XmlCompare.Replacement> testValue = new List<BizUnitCompare.XmlCompare.Replacement>();
			BizUnitCompare.XmlCompare.Replacement testReplacement = new BizUnitCompare.XmlCompare.Replacement();
			testReplacement.NewString = "new";
			testReplacement.OldString = "old";
			testValue.Add(testReplacement);
			BizUnitCompare.XmlCompare.BizUnitXmlCompareConfiguration testInstance = new BizUnitCompare.XmlCompare.BizUnitXmlCompareConfiguration();
			testInstance.StringsToSearchAndReplace = testValue;
			Assert.AreEqual("new", testInstance.StringsToSearchAndReplace[0].NewString);
			Assert.AreEqual("old", testInstance.StringsToSearchAndReplace[0].OldString);
		}

		[Test]
		public void Constructor()
		{
			BizUnitCompare.XmlCompare.BizUnitXmlCompareConfiguration testInstance = new BizUnitCompare.XmlCompare.BizUnitXmlCompareConfiguration();

			Assert.NotNull(testInstance.ElementsToExclude);
			Assert.NotNull(testInstance.AttributesToExclude);
			Assert.NotNull(testInstance.StringsToSearchAndReplace);
		}
	}
}
