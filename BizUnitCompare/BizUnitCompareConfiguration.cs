using System.Xml;
using BizUnit;

namespace BizUnitCompare
{
	internal abstract class BizUnitCompareConfiguration
	{
		private string _filter;
		private string _searchDirectory;
		private uint _timeout;
		private string _goalFilePath;
		private bool _deleteFile;

		internal bool DeleteFile
		{
			get { return _deleteFile; }
			set { _deleteFile = value; }
		}

		internal string GoalFilePath
		{
			get { return _goalFilePath; }
			set { _goalFilePath = value; }
		}

		internal string Filter
		{
			get { return _filter; }
			set { _filter = value; }
		}

		internal string SearchDirectory
		{
			get { return _searchDirectory; }
			set { _searchDirectory = value; }
		}

		internal uint Timeout
		{
			get { return _timeout; }
			set { _timeout = value; }
		}

		internal abstract void Load(XmlNode testConfig, Context context);
	}
}