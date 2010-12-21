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

using System.Collections.Generic;
using System.IO;
using System.Text;
using BizUnitCompare.FlatfileCompare;
using NUnit.Framework;

namespace BizUnitCompareTests.FlatfileCompare
{
	[TestFixture]
	public class FlatfileCleanerTest
	{
		#region Setup/Teardown

		[SetUp]
		[TearDown]
		public void SetUpAndTearDown()
		{
			File.Delete(_documentPath);
		}

		#endregion

		private readonly string _documentPath = Directory.GetCurrentDirectory() + @"\test.test";

		[Test]
		public void RemoveExclusions()
		{
			StringBuilder inputData = new StringBuilder();
			inputData.AppendLine("this is the input");
			inputData.AppendLine("should not be touched");

			StringBuilder expectedData = new StringBuilder();
			expectedData.AppendLine("thisistheinput");
			expectedData.AppendLine("should not be touched");

			StreamWriter writer = File.CreateText(_documentPath);
			writer.Write(inputData.ToString());
			writer.Dispose();

			List<Exclusion> exclusions = new List<Exclusion>();
			Exclusion exclusion = new Exclusion();
			exclusion.RowIdentifyingRegularExpression = "this";

			ExclusionPositions position = new ExclusionPositions();
			position.StartPosition = 5;
			position.EndPosition = 5;
			exclusion.ExclusionPositions.Add(position);

			position = new ExclusionPositions();
			position.StartPosition = 8;
			position.EndPosition = 8;
			exclusion.ExclusionPositions.Add(position);

			position = new ExclusionPositions();
			position.StartPosition = 12;
			position.EndPosition = 12;
			exclusion.ExclusionPositions.Add(position);

			exclusions.Add(exclusion);

			MemoryStream returnStream = FlatfileCleaner.RemoveExclusions(_documentPath, exclusions);
			StreamReader returnReader = new StreamReader(returnStream);
			string returnString = returnReader.ReadToEnd();
			returnStream.Dispose();

			Assert.AreEqual(expectedData.ToString(), returnString);
		}
	}
}