namespace FeedScraper.WebApp
{
    using System;
    using System.Web.UI;

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