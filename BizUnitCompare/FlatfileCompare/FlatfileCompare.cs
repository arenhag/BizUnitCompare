#region License

// Copyright (c) 2012, Fredrik Arenhag
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
using System.Xml;
using BizUnit;

namespace BizUnitCompare.FlatfileCompare
{
    public class FlatfileCompare : ITestStep
    {
        #region ITestStep Members

        public void Execute(XmlNode testConfig, Context context)
        {
            var configuration = new BizUnitFlatfileCompareConfiguration(testConfig, context);

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
                        } while (!(cleanedFoundData.Position >= cleanedFoundData.Length));
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