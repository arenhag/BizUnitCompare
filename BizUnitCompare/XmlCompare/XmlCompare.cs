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
using System.IO;
using System.Text;
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
				StringBuilder diff;
				bool comparisonResult = Compare(out diff, configuration.GoalFilePath, foundFilePath, configuration.StringsToSearchAndReplace, configuration.ElementsToExclude, configuration.AttributesToExclude, configuration.IgnoreChildOrder, configuration.IgnoreComments);
				if (!comparisonResult)
				{
					context.LogInfo(string.Format(CultureInfo.CurrentCulture, "This is the diff result: {0}", diff));
					throw new ApplicationException(string.Format(CultureInfo.CurrentCulture, "Xml comparison failed between {0} and {1}. This is the diff result: {2}", foundFilePath, configuration.GoalFilePath, diff));
				}
				context.LogInfo("Files are identical.");
			}
			finally
			{
				if (!string.IsNullOrEmpty(foundFilePath) && configuration.DeleteFile)
				{
					File.Delete(foundFilePath);
					context.LogInfo(string.Format(CultureInfo.CurrentCulture, "Found file ({0}) deleted.", foundFilePath));
				}
			}
		}

		#endregion

		private static bool Compare(out StringBuilder diff, string goalFilePath, string foundFilePath, IEnumerable<Replacement> replacements, IEnumerable<string> elementsToExclude, IEnumerable<Attribute> attributesToExclude, bool ignoreChildOrder, bool ignoreComments)
		{
			bool retVal;
			diff = new StringBuilder();

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

				XmlWriter diffWriter = XmlWriter.Create(diff);
				retVal = comparer.Compare(goalXmlReader, foundFileXmlReader, diffWriter);
				diffWriter.Flush();
			}
			finally
			{
				if (goalXmlReader != null) goalXmlReader.Close();
				if (foundFileXmlReader != null) foundFileXmlReader.Close();

				if (goalFileReader != null) goalFileReader.Dispose();
				if (foundFileReader != null) foundFileReader.Dispose();
			}

			return retVal;
		}
	}
}