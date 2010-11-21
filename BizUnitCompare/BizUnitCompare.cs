using System;
using System.IO;
using System.Threading;
using BizUnit;
using System.Globalization;

namespace BizUnitCompare
{
	public abstract class BizUnitCompare
	{
		internal static string GetFoundFilePath(Context context, BizUnitCompareConfiguration configuration)
		{
			VerifyParameters(context, configuration);

			context.LogInfo(string.Format(CultureInfo.CurrentCulture, "Waiting for file (in: {0}) for {1} seconds.", configuration.SearchDirectory, configuration.Timeout / 1000));
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
							using (StreamReader testFileStreamReader = new StreamReader(testFileStream))
							{
								fileData = testFileStreamReader.ReadToEnd();
							}

							// it might be tempting to try to load the found file into an XmlDocument here,
							// but the user might have configured replacements which will turn an invalid document into a valid XML.
						}
						context.LogInfo(string.Format(CultureInfo.CurrentCulture, "File found: {0}", files[0]));
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
				throw new FileNotFoundException(string.Format(CultureInfo.CurrentCulture, "No files found in {0} within the given timeout ({1})", configuration.SearchDirectory, configuration.Timeout));
			}
			return foundFilePath;
		}

		private static void VerifyParameters(Context context, BizUnitCompareConfiguration configuration)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException("configuration", "Parameter configuration can not be null");
			}

			if (context == null)
			{
				throw new ArgumentNullException("context", "Parameter context can not be null");
			}
		}
	}
}