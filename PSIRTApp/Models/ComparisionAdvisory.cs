using HtmlAgilityPack;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PSIRTApp.Models
{
    public class ComparisionAdvisory
    {

        public async Task<List<string>> GetTruePositive(string url)
        {
            var responsehtml   = await FetchURLData(url);

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(responsehtml);


            return new List<string>();
        }

        public async Task<List<string>> GetTrueNegative(string url)
        {
            var responsehtml = await FetchURLData(url);

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(responsehtml);

            return new List<string>();
        }


        private async Task<string> FetchURLData(string url )
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(url);

                return response;
            }
        }

    }
}
