using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeedScraper.WebApp
{
    public class FeedValidator
    {
        private readonly List<ExternalFeed> _approvedFeedList;
        public string FeedProcessingMode { get; set; }
        public static string FeedResource { get; set; }
        public static string FeedxPath { get; set; }
        public static string FeedParams { get; set; }
        public string FeedRequestUrl { get; set; }

        public string ErrorPageUrl { get; set; }

        /*
         * 
         * Initialize list of address's supported by the feed scraper
         * 
         */
        public FeedValidator(HttpRequest clientHttpRequest)
        {
            //
            // initialize valid feed options list
            //
            this._approvedFeedList = new List<ExternalFeed>
            {
                new ExternalFeed("FRB", "https://www.federalreserve.gov/"),
                new ExternalFeed("MetroAlerts", "http://www.metroalerts.info/"),
                new ExternalFeed("Reuters", "http://feeds.reuters.com/"),
                new ExternalFeed("AP", "http://hosted.ap.org/")
            };

            //
            // get parameters from request
            //
            if (clientHttpRequest.Url.Query.IndexOf("params=", StringComparison.Ordinal) <= 0)
            {
                FeedParams = "";
            }
            else
            {
                FeedParams = clientHttpRequest.Url.Query.Replace("&", "&amp;");
                FeedParams = FeedParams.Substring(FeedParams.IndexOf("params=", StringComparison.Ordinal) + 7,
                    FeedParams.Length - FeedParams.IndexOf("params=", StringComparison.Ordinal) - 7);
            }

            this.ErrorPageUrl = "http://" + clientHttpRequest.Url.Host + ":" + clientHttpRequest.Url.Port + "/error.xml";

            //
            // get xPath from request
            //
            FeedxPath = clientHttpRequest["xpath"];
            //
            // get feedProcessing mode from request
            //
            this.FeedProcessingMode = clientHttpRequest["mode"];
            //
            // get feed source
            //
            FeedResource = this.GetUrlBySourceName(clientHttpRequest["source"]);

            //
            // create a feed request url from source + param data
            //
            this.FeedRequestUrl = this.GetFeedRequestUrl();

        }
        /*
         * 
         * List of resources that are available to be parsed
         * 
         */
        internal struct ExternalFeed
        {
            public string ResourceName { get; set; }
            public string WebAddress { get; set; }

            public ExternalFeed(string resourceNameForFeed, string addressForFeed)
            {
                this.ResourceName = resourceNameForFeed;
                this.WebAddress = addressForFeed;
            }
        }

        /*
         * 
         * Search valid sources list and match with requested by user
         * 
         */
        private string GetUrlBySourceName(string sourceName)
        {
            
            if (sourceName?.Length > 0)
            { 
                foreach (var source in this._approvedFeedList.Where(source => Equals(source.ResourceName.ToUpper(), sourceName.ToUpper())))
                {
                    return source.WebAddress;
                }
            }
            else
            {
                return this.ErrorPageUrl;
            }
            return this.ErrorPageUrl;
        }

        /*
         * 
         *  function designed to verify if request is not a security thread.
         * 
         */
        public string GetFeedRequestUrl()
        {
            if (FeedResource.Length > 0)
            {
                var url = $"{FeedResource}{FeedParams}";

                    Uri uriResult;
                var result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                             && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                return result ? url : this.ErrorPageUrl;
            }
            else
            {
                return this.ErrorPageUrl;
            }
            
        }
    }
}