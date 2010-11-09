namespace BizUnitCompare.XmlCompare
{
	internal class Attribute
	{
		private string _name;
		private string _parentElementXPath;

		internal string ParentElementXPath
		{
			get { return _parentElementXPath; }
			set { _parentElementXPath = value; }
		}

		internal string Name
		{
			get { return _name; }
			set { _name = value; }
		}
	}
}