using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Schema;

namespace FeedScraper.WebApp
{
    public class DocumentReader
    {
        public XmlDocument XmlDoc = new XmlDocument();
        // Set the validation settings.

        public string UrlAddress { get; set; }
        public string ParsingInstructions { get; set; }

        public DocumentReader(HttpRequest url)
        {
            var rssReaderSettings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Ignore,
                XmlResolver = null
            };


            GetParsingInstructionsFromtRequest(url);

            if (GetUrlFromRequest(url))
            {
                try
                {
                    XmlReader xmlDocReader;
                    using (xmlDocReader = XmlReader.Create(UrlAddress, rssReaderSettings))
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
                                System.Diagnostics.Debug.WriteLine(ex.Message);
                            }
                        }
                    }
                }
                catch (WebException we)
                {
                    System.Diagnostics.Debug.WriteLine($"WebException: {we.Message}");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No URL Entered, check your address");
            }
        }

        /*
         * Reads Mode parameters valid values are: FRB_RSS/RSS
         */
        private void GetParsingInstructionsFromtRequest(HttpRequest clintRequest)
        {
            ParsingInstructions = clintRequest.Url.Query.IndexOf("mode=", StringComparison.Ordinal) > 0 ? clintRequest["mode"] : "none";
        }

        /*
         * retrives Link to rss/xml feed from the GET command
         */
        public bool GetUrlFromRequest(HttpRequest clintRequest)
        {
            if (clintRequest.Url.Query.IndexOf("url=", StringComparison.Ordinal) > 0)
            {
                UrlAddress = clintRequest.Url.Query.Replace("&", "&amp;");
                UrlAddress = UrlAddress.Substring(UrlAddress.IndexOf("url=", StringComparison.Ordinal) + 4, UrlAddress.Length - UrlAddress.IndexOf("url=", StringComparison.Ordinal) - 4);

                Uri uriResult;
                bool result = Uri.TryCreate(UrlAddress, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                return result;
            }
            else
            {
                UrlAddress = $"http://{clintRequest.Url.Host}:{clintRequest.Url.Port.ToString()}/error.aspx";
                return false;
            }
        }
        /*
         * String HDML and XML tags
         */
        public string StripTags(string text)
        {
            return Regex.Replace(text, @"<(.|\n)*?>", string.Empty);
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
                    newsRssList.Add(new RssNewsEntry(node["title"]?.InnerText, StripTags(node["description"]?.InnerText), node["link"]?.InnerText));
                }
                catch (NullReferenceException em)
                {
                    System.Diagnostics.Debug.WriteLine($"{em.Message}");
                    System.Diagnostics.Debug.WriteLine(node.OuterXml);
                }
            }
            return newsRssList;
        }

    }
}