using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FeedScraper.WebApp
{
    public partial class output : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Response.Clear();
            Response.ContentType = "text/xml";

            var feedValidator = new FeedValidator(Request);
            var docReader = new DocumentReader(feedValidator.FeedRequestUrl);

            if (docReader.XmlDoc.OuterXml.Length > 0)
            {
                switch (feedValidator.FeedProcessingMode)
                {
                    case "FRB_RSS":
                        {
                            var rssFeedItems = docReader.RssParser("/RDF/item");
                            var rssDoc = RssGenerator.GetRssFeedFromList(rssFeedItems);

                            Response.Write(rssDoc.OuterXml);
                            System.Diagnostics.Debug.WriteLine(rssDoc.OuterXml);
                            break;
                        }
                    case "RSS":
                        {
                            var rssFeedItems = docReader.RssParser("/rss/channel/item");
                            var rssDoc = RssGenerator.GetRssFeedFromList(rssFeedItems);

                            Response.Write(rssDoc.OuterXml);
                            System.Diagnostics.Debug.WriteLine(rssDoc.OuterXml);
                            break;
                        }
                    default:
                        {
                            Response.Write(docReader.XmlDoc.OuterXml);
                            break;
                        }
                }
            }
            else
            {
                Response.Write($"<error>Cant render current resource</error><error>Make sure that provided resource is valid XML or RSS feed.</error><error>Recieved URL:{feedValidator.FeedRequestUrl}</error>");
            }


            /*
             * https://www.federalreserve.gov/feeds/press_bcreg.xml
             * http://rss.cnn.com/rss/cnn_topstories.rss
             * http://hosted.ap.org/lineups/USHEADS-rss_2.0.xml?SITE=RANDOM&SECTION=HOME
             * http://www.npr.org/rss/rss.php?id=1001
             */
            Response.End();
        }
    }
}