using SmokeballSearcher.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SmokeballSearcher.ViewModels
{
    public class SearchViewModel : INotifyPropertyChanged
    {
        private readonly ISearchEngine _searchEngine;
        private string _keywords = "";
        private string _targetUrl = "";
        private string _urlPositions = "";
        private List<int> _searchResults = new List<int>();

        public SearchViewModel(ISearchEngine searchEngine)
        {
            _searchEngine = searchEngine;
            SearchCommand = new RelayCommand(ExecuteSearch);
            SearchResults = new List<int>();
        }

        public string Keywords
        {
            get => _keywords;
            set
            {
                _keywords = value;
                OnPropertyChanged();
            }
        }

        public string TargetUrl
        {
            get => _targetUrl;
            set
            {
                _targetUrl = value;
                OnPropertyChanged();
            }
        }

        public string UrlPositions
        {
            get => _urlPositions;
            set
            {
                _urlPositions = value;
                OnPropertyChanged();
            }
        }

        public List<int> SearchResults
        {
            get => _searchResults;
            set
            {
                _searchResults = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchCommand { get; }

        public void ExecuteSearch()
        {
            SearchResults.Clear();
            SearchResults = _searchEngine.RunSearch(Keywords, TargetUrl);

            if (SearchResults.Count > 0 && SearchResults.First() == -1)
            {
                UrlPositions = "An error occured while querying the search engine.";
            }
            else
            {
                UrlPositions = string.IsNullOrEmpty(string.Join(", ", SearchResults)) ? "0" : string.Join(", ", SearchResults);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
