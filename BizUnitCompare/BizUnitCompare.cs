#region License

// Copyright (c) 2010, Fredrik Arenhag
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// *	Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// *	Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// *	Neither the name of FREDRIK ARENHAG nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
// INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
// IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY,
// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA,
// OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using System;
using System.Globalization;
using System.IO;
using System.Threading;
using BizUnit;

namespace BizUnitCompare
{
	public abstract class BizUnitCompare
	{
		internal static string GetFoundFilePath(Context context, BizUnitCompareConfiguration configuration)
		{
			VerifyParameters(context, configuration);

			context.LogInfo(string.Format(CultureInfo.CurrentCulture, "Waiting for file (in: {0}) for {1} seconds.", configuration.SearchDirectory, configuration.Timeout/1000));
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
			} while (DateTime.Now < endTime && !fileFound);

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