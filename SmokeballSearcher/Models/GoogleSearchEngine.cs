using System.Net.Http;
using System.Text.Json;
using SmokeballSearcher.Helpers;

namespace SmokeballSearcher.Models
{
    public class GoogleSearchEngine : ISearchEngine
    {
        //api key not hidden for the sake of exercise
        private static readonly string googleApiKey = "AIzaSyCRPMD65A-QUBuwiqYErmPO444GmlVjgfw";
        private static readonly string searchEngineId = "f013bf36f4ae6423d";
        private readonly int resultsLimit = 100;
        private readonly int resultsPerPage = 10;

        public List<SearchResultLW>? GetSearchEngineResultsByKeyword(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return new List<SearchResultLW>();
            }
            var requestUri = $"https://www.googleapis.com/customsearch/v1?q={Uri.EscapeDataString(keyword)}&key={googleApiKey}&cx={searchEngineId}&highRange={resultsLimit}";
            var results = new List<SearchResultLW>();
            using (var client = new HttpClient())
            {
                try
                {
                    for (int start = 1; start < resultsLimit; start += resultsPerPage)
                    {
                        results.AddRange(ParseResults(client.GetStringAsync(requestUri).Result));
                    }
                    return results;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public List<SearchResultLW> ParseResults(string rawJson)
        {
            var jsonDocument = JsonDocument.Parse(rawJson);

            if (!jsonDocument.RootElement.TryGetProperty("items", out JsonElement items))
            {
                return new List<SearchResultLW>();
            }

            return items.EnumerateArray()
                    .Select(item => new SearchResultLW
                    {
                        Title = item.GetProperty("title").GetString() ?? string.Empty,
                        Link = item.GetProperty("link").GetString() ?? string.Empty,
                        DisplayLink = item.GetProperty("displayLink").GetString() ?? string.Empty
                    })
                    .ToList();
        }
    }
}
