using System.Collections.Generic;
using System.Xml;
using BizUnit;

namespace BizUnitCompare.XmlCompare
{
	internal sealed class BizUnitXmlCompareConfiguration : BizUnitCompareConfiguration
	{
		# region Backing property fields

		private List<Attribute> _attributesToExclude;
		private bool _deleteFile;
		private List<string> _elementsToExclude;
		private string _goalFilePath;
		private bool _ignoreChildOrder;
		private bool _ignoreComments;
		private List<Replacement> _stringsToSearchAndReplace;

		# endregion

		# region Property accessors

		internal string GoalFilePath
		{
			get { return _goalFilePath; }
			set { _goalFilePath = value; }
		}

		internal bool DeleteFile
		{
			get { return _deleteFile; }
			set { _deleteFile = value; }
		}

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
					attribute.Name = attributeExclude.NextSibling.InnerText;
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