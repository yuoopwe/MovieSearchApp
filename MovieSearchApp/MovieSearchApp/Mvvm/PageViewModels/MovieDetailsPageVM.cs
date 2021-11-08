using FunctionZero.MvvmZero;
using MovieSearchApp.Models.OMDb;
using MovieSearchApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearchApp.Mvvm.PageViewModels
{
    class MovieDetailsPageVM : MvvmZeroBaseVm
    {
        
        public MovieDetailsModel DetailsResult
        {
            get => _detailsresult;
            set => SetProperty(ref _detailsresult, value);
        }
        public readonly OmdbService _omdbService;
        public MovieDetailsModel _detailsresult;

        public MovieDetailsPageVM(OmdbService omdbService)
        {
            _omdbService = omdbService;

        }

        public void Init(MovieDetailsModel model)
        {
            DetailsResult = model;
        }
    }
}
