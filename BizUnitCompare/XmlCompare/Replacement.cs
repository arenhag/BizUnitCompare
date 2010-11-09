namespace BizUnitCompare.XmlCompare
{
	internal class Replacement
	{
		private string _newString;
		private string _oldString;

		internal string OldString
		{
			get { return _oldString; }
			set { _oldString = value; }
		}

		internal string NewString
		{
			get { return _newString; }
			set { _newString = value; }
		}
	}
}