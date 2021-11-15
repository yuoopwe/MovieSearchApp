using FunctionZero.CommandZero;
using FunctionZero.MvvmZero;
using MovieSearchApp.Models.SearchPage;
using MovieSearchApp.Models.ThemovieDb;
using MovieSearchApp.Services;
using MovieSearchApp.Services.Alert_Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MovieSearchApp.Mvvm.PageViewModels
{
    class PopularPageVm : MvvmZeroBaseVm
    {
        private ThemoviedbService _themoviedbService;
        private IPageServiceZero _pageService;
        private IAlertService _alertService;
        private string _searchText;
        private ThemovieDbModel _result;
        private int _pagecounter;
        private PickerModel _selectedFilter;


        public ICommand GetPopularMoviesCommand { get; }
        public ICommand SearchNextPageCommand { get; }
        public ICommand SearchPreviousPageCommand { get; }
        public IList<PickerModel> FilterList { get; set; }
        public PickerModel currentlySelectedFilter { get; set; }
        public int pageCounter
        {
            get => _pagecounter;
            set => SetProperty(ref _pagecounter, value);
        }
        public ThemovieDbModel Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
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

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }


        public PopularPageVm(ThemoviedbService themoviedbService, IPageServiceZero pageService, IAlertService alertService)
        {
            _themoviedbService = themoviedbService;
            _pageService = pageService;
            _alertService = alertService;
            FilterList = new List<PickerModel>(new[]
            {
                    new PickerModel {Filter = "Genre: All" },
                    new PickerModel {id = 28, Filter = "Genre: Action"},
                    new PickerModel {id = 12, Filter = "Genre: Adventure"},
                    new PickerModel {id = 16, Filter = "Genre: Animation"},
                    new PickerModel {id = 35, Filter = "Genre: Comedy"},
                    new PickerModel {id = 80, Filter = "Genre: Crime"},
                    new PickerModel {id = 99, Filter = "Genre: Documentary"},
                    new PickerModel {id = 18, Filter = "Genre: Drama"},
                    new PickerModel {id = 10751, Filter = "Genre: Family"},
                    new PickerModel {id = 14, Filter = "Genre: Fantasy"},
                    new PickerModel {id = 36, Filter = "Genre: History"},
                    new PickerModel {id = 27, Filter = "Genre: Horror"},
                    new PickerModel {id = 10402, Filter = "Genre: Music"},
                    new PickerModel {id = 9648, Filter = "Genre: Mystery"},
                    new PickerModel {id = 10749, Filter = "Genre: Romance"},
                    new PickerModel {id = 878, Filter = "Genre: Science Fiction"},
                    new PickerModel {id = 10770, Filter = "Genre: TV Movie"},
                    new PickerModel {id = 53, Filter = "Genre: Thriller"},
                    new PickerModel {id = 10752, Filter = "Genre: War"},
                    new PickerModel {id = 37, Filter = "Genre: Western"},
                });
            SelectedFilter = FilterList[0];
            currentlySelectedFilter = SelectedFilter;
            GetPopularMoviesCommand = new CommandBuilder().SetExecuteAsync(GetPopularMoviesExecute).Build();
            SearchNextPageCommand = new CommandBuilder().SetExecuteAsync(GetPopularMoviesNextPageExecute).Build();
            SearchPreviousPageCommand = new CommandBuilder().SetExecuteAsync(GetPopularMoviesPreviousPageExecute).Build();
            pageCounter = 0;

        }

        private async Task GetPopularMoviesNextPageExecute()
        {
            pageCounter++;
            ThemovieDbModel result = await _themoviedbService.DiscoverMovies(SearchText,pageCounter);
            Result = result;
            AppendPosterURLToList();


        }

        private async Task GetPopularMoviesPreviousPageExecute()
        {
            pageCounter--;
            ThemovieDbModel result = await _themoviedbService.DiscoverMovies(SearchText, pageCounter);
            Result = result;
            AppendPosterURLToList();
        }

        private async Task GetPopularMoviesExecute()
        {
           pageCounter = 1; 
            if (SelectedFilter.Filter == "Genre: All")
            {
                ThemovieDbModel result = await _themoviedbService.DiscoverMovies(SearchText, pageCounter);
                Result = result;
            }
            else
            {
                ThemovieDbModel result = await _themoviedbService.DiscoverMoviesGenre(SearchText, pageCounter, SelectedFilter.id);
                Result = result;

            }
           AppendPosterURLToList();
        }

        public void AppendPosterURLToList()
        {
            for(int i=0; i < Result.results.Count; i++)
            {
                Result.results[i].poster_path = Result.results[i].poster_path.Insert(0, "https://image.tmdb.org/t/p/w400");
            }
        }
    }

}
