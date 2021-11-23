using FunctionZero.CommandZero;
using FunctionZero.MvvmZero;
using MovieSearchApp.Models.OMDb;
using MovieSearchApp.Models.Tastedive;
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

        private RecommendationModel _recommendationResult;
        private ThemoviedbService _themoviedbService;
        private OmdbService _omdbService;
        private IPageServiceZero _pageService;
        private IAlertService _alertService;
        private MovieDetailsModel _display;
        private ObservableCollection<MovieDetailsModel> _movieObjectList;

        public ICommand GetDetailsCommand { get; }
        public ICommand ShowTrailerCommand { get; }
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
            MovieObjectList = new ObservableCollection<MovieDetailsModel>();
            ShowTrailerCommand = new CommandBuilder().SetExecuteAsync(GetTrailerExecute).Build();
            GetDetailsCommand = new CommandBuilder().SetExecuteAsync(GetMovieDetailsExecute).Build();
        }

        private async Task GetTrailerExecute()
        {
            for(int i=0; i < 20; i++)
            {
                if(RecommendationResult.Similar.Results[i].Name == Display.Title)
                {
                    await _pageService.PushPageAsync<TrailerPage, TrailerPageVm>((vm) => vm.Init(RecommendationResult.Similar.Results[i], Display));
                    break;
                }
            }

        }
        private async Task GetMovieDetailsExecute()
        {
            
            await _pageService.PushPageAsync<MovieDetailsPage, MovieDetailsPageVM>((vm) => vm.Init(Display));
        }

        public async Task Init(RecommendationModel model)
        {
            MovieObjectList.Clear();
            RecommendationResult = model;
            foreach (var item in model.Similar.Results)
            {
                MovieDetailsModel result = await _omdbService.GetMovieDetailsWithTitleAsync(item.Name);
                MovieObjectList.Add(result);
            }
        }
    }
}
