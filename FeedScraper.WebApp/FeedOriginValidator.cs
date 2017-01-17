using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeedScraper.WebApp
{
    public class FeedOriginValidator
    {
        private List<ExternalFeed> approvedFeedList;
        /*
         * 
         * Initialize list of addreses supported by the feed scraper
         * 
         */
        public FeedOriginValidator()
        {
            approvedFeedList = new List<ExternalFeed>
            {
                new ExternalFeed("FRB", "www.federalreserve.gov"),
                new ExternalFeed("MetroAlerts", "www.metroalerts.info"),
                new ExternalFeed("Reuters", "feeds.reuters.com"),
                new ExternalFeed("AP", "hosted.ap.org")
            };
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

        public bool IsValidFeedOrigin(HttpRequest userRequest)
        {
            //userRequest.Url.Host
            foreach (var feed in approvedFeedList)
            {
                
            }
            return true;
        }
    }
}