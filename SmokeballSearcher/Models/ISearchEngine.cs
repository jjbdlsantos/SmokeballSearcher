using SmokeballSearcher.Helpers;

namespace SmokeballSearcher.Models
{
    public interface ISearchEngine
    {
        public List<SearchResultLW> GetSearchEngineResultsByKeyword(string keyword);
        public List<SearchResultLW> ParseResults(string rawJson);

        public List<int> RunSearch(string keyword, string url)
        {
            var results = GetSearchEngineResultsByKeyword(keyword);
            return FindURLPositions(results, url);
        }

        public List<int> FindURLPositions(List<SearchResultLW> results, string url)
        {
            return results.Select((result, index) => new { result, index })
                            .Where(x => x.result.DisplayLink == url)
                            .Select(x => x.index + 1)
                            .ToList();
        }
    }
}
