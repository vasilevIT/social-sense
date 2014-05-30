using System;
using SocialSense.Network;
using SocialSense.Shared;
using System.Collections.Generic;
using System.Net;

namespace SocialSense.Providers.Google
{
    public class GoogleBlogFinder : BaseGoogleFinder
    {
        private readonly RequestManager requester;
        private readonly GoogleUrlBuilder urlBuilder;
        private readonly GoogleParser parser;

        public GoogleBlogFinder()
        {
            this.requester = new RequestManager ();
            this.urlBuilder = new GoogleUrlBuilder (GoogleSource.News);
            this.parser = new GoogleParser ();
        }

        public override void Search (Query query, Action<IList<ResultItem>> callback)
        {
            HttpRequest request = PrepareRequest (this.urlBuilder.WithQuery(query));
            this.requester.Execute (request, (response) => {
                if (response.StatusCode == HttpStatusCode.OK) {
                    SearchResult result = this.parser.Parse (response.Content);
                    int totalResults = result.Items.Count;
                    callback(result.Items);

                    if (result.HasNextPage && totalResults < query.MinResults)
                    {
                        query.Page++;
                        query.MinResults -= totalResults;
                        Search(query, callback);
                    }
                }
            });
        }
    }
}

