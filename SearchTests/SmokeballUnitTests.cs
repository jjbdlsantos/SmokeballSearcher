using Moq;
using SmokeballSearcher.Models;
using SmokeballSearcher.Helpers;
using SmokeballSearcher.ViewModels;

namespace SearchTests
{
    public class SmokeballUnitTests
    {
        [Fact]
        public void ExecuteSearch_PopulatesSearchResults_SetsUrlPositions()
        {
            //Arrange
            var mockSearchEngine = new Mock<ISearchEngine>();
            var sr = new List<int> { 1, 2, 3 };

            mockSearchEngine.Setup(m => m.RunSearch(It.IsAny<string>(), It.IsAny<string>()))
                            .Returns(sr);
            var viewModel = new SearchViewModel(mockSearchEngine.Object);

            //Act
            viewModel.ExecuteSearch();

            //Assert
            Assert.Equal(sr.Count, viewModel.SearchResults.Count);
            Assert.NotEqual("0", viewModel.UrlPositions);
        }

        [Fact]
        public void ExecuteSearch_SetsUrlPositionsToStringZero()
        {
            //Arrange
            var mockSearchEngine = new Mock<ISearchEngine>();
            var sr = new List<int> { };

            mockSearchEngine.Setup(m => m.RunSearch(It.IsAny<string>(), It.IsAny<string>()))
                            .Returns(sr);
            var viewModel = new SearchViewModel(mockSearchEngine.Object);

            //Act
            viewModel.ExecuteSearch();

            //Assert
            Assert.Equal(sr.Count, viewModel.SearchResults.Count);
            Assert.Equal("0", viewModel.UrlPositions);
        }

        [Fact]
        public void ExecuteSearch_SetsUrlPositionsToErrorMessage()
        {
            //Arrange
            var mockSearchEngine = new Mock<ISearchEngine>();
            var sr = new List<int> { -1 };
            var errorMessage = "An error occured while querying the search engine.";
            mockSearchEngine.Setup(m => m.RunSearch(It.IsAny<string>(), It.IsAny<string>()))
                            .Returns(sr);
            var viewModel = new SearchViewModel(mockSearchEngine.Object);

            //Act
            viewModel.ExecuteSearch();

            //Assert
            Assert.Equal(sr.Count, viewModel.SearchResults.Count);
            Assert.Equal(errorMessage, viewModel.UrlPositions);
        }

        [Fact]
        public void FindURLPositions_ReturnsSingleMatch()
        {
            //arrange
            ISearchEngine se = new GoogleSearchEngine();
            var searchResults = new List<SearchResultLW>();
            var url = "www.r1.com";
            var r1 = new SearchResultLW
            {
                Title = "r1",
                Link = "www.r1.com/about",
                DisplayLink = "www.r1.com"
            };
            var r2 = new SearchResultLW
            {
                Title = "R2",
                Link = "www.r2.com/about",
                DisplayLink = "www.r2.com"
            };
            searchResults.Add(r1); 
            searchResults.Add(r2);

            //act
            var result = se.FindURLPositions(searchResults, url);

            //Assert
            Assert.NotEmpty(result);
            Assert.Single(result);
            Assert.Equal(1, result[0]);
        }

        [Fact]
        public void FindURLPositions_ReturnsMultipleMatches()
        {
            //arrange
            ISearchEngine se = new GoogleSearchEngine();
            var searchResults = new List<SearchResultLW>();
            var url = "www.test.com";
            var r1 = new SearchResultLW
            {
                Title = "Test",
                Link = "www.test.com",
                DisplayLink = "www.test.com"
            };
            var r2 = new SearchResultLW
            {
                Title = "Test - About",
                Link = "www.test.com/about",
                DisplayLink = "www.test.com"
            };
            searchResults.Add(r1);
            searchResults.Add(r2);
            var expectedResult = new List<int> { 1, 2 };

            //act
            var result = se.FindURLPositions(searchResults, url);

            //Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void FindURLPositions_ReturnsNoMatches()
        {
            //arrange
            ISearchEngine se = new GoogleSearchEngine();
            var searchResults = new List<SearchResultLW>();
            var url = "www.test.com";
            var r1 = new SearchResultLW
            {
                Title = "r1",
                Link = "www.r1.com/about",
                DisplayLink = "www.r1.com"
            };
            var r2 = new SearchResultLW
            {
                Title = "R2",
                Link = "www.r2.com/about",
                DisplayLink = "www.r2.com"
            };
            searchResults.Add(r1);
            searchResults.Add(r2);
            var expectedResult = new List<int> { };

            //act
            var result = se.FindURLPositions(searchResults, url);

            //Assert
            Assert.Empty(result);
            Assert.Equal(expectedResult, result);
        }
    }
}