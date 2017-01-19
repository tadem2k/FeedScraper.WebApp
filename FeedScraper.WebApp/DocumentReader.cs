using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Xml;
using System.Xml.Schema;

namespace FeedScraper.WebApp
{

    public class DocumentReader
    {
        public XmlDocument XmlDoc = new XmlDocument();

        public DocumentReader(string feedUrl)
        {
            //
            // http://localhost:50202/output.aspx?source=AP&mode=RSS&params=.rss
            //
            var rssReaderSettings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Ignore,
                XmlResolver = null
            };

            try
            {
                XmlReader xmlDocReader;
                using (xmlDocReader = XmlReader.Create(feedUrl, rssReaderSettings))
                {
                    try
                    {
                        XmlDoc.Load(xmlDocReader);
                        XmlDoc.LoadXml(XmlScraperFunctions.RemoveAllNamespaces(XmlDoc.OuterXml));
                    }
                    catch (Exception ex)
                    {
                        if (ex is XmlSchemaValidationException || ex is XmlException)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }
                }
            }
            catch (WebException we)
            {
                Debug.WriteLine($"WebException: {we.Message}");
            }
        }

        /*
         * Reads typical RSS feeds and grabs the news into the list
         */

        public List<RssNewsEntry> RssParser(string xpath)
        {
            var newsRssList = new List<RssNewsEntry>();

            if (XmlDoc.DocumentElement == null) return newsRssList;

            var newsList = XmlDoc.DocumentElement.SelectNodes(xpath);

            if (newsList == null) return newsRssList;

            foreach (XmlNode node in newsList)
            {
                try
                {
                    newsRssList.Add(new RssNewsEntry(node["title"]?.InnerText, XmlScraperFunctions.StripTags(node["description"]?.InnerText),
                        node["link"]?.InnerText, node["pubDate"]?.InnerText));
                }
                catch (NullReferenceException em)
                {
                    Debug.WriteLine($"{em.Message}");
                    Debug.WriteLine(node.OuterXml);
                }
            }
            return newsRssList;
        }
    }
}