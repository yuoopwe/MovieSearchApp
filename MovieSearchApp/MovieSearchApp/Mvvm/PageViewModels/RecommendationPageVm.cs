using FunctionZero.CommandZero;
using FunctionZero.MvvmZero;
using MovieSearchApp.Models;
using MovieSearchApp.Models.OMDb;
using MovieSearchApp.Models.Tastedive;
using MovieSearchApp.Models.UserAccount;
using MovieSearchApp.Mvvm.Pages;
using MovieSearchApp.Services;
using MovieSearchApp.Services.Alert_Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MovieSearchApp.Mvvm.PageViewModels
{
    class RecommendationPageVm : MvvmZeroBaseVm
    {
        public AccountDetailsModel AccountDetails { get; set; }

        public List<JournalDetailsModel> JournalDetailsList { get; set; }

        private RecommendationModel _recommendationResult;
        private ThemoviedbService _themoviedbService;
        private OmdbService _omdbService;
        private IPageServiceZero _pageService;
        private IAlertService _alertService;
        private MovieDetailsModel _display;
        private ObservableCollection<MovieDetailsModel> _movieObjectList;

        public ICommand GetDetailsCommand { get; }
        public ICommand ShowTrailerCommand { get; }
        public ICommand AddToListCommand { get; }

        public ObservableCollection<MovieDetailsModel> MovieObjectList
        {
            get => _movieObjectList;
            set => SetProperty(ref _movieObjectList, value);
        }
        public MovieDetailsModel Display
        {
            get => _display;
            set => SetProperty(ref _display, value);
        }


        public RecommendationModel RecommendationResult
        {
            get => _recommendationResult;
            set => SetProperty(ref _recommendationResult, value);
        }
        public RecommendationPageVm(OmdbService omdbService, ThemoviedbService themoviedbService, IPageServiceZero pageService, IAlertService alertService)
        {
            _themoviedbService = themoviedbService;
            _omdbService = omdbService;
            _pageService = pageService;
            _alertService = alertService;
            AccountDetails = new AccountDetailsModel();
            JournalDetailsList = new List<JournalDetailsModel>();
            AccountDetails.IsLoggedIn = false;
            MovieObjectList = new ObservableCollection<MovieDetailsModel>();
            AddToListCommand = new CommandBuilder().SetExecuteAsync(AddToListExecute).Build();
            ShowTrailerCommand = new CommandBuilder().SetExecuteAsync(GetTrailerExecute).Build();
            GetDetailsCommand = new CommandBuilder().SetExecuteAsync(GetMovieDetailsExecute).Build();
        }

        public async Task AddToListExecute(object item1)
        {
            bool filmIsInList = false;
            if (AccountDetails.IsLoggedIn == true)
            {
                MovieDetailsModel item2 = item1 as MovieDetailsModel;
                foreach (var item in JournalDetailsList)
                {
                    if (item.MovieTitle == item2.Title)
                    {
                        filmIsInList = true;
                    }
                }
                if (filmIsInList == true)
                {
                   await _alertService.DisplayAlertAsync("Message", "You have already added this film to your list", "Ok");

                }
                else
                {
                    await _pageService.PushPageAsync<AddToListPage, AddToListPageVm>((vm) => vm.RecommendationInit(item2, JournalDetailsList, AccountDetails, "Recommendation", RecommendationResult));
                }
               
            }
            else
            {
                await _alertService.DisplayAlertAsync("Message", "You must login to access this feature", "Ok");

            }

        }

        private async Task GetTrailerExecute(object items)
        {
            MovieDetailsModel item2 = items as MovieDetailsModel;
            for(int i=0; i < 21; i++)
            {
                if(RecommendationResult.Similar.Results[i].Name.ToLower() == item2.Title.ToLower())
                {
                    await _pageService.PushPageAsync<TrailerPage, TrailerPageVm>((vm) => vm.Init(RecommendationResult.Similar.Results[i], item2));
                    break;
                }
                
                if(i == 21)
                {
                    await _alertService.DisplayAlertAsync("Trailer", "Trailer Not Found", "Ok");
                    break;
                }
            }

        }
        private async Task GetMovieDetailsExecute(object items)
        {
            MovieDetailsModel item2 = items as MovieDetailsModel;
            await _pageService.PushPageAsync<MovieDetailsPage, MovieDetailsPageVM>((vm) => vm.Init(item2));
        }

        public async Task Init(RecommendationModel model, AccountDetailsModel accountDetails, List<JournalDetailsModel> journalList)
        {
            MovieObjectList.Clear();
            RecommendationResult = model;
            foreach (var movieobject in model.Similar.Results)
            {
                MovieDetailsModel result = await _omdbService.GetMovieDetailsWithTitleAsync(movieobject.Name);
                MovieObjectList.Add(result);
            }
            if (accountDetails.IsLoggedIn == true)
            {
                AccountDetails = accountDetails;
                JournalDetailsList = journalList;
            }
        }
    }
}
