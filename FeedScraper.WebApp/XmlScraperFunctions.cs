// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlScraperFunctions.cs" company="">
//   
// </copyright>
// <summary>
//   Class contains collection of functions that can be used to work with RSS/XML feeds
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace FeedScraper.WebApp
{
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.XPath;

    /// <summary>
    /// The xml scraper functions.
    /// </summary>
    public class XmlScraperFunctions
    {
        /// <summary>
        ///     The get name space manager. Loading Namespaces from the XML file
        /// </summary>
        /// <param name="xDoc">
        ///     XmlDocument type object to process XML file
        /// </param>
        /// <returns>
        ///     The <see cref="XmlNamespaceManager"/> that contains namespaces for the current XML file
        /// </returns>
        public static XmlNamespaceManager GetNameSpaceManager(XmlDocument xDoc)
        {
            var nsm = new XmlNamespaceManager(xDoc.NameTable);
            var rootNode = xDoc.CreateNavigator();
            rootNode.MoveToFollowing(XPathNodeType.Element);
            var nameSpaces = rootNode.GetNamespacesInScope(XmlNamespaceScope.All);

            if (nameSpaces == null)
            {
                return nsm;
            }

            foreach (var kvp in nameSpaces)
            {
                nsm.AddNamespace(kvp.Key.Length == 0 ? "ns" : kvp.Key, kvp.Value);
            }

            return nsm;
        }

        /// <summary>
        ///     The strip XML/html tags.
        /// </summary>
        /// <param name="text">
        ///     String that needs to be striped of tags
        /// </param>
        /// <returns>
        ///     The <see cref="string"/>.
        /// </returns>
        public static string StripTags(string text)
        {
            return Regex.Replace(text, @"<(.|\n)*?>", string.Empty);
        }

        /// <summary>
        ///     The remove all namespaces function is recursive 
        /// </summary>
        /// <param name="xmlDocument">
        ///     The xml document to be processed
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string RemoveAllNamespaces(string xmlDocument)
        {
            var xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));

            return xmlDocumentWithoutNs.ToString();
        }

        /// <summary>
        ///     The remove all namespaces.
        /// </summary>
        /// <param name="xmlDocument">
        ///     The xml document to be processed.
        /// </param>
        /// <returns>
        /// The <see cref="XElement"/>.
        /// </returns>
        private static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                var xElement = new XElement(xmlDocument.Name.LocalName) { Value = xmlDocument.Value };

                foreach (var attribute in xmlDocument.Attributes())
                {
                    xElement.Add(attribute);
                }

                return xElement;
            }
            return new XElement(
                xmlDocument.Name.LocalName,
                xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }
    }

}