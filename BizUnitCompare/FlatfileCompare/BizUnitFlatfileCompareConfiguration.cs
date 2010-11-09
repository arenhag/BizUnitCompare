using System.Collections.Generic;
using System.Xml;
using BizUnit;

namespace BizUnitCompare.FlatfileCompare
{
	internal sealed class BizUnitFlatfileCompareConfiguration : BizUnitCompareConfiguration
	{
		# region Backing property fields

		private bool _deleteFile;
		private List<Exclusion> _exclusions;
		private string _goalFilePath;

		# endregion

		# region Property Accessors

		internal bool DeleteFile
		{
			get { return _deleteFile; }
			set { _deleteFile = value; }
		}

		internal string GoalFilePath
		{
			get { return _goalFilePath; }
			set { _goalFilePath = value; }
		}

		internal List<Exclusion> Exclusions
		{
			get { return _exclusions; }
			set { _exclusions = value; }
		}

		#endregion

		internal BizUnitFlatfileCompareConfiguration()
		{
			Exclusions = new List<Exclusion>();
		}

		internal BizUnitFlatfileCompareConfiguration(XmlNode testconfig, Context context)
		{
			Load(testconfig, context);
		}

		internal override void Load(XmlNode testConfig, Context context)
		{
			GoalFilePath = context.ReadConfigAsString(testConfig, "GoalFile");
			SearchDirectory = context.ReadConfigAsString(testConfig, "SearchDirectory");
			Filter = context.ReadConfigAsString(testConfig, "Filter");
			Timeout = context.ReadConfigAsUInt32(testConfig, "Timeout");
			DeleteFile = context.ReadConfigAsBool(testConfig, "DeleteFile", true);
			Exclusions = ParseExclusions(testConfig);
		}

		private static List<Exclusion> ParseExclusions(XmlNode testConfig)
		{
			List<Exclusion> exclusions = new List<Exclusion>();

			XmlNodeList exclusionNodes = testConfig.SelectNodes("exclusions/rowType");

			if (exclusionNodes != null)
			{
				foreach (XmlNode exclusionNode in exclusionNodes)
				{
					Exclusion exclusion = new Exclusion();
					exclusion.RowIdentifyingRegularExpression = exclusionNode.Attributes["identifier"].Value;

					foreach (XmlNode positionNode in exclusionNode.ChildNodes)
					{
						ExclusionPositions positions = new ExclusionPositions();
						positions.StartPosition = int.Parse(positionNode.Attributes["startPosition"].Value);
						positions.EndPosition = int.Parse(positionNode.Attributes["endPosition"].Value);
						exclusion.ExclusionPositions.Add(positions);
					}
					exclusions.Add(exclusion);
				}
			}
			return exclusions;
		}
	}
}