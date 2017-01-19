using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Xml;
using System.Xml.Schema;

namespace FeedScraper.WebApp
{
    using System.Globalization;
    using System.Linq;

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
                        this.XmlDoc.Load(xmlDocReader);
                        this.XmlDoc.LoadXml(XmlScraperFunctions.RemoveAllNamespaces(XmlDoc.OuterXml));
                    }
                    catch (Exception ex)
                    {
                        if (ex is XmlSchemaValidationException || ex is XmlException)
                        {
                            //Debug.WriteLine(ex.Message);
                            MyDebug.WriteLine(ex.Message);
                        }
                    }
                }
            }
            catch (WebException we)
            {
                //Debug.WriteLine($"WebException: {we.Message}");
                MyDebug.WriteLine($"WebException: {we.Message}");
            }
        }

        /*
         * Reads typical RSS feeds and grabs the news into the list
         */

        public List<RssNewsEntry> RssParser(string xpath)
        {
            var newsRssList = new List<RssNewsEntry>();

            if (this.XmlDoc.DocumentElement == null) return newsRssList;

            var newsList = this.XmlDoc.DocumentElement.SelectNodes(xpath);

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
                    //Debug.WriteLine($"{em.Message}");
                    //Debug.WriteLine(node.OuterXml);
                    MyDebug.WriteLine($"{em.Message}");
                    MyDebug.WriteLine(node.OuterXml);
                }
            }
            //
            // if error message than add list of errors from debuger
            //
            if (MyDebug.Messages.Count != 0)
            {
                newsRssList.AddRange(MyDebug.Messages.Select(msg => new RssNewsEntry("debug", msg, "", DateTime.Today.ToString(CultureInfo.InvariantCulture))));
            }
            return newsRssList;
        }
    }
}