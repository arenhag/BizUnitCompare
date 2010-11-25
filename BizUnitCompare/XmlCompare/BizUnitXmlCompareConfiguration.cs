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

using System.Collections.Generic;
using System.Xml;
using BizUnit;

namespace BizUnitCompare.XmlCompare
{
	internal sealed class BizUnitXmlCompareConfiguration : BizUnitCompareConfiguration
	{
		# region Backing property fields

		private List<Attribute> _attributesToExclude;
		private List<string> _elementsToExclude;
		private bool _ignoreChildOrder;
		private bool _ignoreComments;
		private List<Replacement> _stringsToSearchAndReplace;

		# endregion

		# region Property accessors

		internal bool IgnoreChildOrder
		{
			get { return _ignoreChildOrder; }
			set { _ignoreChildOrder = value; }
		}

		internal bool IgnoreComments
		{
			get { return _ignoreComments; }
			set { _ignoreComments = value; }
		}

		internal List<string> ElementsToExclude
		{
			get { return _elementsToExclude; }
			set { _elementsToExclude = value; }
		}

		internal List<Attribute> AttributesToExclude
		{
			get { return _attributesToExclude; }
			set { _attributesToExclude = value; }
		}

		internal List<Replacement> StringsToSearchAndReplace
		{
			get { return _stringsToSearchAndReplace; }
			set { _stringsToSearchAndReplace = value; }
		}

		# endregion

		internal BizUnitXmlCompareConfiguration()
		{
			ElementsToExclude = new List<string>();
			AttributesToExclude = new List<Attribute>();
			StringsToSearchAndReplace = new List<Replacement>();
		}

		internal BizUnitXmlCompareConfiguration(XmlNode testConfig, Context context)
		{
			Load(testConfig, context);
		}

		internal override void Load(XmlNode testConfig, Context context)
		{
			GoalFilePath = context.ReadConfigAsString(testConfig, "GoalFile");
			SearchDirectory = context.ReadConfigAsString(testConfig, "SearchDirectory");
			Filter = context.ReadConfigAsString(testConfig, "Filter");
			Timeout = context.ReadConfigAsUInt32(testConfig, "Timeout");
			DeleteFile = context.ReadConfigAsBool(testConfig, "DeleteFile", true);
			IgnoreChildOrder = context.ReadConfigAsBool(testConfig, "IgnoreChildOrder", true);
			IgnoreComments = context.ReadConfigAsBool(testConfig, "IgnoreComments", true);
			ElementsToExclude = ParseElementExclusions(testConfig);
			AttributesToExclude = ParseAttributeExclusions(testConfig);
			StringsToSearchAndReplace = ParseReplacements(testConfig);
		}

		private static List<Attribute> ParseAttributeExclusions(XmlNode testConfig)
		{
			List<Attribute> attributesToExclude = new List<Attribute>();
			XmlNodeList configuredAttributesToExclude = testConfig.SelectNodes("Exclusions/Attribute/ParentElement");
			if (configuredAttributesToExclude != null)
			{
				foreach (XmlNode attributeExclude in configuredAttributesToExclude)
				{
					Attribute attribute = new Attribute();
					attribute.ParentElementXPath = attributeExclude.InnerText;
					if (attributeExclude.NextSibling != null) attribute.Name = attributeExclude.NextSibling.InnerText;
					attributesToExclude.Add(attribute);
				}
			}
			return attributesToExclude;
		}

		private static List<string> ParseElementExclusions(XmlNode testConfig)
		{
			List<string> elementsToExclude = new List<string>();
			XmlNodeList configuredElementsToExclude = testConfig.SelectNodes("Exclusions/Element");
			if (configuredElementsToExclude != null)
			{
				foreach (XmlNode toExclude in configuredElementsToExclude)
				{
					elementsToExclude.Add(toExclude.InnerText);
				}
			}
			return elementsToExclude;
		}

		private static List<Replacement> ParseReplacements(XmlNode testConfig)
		{
			List<Replacement> stringsToSearchAndReplace = new List<Replacement>();
			XmlNodeList configuredReplacements = testConfig.SelectNodes("Replacements/Replace");
			if (configuredReplacements != null)
			{
				foreach (XmlNode configuredReplacement in configuredReplacements)
				{
					Replacement replacement = new Replacement();
					XmlNodeList children = configuredReplacement.ChildNodes;
					foreach (XmlNode child in children)
					{
						XmlCDataSection data;
						switch (child.Name)
						{
							case "OldString":
								data = child.FirstChild as XmlCDataSection;
								replacement.OldString = data != null ? data.Value : string.Empty;
								break;
							case "NewString":
								data = child.FirstChild as XmlCDataSection;
								replacement.NewString = data != null ? data.Value : string.Empty;
								break;
						}
					}
					stringsToSearchAndReplace.Add(replacement);
				}
			}
			return stringsToSearchAndReplace;
		}
	}
}