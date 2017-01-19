namespace FeedScraper.WebApp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    ///     The error page
    /// </summary>
    public partial class error : Page
    {
        /// <summary>
        /// The page_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Response.Clear();
            this.Response.ContentType = "text/xml";
            /*
            var feedValidator = new FeedValidator(this.Request);
            var docReader = new DocumentReader(feedValidator.ErrorPageUrl);
            this.Response.Write(docReader.XmlDoc.OuterXml);
            */

            //this.Response.Write("<?xml version=\"1.0\" encoding=\"UTF - 8\"?>");
            this.Response.Write("<messages>");
            foreach (var msg in MyDebug.Messages)
            {
                this.Response.Write($"<message>{msg}</message>");
            }

            this.Response.Write("</messages>");
            MyDebug.DeleteMessages();
            this.Response.End();
        }
    }
}