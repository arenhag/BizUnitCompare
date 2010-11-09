using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace BizUnitCompare.XmlCompare
{
	internal static class XmlDocumentCleaner
	{
		internal static string ReplaceInDocument(string documentPath, IEnumerable<Replacement> replacements)
		{
			string newString;
			using (FileStream stream = new FileStream(documentPath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				newString = new StreamReader(stream).ReadToEnd();

				foreach (Replacement replacement in replacements)
				{
					newString = newString.Replace(replacement.OldString, replacement.NewString);
				}
			}
			return newString;
		}

		internal static void RemoveAttribute(ref XmlDocument document, Attribute attribute)
		{
			XmlNodeList parentNodes = document.SelectNodes(attribute.ParentElementXPath);
			if (parentNodes != null)
			{
				foreach (XmlNode parentNode in parentNodes)
				{
					parentNode.Attributes.RemoveNamedItem(attribute.Name);
				}
			}
		}

		internal static void RemoveElements(ref XmlDocument document, string xpathToElement)
		{
			XmlNodeList nodesToRemove = document.SelectNodes(xpathToElement);
			if (nodesToRemove != null)
			{
				foreach (XmlNode node in nodesToRemove)
				{
					node.ParentNode.RemoveChild(node);
				}
			}
		}
	}
}