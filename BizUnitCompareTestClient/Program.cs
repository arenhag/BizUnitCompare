using System;
using BizUnitCompareTestClient.FlatfileCompare;
using BizUnitCompareTestClient.XmlCompare;

namespace BizUnitCompareTestClient
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine("===== LAUNCHING XML COMPARE TEST =====");
			XmlCompareTest xmlCompareTest = new XmlCompareTest();
			xmlCompareTest.Test();
			Console.WriteLine("===== XML COMPARE TEST COMPLETE =====");
			
			Console.WriteLine();

			Console.WriteLine("===== LAUNCHING FLATFILE COMPARE TEST =====");
			FlatfileCompareTest flatfileCompareTest = new FlatfileCompareTest();
			flatfileCompareTest.Test();
			Console.WriteLine("===== FLATFILE COMPARE TEST COMPLETE =====");

			Console.WriteLine();
			Console.WriteLine(Environment.NewLine + "Press enter to quit.");
			Console.ReadLine();
		}
	}
}