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
