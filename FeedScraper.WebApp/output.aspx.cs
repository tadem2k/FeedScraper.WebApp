using System;

namespace FeedScraper.WebApp
{
    public partial class output : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Response.Clear();
            this.Response.ContentType = "text/xml";

            var feedValidator = new FeedValidator(this.Request);
            var docReader = new DocumentReader(feedValidator.FeedRequestUrl);

            if (docReader.XmlDoc.OuterXml.Length > 0)
            {
                switch (feedValidator.FeedProcessingMode)
                {
                    case "FRB_RSS":
                        {
                            var rssFeedItems = docReader.RssParser("/RDF/item");
                            var rssDoc = RssGenerator.GetRssFeedFromList(rssFeedItems);

                            this.Response.Write(rssDoc.OuterXml);
                            break;
                        }
                    case "RSS":
                        {
                            var rssFeedItems = docReader.RssParser("/rss/channel/item");
                            var rssDoc = RssGenerator.GetRssFeedFromList(rssFeedItems);

                            this.Response.Write(rssDoc.OuterXml);
                            //MyDebug.WriteLine(rssDoc.OuterXml);
                            break;
                        }
                    default:
                        {
                            this.Response.Write(docReader.XmlDoc.OuterXml);
                            break;
                        }
                }
            }
            else
            {
                this.Response.Redirect("http://" + this.Request.Url.Host + ":" + this.Request.Url.Port + "/error.aspx");
                /*
                this.Response.Write($"<error>Cant render current resource</error><error>Make sure that provided resource is valid XML or RSS feed.</error><error>Recieved URL:{feedValidator.FeedRequestUrl}</error>");

                foreach (var msg in MyDebug.Messages)
                {
                    this.Response.Write($"<error>{msg}</error>");
                }
                */
                
            }


            /*
             * https://www.federalreserve.gov/feeds/press_bcreg.xml
             * http://rss.cnn.com/rss/cnn_topstories.rss
             * http://hosted.ap.org/lineups/USHEADS-rss_2.0.xml?SITE=RANDOM&SECTION=HOME
             * http://www.npr.org/rss/rss.php?id=1001
             */
            this.Response.End();
        }
    }
}