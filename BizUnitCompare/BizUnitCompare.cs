using System;
using System.IO;
using System.Threading;
using BizUnit;

namespace BizUnitCompare
{
	public abstract class BizUnitCompare
	{
		internal static string GetFoundFilePath(Context context, BizUnitCompareConfiguration configuration)
		{
			context.LogInfo("Waiting for file (in: {0}) for {1} seconds.", configuration.SearchDirectory, configuration.Timeout/1000);
			DateTime endTime = DateTime.Now.AddMilliseconds(configuration.Timeout);

			bool fileFound = false;
			string foundFilePath = string.Empty;

			do
			{
				string[] files = Directory.GetFiles(configuration.SearchDirectory, configuration.Filter, SearchOption.TopDirectoryOnly);
				if (files.Length > 0)
				{
					try
					{
						string fileData;
						using (FileStream testFileStream = File.Open(files[0], FileMode.Open, FileAccess.Read, FileShare.Read))
						{
							using (StreamReader blaha = new StreamReader(testFileStream))
							{
								fileData = blaha.ReadToEnd();
							}

							// it might be tempting to try to load the found file into an XmlDocument here,
							// but the user might have configured replacements which will turn an invalid document into a valid XML.
						}
						context.LogInfo("File found: {0}", files[0]);
						context.LogInfo(fileData);
						foundFilePath = files[0];
						fileFound = true;
					}
					catch (IOException)
					{
						context.LogWarning("Error while opening found file ({0}) for reading. Will retry continously until timeout expires.", files[0]);
					}
				}

				Thread.Sleep(100);
			}
			while (DateTime.Now < endTime && !fileFound);

			if (!fileFound)
			{
				throw new ApplicationException(string.Format(("No files found in {0} within the given timeout ({1})"), configuration.SearchDirectory, configuration.Timeout));
			}
			return foundFilePath;
		}
	}
}