﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JJB.Essentials
{
    public class googleSearchResponse
    {
        public string kind { get; set; }
        public Url url { get; set; }
        public Queries queries { get; set; }
        public Context context { get; set; }
        public Searchinformation searchInformation { get; set; }
        public Item[] items { get; set; }


        public class Url
        {
            public string type { get; set; }
            public string template { get; set; }
        }

        public class Queries
        {
            public Request[] request { get; set; }
            public Nextpage[] nextPage { get; set; }
        }

        public class Request
        {
            public string title { get; set; }
            public string totalResults { get; set; }
            public string searchTerms { get; set; }
            public int count { get; set; }
            public int startIndex { get; set; }
            public string inputEncoding { get; set; }
            public string outputEncoding { get; set; }
            public string safe { get; set; }
            public string cx { get; set; }
        }

        public class Nextpage
        {
            public string title { get; set; }
            public string totalResults { get; set; }
            public string searchTerms { get; set; }
            public int count { get; set; }
            public int startIndex { get; set; }
            public string inputEncoding { get; set; }
            public string outputEncoding { get; set; }
            public string safe { get; set; }
            public string cx { get; set; }
        }

        public class Context
        {
            public string title { get; set; }
        }

        public class Searchinformation
        {
            public float searchTime { get; set; }
            public string formattedSearchTime { get; set; }
            public string totalResults { get; set; }
            public string formattedTotalResults { get; set; }
        }

        public class Item
        {
            public string kind { get; set; }
            public string title { get; set; }
            public string htmlTitle { get; set; }
            public string link { get; set; }
            public string displayLink { get; set; }
            public string snippet { get; set; }
            public string htmlSnippet { get; set; }
            public string cacheId { get; set; }
            public string formattedUrl { get; set; }
            public string htmlFormattedUrl { get; set; }
            public Pagemap pagemap { get; set; }
        }

        public class Pagemap
        {
            public Cse_Thumbnail[] cse_thumbnail { get; set; }
            public Metatag[] metatags { get; set; }
            public Cse_Image[] cse_image { get; set; }
        }

        public class Cse_Thumbnail
        {
            public string width { get; set; }
            public string height { get; set; }
            public string src { get; set; }
        }

        public class Metatag
        {
            public string fbapp_id { get; set; }
            public string ogtype { get; set; }
            public string ogurl { get; set; }
            public string ogsite_name { get; set; }
            public string ogdescription { get; set; }
        }

        public class Cse_Image
        {
            public string src { get; set; }
        }

    }
}