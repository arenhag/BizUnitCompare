using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using BizUnit;
using Microsoft.XmlDiffPatch;
using System.Globalization;

namespace BizUnitCompare.XmlCompare
{
	public class XmlCompare : BizUnitCompare, ITestStep
	{
		#region ITestStep Members

		public void Execute(XmlNode testConfig, Context context)
		{
			BizUnitXmlCompareConfiguration configuration = new BizUnitXmlCompareConfiguration(testConfig, context);

			string foundFilePath = GetFoundFilePath(context, configuration);

			try
			{
				if (!Compare(configuration.GoalFilePath, foundFilePath, configuration.StringsToSearchAndReplace, configuration.ElementsToExclude, configuration.AttributesToExclude, configuration.IgnoreChildOrder, configuration.IgnoreComments))
				{
					throw new ApplicationException(string.Format(CultureInfo.CurrentCulture, "Xml comparison failed between {0} and {1}.", foundFilePath, configuration.GoalFilePath));
				}
				context.LogInfo("Files are identical.");
			}
			finally
			{
				if (string.IsNullOrEmpty(foundFilePath) && configuration.DeleteFile)
				{
					File.Delete(foundFilePath);
					context.LogInfo(string.Format(CultureInfo.CurrentCulture, "Found file ({0}) deleted.", foundFilePath));
				}
			}
		}

		#endregion

		private static bool Compare(string goalFilePath, string foundFilePath, IEnumerable<Replacement> replacements, IEnumerable<string> elementsToExclude, IEnumerable<Attribute> attributesToExclude, bool ignoreChildOrder, bool ignoreComments)
		{
			bool retVal;

			XmlDocument foundFileDocument = new XmlDocument();
			XmlDocument goalFileDocument = new XmlDocument();

			TextReader goalFileReader = null;
			TextReader foundFileReader = null;

			XmlReader foundFileXmlReader = null;
			XmlReader goalXmlReader = null;

			try
			{
				goalFileReader = new StringReader(XmlDocumentCleaner.ReplaceInDocument(goalFilePath, replacements));
				foundFileReader = new StringReader(XmlDocumentCleaner.ReplaceInDocument(foundFilePath, replacements));

				foundFileDocument.Load(foundFileReader);
				goalFileDocument.Load(goalFileReader);

				foreach (string element in elementsToExclude)
				{
					XmlDocumentCleaner.RemoveElements(ref foundFileDocument, element);
					XmlDocumentCleaner.RemoveElements(ref goalFileDocument, element);
				}

				foreach (Attribute attribute in attributesToExclude)
				{
					XmlDocumentCleaner.RemoveAttribute(ref foundFileDocument, attribute);
					XmlDocumentCleaner.RemoveAttribute(ref goalFileDocument, attribute);
				}

				foundFileXmlReader = new XmlNodeReader(foundFileDocument);
				goalXmlReader = new XmlNodeReader(goalFileDocument);

				XmlDiff comparer = new XmlDiff();
				comparer.IgnoreChildOrder = ignoreChildOrder;
				comparer.IgnoreComments = ignoreComments;

				//XmlWriter diff = XmlWriter.Create(@"C:\Documents and Settings\Administrator\Desktop\testfiles\xmldiff\diff.xml");
				retVal = comparer.Compare(goalXmlReader, foundFileXmlReader);
			}
			finally
			{
				goalXmlReader.Close();
				foundFileXmlReader.Close();

				goalFileReader.Dispose();
				foundFileReader.Dispose();
			}

			return retVal;
		}
	}
}