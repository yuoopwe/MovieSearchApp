using FunctionZero.CommandZero;
using MovieSearchApp.Models.OMDb;
using MovieSearchApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MovieSearchApp.Mvvm.PageViewModels
{
    class SearchPageVm : FunctionZero.MvvmZero.MvvmZeroBaseVm
    {
        public MovieCollectionModel _result;
        public ICommand SearchMoviesCommand { get; }
        public readonly OmdbService _omdbService;
        public object _searchText;
        public MovieCollectionModel Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }
        public object SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public SearchPageVm(OmdbService omdbService)
        {
            _omdbService = omdbService;

            SearchMoviesCommand = new CommandBuilder().SetExecuteAsync(GetMoviesExecute).Build();
        }

        public async Task GetMoviesExecute()
        {
            MovieCollectionModel result = await _omdbService.GetMoviesAsync(SearchText.ToString());
            Result = result;
        }
    }
}
