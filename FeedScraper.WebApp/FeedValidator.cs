using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace FeedScraper.WebApp
{
    public class FeedValidator
    {
        private readonly List<ExternalFeed> _approvedFeedList;
        public string FeedProcessingMode { get; set; }
        public static string FeedResource { get; set; }
        public static string FeedxPatch { get; set; }
        public static string FeedParams { get; set; }
        public string FeedRequestUrl { get; set; }
        private string ErrorPageUrl { get; set; }

        /*
         * 
         * Initialize list of addreses supported by the feed scraper
         * 
         */
        public FeedValidator(HttpRequest clientHttpRequest)
        {
            //
            // intialize valid feed options list
            //
            _approvedFeedList = new List<ExternalFeed>
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
                FeedParams = FeedParams.Substring(FeedParams.IndexOf("param=", StringComparison.Ordinal) + 6,
                    FeedParams.Length - FeedParams.IndexOf("param=", StringComparison.Ordinal) - 6);
            }
            //
            // get xPath from request
            //
            FeedxPatch = clientHttpRequest?["xpath"];
            //
            // get feedProcessing mode from request
            //
            FeedProcessingMode = clientHttpRequest?["mode"];
            //
            // get feed source
            //
            FeedResource = GetUrlBySourceName(clientHttpRequest?["source"]);

            FeedRequestUrl = GetFeedRequestUrl();
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
                ResourceName = resourceNameForFeed;
                WebAddress = addressForFeed;
            }
        }

        /*
         * 
         * Search valid sources list and match with requested by user
         * 
         */
        private string GetUrlBySourceName(string sourceName)
        {
            
            if (sourceName.Length > 0)
            { 
                foreach (var source in _approvedFeedList.Where(source => Equals(source.ResourceName.ToUpper(), sourceName.ToUpper())))
                {
                    return source.WebAddress;
                }
            }
            else
            {
                return "";
            }
            return "";
        }

        /*
         * 
         *  function designed to verify if request is not a security thread.
         * 
         */
        public static string GetFeedRequestUrl()
        {
            if (FeedResource.Length > 0)
            {
                var url = FeedResource + FeedParams;

                    Uri uriResult;
                var result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                             && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                return result ? url : "";
            }
            else
            {
                return "";
            }
            
        }
    }
}