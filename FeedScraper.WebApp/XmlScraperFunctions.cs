using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace FeedScraper.WebApp
{
    public class XmlScraperFunctions
    {
        /*
         * 
         * Loading Namespaces from the file
         * 
         */
        public static XmlNamespaceManager GetNameSpaceManager(XmlDocument xDoc)
        {
            var nsm = new XmlNamespaceManager(xDoc.NameTable);
            var rootNode = xDoc.CreateNavigator();
            rootNode.MoveToFollowing(XPathNodeType.Element);
            var nameSpaces = rootNode.GetNamespacesInScope(XmlNamespaceScope.All);

            if (nameSpaces == null) return nsm;

            foreach (KeyValuePair<string, string> kvp in nameSpaces)
            {
                nsm.AddNamespace(kvp.Key.Length == 0 ? "ns" : kvp.Key, kvp.Value);
            }

            return nsm;
        }

        public static string RemoveAllNamespaces(string xmlDocument)
        {
            var xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));

            return xmlDocumentWithoutNs.ToString();
        }

        private static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                var xElement = new XElement(xmlDocument.Name.LocalName) { Value = xmlDocument.Value };

                foreach (var attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }
    }
}