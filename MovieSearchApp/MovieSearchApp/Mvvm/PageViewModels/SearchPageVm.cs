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

        public MovieCollectionModel result;
        public readonly IAlertService _alertService;
        public readonly TastediveService _tastediveService;
        public readonly OmdbService _omdbService;
        public readonly IPageServiceZero _pageService;
        public string _searchText;
        public MovieDetailsModel _detailsresult;
        public MovieModel _display;
        public int _pagecounter;
        private PickerModel _selectedFilter;
        private List<MovieModel> _searchResult;
        private string currentSearch { get; set; }
        private PickerModel currentlySelectedFilter { get; set; }

        public IList<PickerModel> FilterList { get; set; }
        public List<MovieModel> SearchResult
        {
            get => _searchResult;
            set => SetProperty(ref _searchResult, value);
        }

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

        public MovieModel Display
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

            GetRecommendationsCommand = new CommandBuilder().SetExecuteAsync(GetRecommendationsExecute).Build();
            SearchNextPageCommand = new CommandBuilder().SetExecuteAsync(GetNextPageExecute).Build();
            SearchPreviousPageCommand = new CommandBuilder().SetExecuteAsync(GetPreviousPageExecute).Build();
            SearchCommand = new CommandBuilder().SetExecuteAsync(SearchCommandExecute).Build();
            GetDetailsCommand = new CommandBuilder().SetExecuteAsync(GetMovieDetailsExecute).Build();

        }

        public async Task GetRecommendationsExecute()
        {
            RecommendationModel recommendationResult = await _tastediveService.GetRecommendations(System.Web.HttpUtility.UrlPathEncode(Display.Title));
            RecommendationResult = recommendationResult;
            await _pageService.PushPageAsync<RecommendationPage, RecommendationPageVm>((vm) => vm.Init(recommendationResult));
        }
        public async Task GetMovieDetailsExecute()
        {
            if (Display == null)
            {
                await _alertService.DisplayAlertAsync("Message", "Please select a movie before searching for more details", "Ok");
            }
            else
            {
                var movieId = Display.imdbID;
                MovieDetailsModel detailsResult = await _omdbService.GetMovieDetailsAsync(movieId);
                await _pageService.PushPageAsync<MovieDetailsPage, MovieDetailsPageVM>((vm) => vm.Init(detailsResult));
                DetailsResult = detailsResult;
            }


        }

        public async Task<int> SearchCommandExecute()
        {
            pageCounter = 1;
            List<MovieModel> searchResult = new List<MovieModel>();
            switch (SelectedFilter.Filter)
            {
                case "Search TV Shows":
                    result = await _omdbService.GetSeriesAsync(SearchText, pageCounter);
                    Result = result;
                    break;

                case "Search Movies":
                    result = await _omdbService.GetMoviesAsync(SearchText, pageCounter);
                    Result = result;
                    break;

                case "Search Game":
                    result = await _omdbService.GetGamesAsync(SearchText, pageCounter);
                    Result = result;
                    break;

                default:
                    result = await _omdbService.GetAllAsync(SearchText, pageCounter);
                    Result = result;
                    break;
            }
            if (result.Response == "False")
            {
                await _alertService.DisplayAlertAsync("Message", "No Result Found, Please Try Again", "Ok");
            }
            
            currentSearch = SearchText;
            currentlySelectedFilter = SelectedFilter;
            return pageCounter;

        }


        public async Task<int> GetNextPageExecute()
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

                        switch (SelectedFilter.Filter)
                        {
                            case "Search TV Shows":
                                result = await _omdbService.GetSeriesAsync(currentSearch, pageCounter);
                                Result = result;
                                break;

                            case "Search Movies":
                                result = await _omdbService.GetMoviesAsync(currentSearch, pageCounter);
                                Result = result;
                                break;

                            case "Search Game":
                                result = await _omdbService.GetGamesAsync(currentSearch, pageCounter);
                                Result = result;
                                break;

                            default:
                                result = await _omdbService.GetAllAsync(currentSearch, pageCounter);
                                Result = result;
                                break;
                        }

                        if (result.Response == "False")
                        {
                            await _alertService.DisplayAlertAsync("Message", "No More Results Found", "Ok");
                            pageCounter = currentCounter;
                        }
                    }
                    else
                    {
                        await _alertService.DisplayAlertAsync("Message", "Please enter a movie and try again", "Ok");
                        pageCounter = currentCounter;
                    }

                }


                Display = null;
                return pageCounter;
        }
        
        public async Task<int> GetPreviousPageExecute()
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

                    switch (SelectedFilter.Filter)
                    {
                        case "Search TV Shows":
                            result = await _omdbService.GetSeriesAsync(currentSearch, pageCounter);
                            Result = result;
                            break;

                        case "Search Movies":
                            result = await _omdbService.GetMoviesAsync(currentSearch, pageCounter);
                            Result = result;
                            break;

                        case "Search Game":
                            result = await _omdbService.GetGamesAsync(currentSearch, pageCounter);
                            Result = result;
                            break;

                        default:
                            result = await _omdbService.GetAllAsync(currentSearch, pageCounter);
                            Result = result;
                            break;
                    }

                    if (result.Response == "False")
                    {
                        await _alertService.DisplayAlertAsync("Message", "No More Results Found", "Ok");
                        pageCounter = currentCounter;
                    }
                }
            }


            Display = null;
            return pageCounter;
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

