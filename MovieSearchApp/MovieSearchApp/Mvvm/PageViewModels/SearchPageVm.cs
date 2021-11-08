using FunctionZero.CommandZero;
using FunctionZero.MvvmZero;
using MovieSearchApp.Models.OMDb;
using MovieSearchApp.Mvvm.Pages;
using MovieSearchApp.Services;
using MovieSearchApp.Services.Alert_Service;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

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
        public ICommand SearchMoviesCommandNext { get; }
        public ICommand SearchMoviesCommandBack { get; }
        public ICommand SearchMoviesCommand { get; }
        public ICommand GetDetailsCommand { get; }

        public MovieCollectionModel result;
        public readonly IAlertService _alertService;
        public readonly OmdbService _omdbService;
        public readonly IPageServiceZero _pageService;
        public string _searchText;
        public MovieDetailsModel _detailsresult;
        public MovieModel _display;
        public int _pagecounter;

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

        public SearchPageVm(OmdbService omdbService, IPageServiceZero pageService, IAlertService alertService)
        {
            _omdbService = omdbService;
            _pageService = pageService;
            _alertService = alertService;


            SearchMoviesCommandNext = new CommandBuilder().SetExecuteAsync(GetMoviesExecuteNext).Build();
            SearchMoviesCommandBack = new CommandBuilder().SetExecuteAsync(GetMoviesExecuteBack).Build();
            SearchMoviesCommand = new CommandBuilder().SetExecuteAsync(GetMoviesExecute).Build();
            GetDetailsCommand = new CommandBuilder().SetExecuteAsync(GetMovieDetailsExecute).Build();

        }

        public async Task GetMovieDetailsExecute()
        {
            string movieId = Display.imdbID;
            MovieDetailsModel detailsResult = await _omdbService.GetMovieDetailsAsync(movieId);
            await _pageService.PushPageAsync<MovieDetailsPage, MovieDetailsPageVM>((vm) => vm.Init(detailsResult));
            DetailsResult = detailsResult;
        }

        public async Task<int> GetMoviesExecute()
        {
            pageCounter = 1;
            MovieCollectionModel result = await _omdbService.GetMoviesAsync(SearchText, pageCounter);
            if (result.Response == "False")
            {
                await _alertService.DisplayAlertAsync("Message", "No movie found, please try again", "Ok");
            }
            Result = result;
            return pageCounter;
        }
        public async Task<int> GetMoviesExecuteNext()
        {
            int currentCounter;
            currentCounter = pageCounter;
            pageCounter++;
            if (SearchText != null)
            {
                result = await _omdbService.GetMoviesAsync(SearchText, pageCounter);
                if (result.Response == "False")
                {
                    await _alertService.DisplayAlertAsync("Message", "No more results found, please go back a page or re-enter a movie into the search bar", "Ok");
                    pageCounter = currentCounter;
                }
                else
                {
                    Result = result;
                }
            }
            else
            {
                await _alertService.DisplayAlertAsync("Message", "Please enter a movie and try again", "Ok");
                pageCounter = currentCounter;
            }


            return pageCounter;
        }
        public async Task<int> GetMoviesExecuteBack()
        {
            int currentCounter = pageCounter;
            pageCounter--;

                if (SearchText == null && result == null)
                {
                    await _alertService.DisplayAlertAsync("Message", "Please enter a movie", "Ok");
                    pageCounter = currentCounter;

                }
                else if (pageCounter < 1)
                {
                    await _alertService.DisplayAlertAsync("Message", "You cannot go back a page from here", "Ok");
                    pageCounter = 1;
                }
                else 
                {
                    MovieCollectionModel result = await _omdbService.GetMoviesAsync(SearchText, pageCounter);
                    Result = result;
                }

          
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
