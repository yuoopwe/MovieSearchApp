using FunctionZero.MvvmZero;
using MovieSearchApp.Models.Tastedive;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearchApp.Mvvm.PageViewModels
{
    class RecommendationPageVm : MvvmZeroBaseVm
    {
        private RecommendationModel _recommendationResult;

        public RecommendationModel RecommendationResult
        {
            get => _recommendationResult;
            set => SetProperty(ref _recommendationResult, value);
        }
        public void Init(RecommendationModel model)
        {
            RecommendationResult = model;
        }
    }
}
