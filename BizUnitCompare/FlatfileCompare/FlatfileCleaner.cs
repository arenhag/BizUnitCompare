using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace BizUnitCompare.FlatfileCompare
{
	internal class FlatfileCleaner
	{
		internal static MemoryStream RemoveExclusions(string documentPath, List<Exclusion> exclusions)
		{
			MemoryStream cleanedData = new MemoryStream();
			StreamWriter cleanedDataWriter = new StreamWriter(cleanedData);

			using (FileStream foundFileStream = File.Open(documentPath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (StreamReader foundFileReader = new StreamReader(foundFileStream))
				{
					do
					{
						string line = foundFileReader.ReadLine();
						string strippedLine = line;

						foreach (Exclusion exclusion in exclusions)
						{
							Regex identifyingExpression = new Regex(exclusion.RowIdentifyingRegularExpression);
							if (identifyingExpression.Match(line).Success)
							{
								strippedLine = StripExclusionFromString(line, exclusion);
								break; // this is to ensure that a line will only match against one regular expression. if the user wants to make more than one exclusion per row type he/she should use multiple "<exclusion>"-nodes for that "<rowType>" in the config.
							}
						}

						cleanedDataWriter.WriteLine(strippedLine);
					}
					while (!foundFileReader.EndOfStream);
				}
			}
			cleanedDataWriter.Flush();
			cleanedData.Flush();
			cleanedData.Seek(0, SeekOrigin.Begin);
			return cleanedData;
		}

		private static string StripExclusionFromString(string line, Exclusion exclusion)
		{
			StringBuilder outputLine = new StringBuilder(line.Length);
			for (int i = 0; i < line.Length; i++)
			{
				if (i >= exclusion.ExclusionBitMap.Length || !exclusion.ExclusionBitMap[i])
				{
					outputLine.Append(line.Substring(i, 1));
				}
			}
			return outputLine.ToString();
		}
	}
}