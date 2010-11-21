using System;
using System.IO;
using System.Xml;
using BizUnit;
using System.Globalization;

namespace BizUnitCompare.FlatfileCompare
{
	public class FlatfileCompare : ITestStep
	{
		#region ITestStep Members

		public void Execute(XmlNode testConfig, Context context)
		{
			BizUnitFlatfileCompareConfiguration configuration = new BizUnitFlatfileCompareConfiguration(testConfig, context);

			string foundFilePath = BizUnitCompare.GetFoundFilePath(context, configuration);

			using (MemoryStream cleanedFoundData = FlatfileCleaner.RemoveExclusions(foundFilePath, configuration.Exclusions))
			{
				using (MemoryStream cleanedGoalData = FlatfileCleaner.RemoveExclusions(configuration.GoalFilePath, configuration.Exclusions))
				{
					// just to be sure.
					cleanedFoundData.Seek(0, SeekOrigin.Begin);
					cleanedGoalData.Seek(0, SeekOrigin.Begin);

					if (cleanedFoundData.Length != cleanedGoalData.Length)
					{
						throw new ApplicationException(string.Format(CultureInfo.CurrentCulture, "Flatfile comparison failed (different length) between {0} and {1}.", foundFilePath, configuration.GoalFilePath));
					}

					try
					{
						do
						{
							int foundByte = cleanedFoundData.ReadByte();
							int goalByte = cleanedGoalData.ReadByte();
							if (foundByte != goalByte)
							{
								throw new ApplicationException(string.Format(CultureInfo.CurrentCulture, "Flatfile comparison failed at offset {2} between {0} and {1}.", foundFilePath, configuration.GoalFilePath, cleanedFoundData.Position - 1));
							}
						}
						while (!(cleanedFoundData.Position >= cleanedFoundData.Length));
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
			}
		}

		#endregion
	}
}