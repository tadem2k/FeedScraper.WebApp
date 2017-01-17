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

            XmlNode docNode = rssXml.CreateXmlDeclaration("1.0", "UTF-8", null);
            rssXml.AppendChild(docNode);

            XmlNode rssNode = rssXml.CreateElement("rss");
            rssXml.AppendChild(rssNode);

            XmlAttribute rssNodeVersionAttr = rssXml.CreateAttribute("version");
            rssNodeVersionAttr.Value = "2.0";
            rssNode.Attributes?.Append(rssNodeVersionAttr);

            XmlNode rssNodeChannel = rssXml.CreateElement("channel");
            rssNode.AppendChild(rssNodeChannel);

            XmlNode rssNodeTitle = rssXml.CreateElement("title");
            XmlNode rssNodeLink = rssXml.CreateElement("link");
            XmlNode rssNodeDescription = rssXml.CreateElement("description");
            XmlNode rssNodePubDateNode = rssXml.CreateElement("pubDate");

            rssNodeChannel.AppendChild(rssNodeTitle);
            rssNodeChannel.AppendChild(rssNodeLink);
            rssNodeChannel.AppendChild(rssNodeDescription);
            rssNodeChannel.AppendChild(rssNodePubDateNode);

            foreach (var entry in rssList)
            {
                XmlNode newsItemNode = rssXml.CreateElement("item");
                rssNodeChannel.AppendChild(newsItemNode);

                XmlNode newsItemTitleNode = rssXml.CreateElement("title");
                XmlNode newsItemDescrNode = rssXml.CreateElement("description");
                XmlNode newsItemLinkNode = rssXml.CreateElement("link");
                XmlNode newsItemPubDateNode = rssXml.CreateElement("pubDate");

                newsItemTitleNode.InnerText = entry.Title;
                newsItemLinkNode.InnerText = entry.Link;
                newsItemDescrNode.InnerText = entry.Description;
                newsItemPubDateNode.InnerText = entry.PubDate;

                newsItemNode.AppendChild(newsItemTitleNode);
                newsItemNode.AppendChild(newsItemLinkNode);
                newsItemNode.AppendChild(newsItemDescrNode);
                newsItemNode.AppendChild(newsItemPubDateNode);

            }

            return rssXml;
        }
    }
}