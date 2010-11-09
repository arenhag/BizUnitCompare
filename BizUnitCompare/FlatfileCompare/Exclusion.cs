using System.Collections.Generic;

namespace BizUnitCompare.FlatfileCompare
{
	internal class Exclusion
	{
		internal Exclusion()
		{
			ExclusionPositions = new List<ExclusionPositions>();
		}

		private string _rowIdentifyingRegularExpression;
		private List<ExclusionPositions> _exclusionPositions;

		internal string RowIdentifyingRegularExpression
		{
			get { return _rowIdentifyingRegularExpression; }
			set { _rowIdentifyingRegularExpression = value; }
		}

		internal List<ExclusionPositions> ExclusionPositions
		{
			get { return _exclusionPositions; }
			set { _exclusionPositions = value; }
		}

		internal bool[] ExclusionBitMap
		{
			get
			{
				int bitmapLength = 0;
				foreach (ExclusionPositions exclusionPosition in ExclusionPositions)
				{
					if (exclusionPosition.EndPosition > bitmapLength)
					{
						bitmapLength = exclusionPosition.EndPosition;
					}
				}

				bool[] bitmap = new bool[bitmapLength];

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
}