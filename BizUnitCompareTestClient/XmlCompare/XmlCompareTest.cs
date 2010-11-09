using System.IO;
using System.Xml;
using BizUnit;

namespace BizUnitCompareTestClient.XmlCompare
{
	internal class XmlCompareTest
	{
		internal void Test()
		{
			XmlDocument config = new XmlDocument();
			Context context = new Context();

			config.Load(@"XmlCompare\testConfig.xml");
			XmlNode configPart = config.SelectSingleNode("/TestStep");

			context.Add("searchDirectory", Directory.GetCurrentDirectory() + @"\XmlCompare");
			BizUnitCompare.XmlCompare.XmlCompare comparer = new BizUnitCompare.XmlCompare.XmlCompare();

			comparer.Execute(configPart, context);
		}
	}
}