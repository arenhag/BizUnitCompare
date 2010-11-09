namespace BizUnitCompare.FlatfileCompare
{
	internal class ExclusionPositions
	{
		private int _endPosition;
		private int _startPosition;

		internal int EndPosition
		{
			get { return _endPosition; }
			set { _endPosition = value; }
		}

		internal int StartPosition
		{
			get { return _startPosition; }
			set { _startPosition = value; }
		}
	}
}