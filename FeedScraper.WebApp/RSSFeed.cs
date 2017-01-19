using System.Collections.Generic;
using System.Xml;

namespace FeedScraper.WebApp
{
    public class RssNewsEntry
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string PubDate { get; set; }

        public RssNewsEntry(string title, string description, string link, string pubDate)
        {
            Title = title;
            Description = description;
            Link = link;
            PubDate = pubDate;
        }
    }

    public class RssGenerator
    {
        public static XmlDocument GetRssFeedFromList(List<RssNewsEntry> rssList)
        {
            var rssXml = new XmlDocument();

            var docNode = rssXml.CreateXmlDeclaration("1.0", "UTF-8", null);
            rssXml.AppendChild(docNode);

            var rssNode = rssXml.CreateElement("rss");
            rssXml.AppendChild(rssNode);

            var rssNodeVersionAttr = rssXml.CreateAttribute("version");
            rssNodeVersionAttr.Value = "2.0";
            rssNode.Attributes?.Append(rssNodeVersionAttr);

            var rssNodeChannel = rssXml.CreateElement("channel");
            rssNode.AppendChild(rssNodeChannel);

            var rssNodeTitle = rssXml.CreateElement("title");
            var rssNodeLink = rssXml.CreateElement("link");
            var rssNodeDescription = rssXml.CreateElement("description");
            var rssNodePubDateNode = rssXml.CreateElement("pubDate");

            rssNodeChannel.AppendChild(rssNodeTitle);
            rssNodeChannel.AppendChild(rssNodeLink);
            rssNodeChannel.AppendChild(rssNodeDescription);
            rssNodeChannel.AppendChild(rssNodePubDateNode);

            foreach (var entry in rssList)
            {
                var newsItemNode = rssXml.CreateElement("item");
                rssNodeChannel.AppendChild(newsItemNode);

                var newsItemTitleNode = rssXml.CreateElement("title");
                var newsItemDescriptionNode = rssXml.CreateElement("description");
                var newsItemLinkNode = rssXml.CreateElement("link");
                var newsItemPubDateNode = rssXml.CreateElement("pubDate");

                newsItemTitleNode.InnerText = entry.Title;
                newsItemLinkNode.InnerText = entry.Link;
                newsItemDescriptionNode.InnerText = entry.Description;
                newsItemPubDateNode.InnerText = entry.PubDate;

                newsItemNode.AppendChild(newsItemTitleNode);
                newsItemNode.AppendChild(newsItemLinkNode);
                newsItemNode.AppendChild(newsItemDescriptionNode);
                newsItemNode.AppendChild(newsItemPubDateNode);

            }

            return rssXml;
        }
    }
}