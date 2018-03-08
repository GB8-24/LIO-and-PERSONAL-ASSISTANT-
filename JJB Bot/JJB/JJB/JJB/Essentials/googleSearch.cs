using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace JJB.Essentials
{
    public class googleSearch
    {
        public async Task<googleSearchResponse> search(string q)
        {
            var client = new HttpClient();

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            // Request parameters
            queryString["q"] = q;
            queryString["key"] = ConfigurationManager.AppSettings["googleSearch"];
            queryString["cx"] = "006653949403192262233:zzrcivmuny4";
            //queryString["safesearch"] = "Moderate";
            var uri = "https://www.googleapis.com/customsearch/v1?" + queryString;

            var response = await client.GetAsync(uri);
            string r = await response.Content.ReadAsStringAsync();
            googleSearchResponse g = JsonConvert.DeserializeObject<googleSearchResponse>(r);

            return g;
        }

    }
}