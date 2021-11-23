using FunctionZero.CommandZero;
using FunctionZero.MvvmZero;
using MovieSearchApp.Models.OMDb;
using MovieSearchApp.Models.SearchPage;
using MovieSearchApp.Mvvm.Pages;
using MovieSearchApp.Services;
using MovieSearchApp.Services.Alert_Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using System.Linq;
using MovieSearchApp.Models.Tastedive;
using Xamarin.Forms;

namespace MovieSearchApp.Mvvm.PageViewModels
{
    class SearchPageVm : MvvmZeroBaseVm
    {
        public int pageCounter
        {
            get => _pagecounter;
            set => SetProperty(ref _pagecounter, value);
        }
        public MovieCollectionModel _result;
        public ICommand SearchNextPageCommand { get; }
        public ICommand SearchPreviousPageCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand GetDetailsCommand { get; }
        public ICommand GetRecommendationsCommand { get; }
        public ICommand GetDetailsCommandTest { get; }
       
        public MovieDetailsModel item { get; set; }



        public MovieCollectionModel result;
        public readonly IAlertService _alertService;
        public readonly TastediveService _tastediveService;
        public readonly OmdbService _omdbService;
        public readonly IPageServiceZero _pageService;
        public string _searchText;
        public MovieDetailsModel _detailsresult;
        public MovieDetailsModel _display;
        public int _pagecounter;
        private PickerModel _selectedFilter;
        private List<MovieModel> _searchResult;
        public string currentSearch { get; set; }
        private PickerModel currentlySelectedFilter { get; set; }

        public IList<PickerModel> FilterList { get; set; }
        public ObservableCollection<MovieDetailsModel> MovieDetailsList
        {
            get => _movieDetailsList;
            set => SetProperty(ref _movieDetailsList, value);
        }
        public List<MovieModel> SearchResult
        {
            get => _searchResult;
            set => SetProperty(ref _searchResult, value);
        }

