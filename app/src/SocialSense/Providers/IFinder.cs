using System;
using SocialSense.Shared;
using System.Collections.Generic;

namespace SocialSense.Providers
{
	public interface IFinder
    {
		void Search(Query query, Action<IList<ResultItem>> callback);
    }
}