using System.Xml;

namespace LineageServer.Extensions
{
	static class XmlNodeExtensions
	{
		public static bool GetBool(this XmlNode xmlNode, string attributeName)
		{
			return GetBool(xmlNode, attributeName, false);
		}
		public static bool GetBool(this XmlNode xmlNode, string attributeName, bool defaultValue)
		{
			XmlNode node = xmlNode.Attributes.GetNamedItem(attributeName);
			if (node == null)
			{
				return defaultValue;
			}
			else
			{
				if (bool.TryParse(node.Value, out bool result))
				{
					return result;
				}
				else
				{
					return defaultValue;
				}
			}
		}
		public static int GetInt(this XmlNode xmlNode, string attributeName)
		{
			return GetInt(xmlNode, attributeName, 0);
		}
		public static int GetInt(this XmlNode xmlNode, string attributeName, int defaultValue)
		{
			XmlNode node = xmlNode.Attributes.GetNamedItem(attributeName);
			if (node == null)
			{
				return defaultValue;
			}
			else
			{
				if (int.TryParse(node.Value, out int result))
				{
					return result;
				}
				else
				{
					return defaultValue;
				}
			}
		}
		public static string GetString(this XmlNode xmlNode, string attributeName)
		{
			return GetString(xmlNode, attributeName, string.Empty);
		}
		public static string GetString(this XmlNode xmlNode, string attributeName, string defaultValue)
		{
			XmlNode node = xmlNode.Attributes.GetNamedItem(attributeName);
			if (node == null)
			{
				return defaultValue;
			}
			else
			{
				return node.Value;
			}
		}
	}
}