        public MovieDetailsModel itemn { get; set; }
        public PickerModel SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                if (_selectedFilter != value)
                {
                    _selectedFilter = value;
                    this.OnPropertyChanged();
                };
            }
        }

        public RecommendationModel RecommendationResult;
        private ObservableCollection<MovieDetailsModel> _movieDetailsList;

        public MovieDetailsModel DetailsResult
        {
            get => _detailsresult;
            set => SetProperty(ref _detailsresult, value);
        }

        public MovieCollectionModel Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        public MovieDetailsModel Display
        {
            get => _display;
            set => SetProperty(ref _display, value);
        }

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public SearchPageVm(OmdbService omdbService, TastediveService tastediveService, IPageServiceZero pageService, IAlertService alertService)
        {
            _tastediveService = tastediveService;
            _omdbService = omdbService;
            _pageService = pageService;
            _alertService = alertService;

            FilterList = new List<PickerModel>(new[]
            {
                    new PickerModel {Filter = "Search All" },
                    new PickerModel {Filter = "Search Movies"},
                    new PickerModel {Filter = "Search TV Shows"},
                    new PickerModel {Filter = "Search Game"}

                });
            currentSearch = SearchText;
            SelectedFilter = FilterList[0];
            currentlySelectedFilter = SelectedFilter;
            //must be observable collection to access it from xaml
            MovieDetailsList = new ObservableCollection<MovieDetailsModel>();


            GetDetailsCommand = new CommandBuilder().SetExecuteAsync(GetDetailsExecute).Build();
            GetRecommendationsCommand = new CommandBuilder().SetExecuteAsync(GetRecommendationsExecute).Build();
            SearchNextPageCommand = new CommandBuilder().SetExecuteAsync(GetNextPageExecute).Build();
            SearchPreviousPageCommand = new CommandBuilder().SetExecuteAsync(GetPreviousPageExecute).Build();
            SearchCommand = new CommandBuilder().SetExecuteAsync(SearchCommandExecute).Build();


        }

        public async Task GetDetailsExecute(object item)
        {

            MovieDetailsModel item2 = item as MovieDetailsModel;
            var movieId = item2.imdbID;
            MovieDetailsModel detailsResult = await _omdbService.GetMovieDetailsWithIdAsync(movieId);
            await _pageService.PushPageAsync<MovieDetailsPage, MovieDetailsPageVM>((vm) => vm.Init(detailsResult));
            DetailsResult = detailsResult;
            
        }
       

        //function that controls getting recommendations and bringing us to this page
        public async Task GetRecommendationsExecute(object item)
        {
            MovieDetailsModel item2 = item as MovieDetailsModel;
            string type;
            switch (item2.Type)
            {
                case "series":
                    type = "show:";
                        break;
                case "movie":
                    type = "movie:";
                        break;
                case "game":
                    type = "game:";
                      break;

                default:
                    type = "";
                    break;

            }
            RecommendationModel recommendationResult = await _tastediveService.GetRecommendationsByType(System.Web.HttpUtility.UrlPathEncode(item2.Title), type );
            RecommendationResult = recommendationResult;
            await _pageService.PushPageAsync<RecommendationPage, RecommendationPageVm>(async (vm) => await vm.Init(recommendationResult));
        }


        
        //Searches for movies/etc. and displays on page 
        public async Task<int> SearchCommandExecute()
        {
            pageCounter = 1;
            List<MovieModel> searchResult = new List<MovieModel>();
            currentSearch = SearchText;
            await SearchMoviesAndUpdateDisplay();
            currentlySelectedFilter = SelectedFilter;
            return pageCounter;

        }


        //Gets next page of movies/etc.
        public async Task GetNextPageExecute()
        {
            int currentCounter;
            List<MovieModel> searchResult = new List<MovieModel>();
            currentCounter = pageCounter;
            pageCounter++;

            if (currentlySelectedFilter != SelectedFilter)
            {
                await _alertService.DisplayAlertAsync("Message", "You must start a new search with the required filter selected before changing page", "Ok");
                SelectedFilter = currentlySelectedFilter;
                pageCounter--;
            }
            else
            {
                if (SearchText != null)
                {

                  await SearchMoviesAndUpdateDisplay();

                }
               else
               {
                   await _alertService.DisplayAlertAsync("Message", "Please enter a movie and try again", "Ok");
                   pageCounter = currentCounter;
               }

            }


            Display = null;
  
        }
        
        //gets previous page of movies etc.
        public async Task GetPreviousPageExecute()
        {
            int currentCounter = pageCounter;
            pageCounter--;
            if (currentlySelectedFilter != SelectedFilter)
            {
                await _alertService.DisplayAlertAsync("Message", "You must start a new search with the required filter selected before changing page", "Ok");
                pageCounter++;
                SelectedFilter = currentlySelectedFilter;
            }
            else
            {
                if (SearchText == null && result == null)
                {
                    await _alertService.DisplayAlertAsync("Message", "Please Enter A Search", "Ok");
                    pageCounter = currentCounter;

                }
                else if (pageCounter < 1)
                {
                    await _alertService.DisplayAlertAsync("Message", "You Cannot Go Back A Page From Here", "Ok");
                    pageCounter = 1;
                }
                else
                {

                    await SearchMoviesAndUpdateDisplay();

                }
            }

            Display = null;
        }


        //function that searches movies, used in above methods, cuts repetition
        public async Task SearchMoviesAndUpdateDisplay()
        {
            MovieDetailsList.Clear();
            switch (SelectedFilter.Filter)
            {
                case "Search TV Shows":
                    result = await _omdbService.GetSeriesAsync(currentSearch, pageCounter);
                    if (result.Response == "False")
                    {
                        await _alertService.DisplayAlertAsync("Message", "No Result Found", "Ok");
                        break;
                    }
                    await CreateDisplayList(result);
                    Result = result;
                    break;

                case "Search Movies":
                    result = await _omdbService.GetMoviesAsync(currentSearch, pageCounter);
                    if (result.Response == "False")
                    {
                        await _alertService.DisplayAlertAsync("Message", "No Result Found", "Ok");
                        break;
                    }
                    await CreateDisplayList(result);
                    Result = result;
                    break;

                case "Search Game":
                    result = await _omdbService.GetGamesAsync(currentSearch, pageCounter);
                    if (result.Response == "False")
                    {
                        await _alertService.DisplayAlertAsync("Message", "No Result Found", "Ok");
                        break;
                    }
                    await CreateDisplayList(result);
                    Result = result;
                    break;

                default:
                    result = await _omdbService.GetAllAsync(currentSearch, pageCounter);
                    if (result.Response == "False")
                    {
                        await _alertService.DisplayAlertAsync("Message", "No Result Found", "Ok");
                        break;
                    }
                    await CreateDisplayList(result);
                    Result = result;                 
                    break;
            }
        }

        //This is how we create the list of objects that we will display on screen as we arent pulling it directly from the api we must make it ourselves
        public async Task CreateDisplayList(MovieCollectionModel result)
        {
            foreach (var item in result.Search)
            {
               
                MovieDetailsModel result2 = await _omdbService.GetMovieDetailsWithIdAsync(item.imdbID);
                MovieDetailsList.Add(result2);

            }

        }

        


        // protected override async void OnPropertyChanged([CallerMemberName] string propertyName = null)
        // {
        //     base.OnPropertyChanged(propertyName);
        ///     if (propertyName == nameof(Display))
        //    {
        //         if (Display != null)
        //        {
        //           await GetMovieDetailsExecute();
        //            Display = null;
        //        }
        //    }
        // }
    }
}

