using System.IO;
using System.Xml;
using BizUnit;

namespace BizUnitCompareTestClient.FlatfileCompare
{
	internal class FlatfileCompareTest
	{
		internal void Test()
		{
			XmlDocument config = new XmlDocument();
			Context context = new Context();

			config.Load(@"FlatfileCompare\testConfig.xml");
			XmlNode configPart = config.SelectSingleNode("/TestStep");

			context.Add("searchDirectory", Directory.GetCurrentDirectory() + @"\FlatfileCompare");
			BizUnitCompare.FlatfileCompare.FlatfileCompare comparer = new BizUnitCompare.FlatfileCompare.FlatfileCompare();

			comparer.Execute(configPart, context);
		}
	}
}