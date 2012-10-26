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

using System.Collections.Generic;

namespace BizUnitCompare.FlatfileCompare
{
    internal class Exclusion
    {
        internal Exclusion()
        {
            ExclusionPositions = new List<ExclusionPositions>();
        }

        internal string RowIdentifyingRegularExpression { get; set; }

        internal List<ExclusionPositions> ExclusionPositions { get; set; }

        internal bool[] ExclusionBitMap
        {
            get { return CalculateBitmap(); }
        }

        private bool[] CalculateBitmap()
        {
            int bitmapLength = 0;
            foreach (ExclusionPositions exclusionPosition in ExclusionPositions)
            {
                if (exclusionPosition.EndPosition > bitmapLength)
                {
                    bitmapLength = exclusionPosition.EndPosition;
                }
            }

            var bitmap = new bool[bitmapLength];

            foreach (ExclusionPositions exclusionPositions in ExclusionPositions)
            {
                for (int i = exclusionPositions.StartPosition - 1; i < exclusionPositions.EndPosition; i++)
                {
                    bitmap[i] = true;
                }
            }
            return bitmap;
        }
    }
}