using FunctionZero.CommandZero;
using FunctionZero.MvvmZero;
using MovieSearchApp.Models.OMDb;
using MovieSearchApp.Models.Tastedive;
using MovieSearchApp.Mvvm.Pages;
using MovieSearchApp.Services;
using MovieSearchApp.Services.Alert_Service;
using System;
using System.Collections.Generic;
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
        private Result _display;

        public ICommand GetDetailsCommand { get; }

        public Result Display
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


            GetDetailsCommand = new CommandBuilder().SetExecuteAsync(GetMovieDetailsExecute).Build();
        }

        private async Task GetMovieDetailsExecute()
        {
            MovieDetailsModel result = await _omdbService.GetMovieDetailsWithTitleAsync(Display.Name);
            await _pageService.PushPageAsync<MovieDetailsPage, MovieDetailsPageVM>((vm) => vm.Init(result));
        }

        public void Init(RecommendationModel model)
        {
            RecommendationResult = model;
        }
    }
}
