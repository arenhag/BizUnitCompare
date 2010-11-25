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
using System.Globalization;
using System.Xml;
using BizUnit;

namespace BizUnitCompare.FlatfileCompare
{
	internal sealed class BizUnitFlatfileCompareConfiguration : BizUnitCompareConfiguration
	{
		# region Backing property fields

		private List<Exclusion> _exclusions;

		# endregion

		# region Property Accessors

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
					if (exclusionNode.Attributes != null)
					{
						exclusion.RowIdentifyingRegularExpression = exclusionNode.Attributes["identifier"].Value;

						foreach (XmlNode positionNode in exclusionNode.ChildNodes)
						{
							ExclusionPositions positions = new ExclusionPositions();
							if (positionNode.Attributes != null)
							{
								positions.StartPosition = int.Parse(positionNode.Attributes["startPosition"].Value, CultureInfo.InvariantCulture);
								positions.EndPosition = int.Parse(positionNode.Attributes["endPosition"].Value, CultureInfo.InvariantCulture);
							}
							exclusion.ExclusionPositions.Add(positions);
						}
					}
					exclusions.Add(exclusion);
				}
			}
			return exclusions;
		}
	}
}