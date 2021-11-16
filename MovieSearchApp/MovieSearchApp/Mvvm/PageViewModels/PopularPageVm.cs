using FunctionZero.CommandZero;
using FunctionZero.MvvmZero;
using MovieSearchApp.Models.OMDb;
using MovieSearchApp.Models.PopularPage;
using MovieSearchApp.Models.SearchPage;
using MovieSearchApp.Models.Tastedive;
using MovieSearchApp.Models.ThemovieDb;
using MovieSearchApp.Mvvm.Pages;
using MovieSearchApp.Mvvm.Pages.PopularPage;
using MovieSearchApp.Mvvm.Pages.PopularPageFolder;
using MovieSearchApp.Services;
using MovieSearchApp.Services.Alert_Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MovieSearchApp.Mvvm.PageViewModels
{
    class PopularPageVm : MvvmZeroBaseVm
    {
        private TastediveService _tastediveService;
        private OmdbService _omdbService;
        private ThemoviedbService _themoviedbService;
        private IPageServiceZero _pageService;
        private IAlertService _alertService;
        public bool AgeRatingVisible
        {
            get => _ageRatingVisible;
            set => SetProperty(ref _ageRatingVisible, value);
        }

        public bool LanguageVisible
        {
            get => _languageVisible;
            set => SetProperty(ref _languageVisible, value);
        }
        private string _searchText;
        private ThemovieDbModel _result;
        private int _pagecounter;
        private CheckboxModel _selectedCheckbox;
        private string _genreCheckboxText;
        private bool _ageRatingVisible;
        private bool _languageVisible;
        private AgeRatingModel _selectedAgeRating;
        private LanguageModel _selectedLanguage;
        private MovieDetailsModel _display;
        private IList<CheckboxModel> _checkboxList;
        private ObservableCollection<MovieDetailsModel> _scoresDetailsList;

        private string genreListFormatted { get; set; }
        public List<string> genresList { get; set; }
        public string GenreCheckboxText
        {
            get => _genreCheckboxText;
            set => SetProperty(ref _genreCheckboxText, value);
        }

        public ICommand GetPopularMoviesCommand { get; }
        public ICommand SearchNextPageCommand { get; }
        public ICommand GenreCheckboxPageCommand { get; }
        public ICommand SearchPreviousPageCommand { get; }
        public ICommand GetDetailsCommand { get; }
        public ICommand GetRecommendationsCommand { get; }

        public IList<CheckboxModel> CheckboxList
        {
            get => _checkboxList;
            set => SetProperty(ref _checkboxList, value);
        }
        public IList<AgeRatingModel> AgeRatingList { get; set; }
        public IList<LanguageModel> LanguageList { get; set; }

        public CheckboxModel currentlySelectedCheckbox { get; set; }
        public MovieDetailsModel Display
        {
            get => _display;
            set => SetProperty(ref _display, value);
        }
        public LanguageModel SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (_selectedLanguage != value)
                {
                    _selectedLanguage = value;
                    this.OnPropertyChanged();
                };
            }
        }
        public AgeRatingModel SelectedAgeRating
        {
            get => _selectedAgeRating;
            set
            {
                if (_selectedAgeRating != value)
                {
                    _selectedAgeRating = value;
                    this.OnPropertyChanged();
                };
            }
        }

        public int pageCounter
        {
            get => _pagecounter;
            set => SetProperty(ref _pagecounter, value);
        }

        public ObservableCollection<MovieDetailsModel> ScoresDetailsList
        {
            get => _scoresDetailsList;
            set => SetProperty(ref _scoresDetailsList, value);
        }
        public MovieDetailsModel DetailsResult { get; set; }
        public ThemovieDbModel Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }
        public CheckboxModel SelectedCheckbox
        {
            get => _selectedCheckbox;
            set
            {
                if (_selectedCheckbox != value)
                {
                    _selectedCheckbox = value;
                    this.OnPropertyChanged();
                };
            }
        }

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }


        public PopularPageVm(OmdbService omdbService, ThemoviedbService themoviedbService, TastediveService tastediveService, IPageServiceZero pageService, IAlertService alertService)
        {
            _tastediveService = tastediveService;
            _omdbService = omdbService;
            _themoviedbService = themoviedbService;
            _pageService = pageService;
            _alertService = alertService;
            AgeRatingVisible = false;
            LanguageVisible = false;
            LanguageList = new List<LanguageModel>(new[]
            {
                new LanguageModel {LanguageName = "English", LanguageCode = "en"},
                new LanguageModel {LanguageName = "Japanese", LanguageCode = "ja"},
                new LanguageModel {LanguageName = "Chinese", LanguageCode = "zh"},




            });

            AgeRatingList = new List<AgeRatingModel>(new[]
            {
                new AgeRatingModel {Rating = "G"},
                new AgeRatingModel {Rating = "PG"},
                new AgeRatingModel {Rating = "PG-13"},
                new AgeRatingModel {Rating = "R"},
                new AgeRatingModel {Rating = "NC-17"},

            });
            CheckboxList = new List<CheckboxModel>(new[]
            {
                    new CheckboxModel {id = 28, Filter = "Action", IsChecked = false},
                    new CheckboxModel {id = 12, Filter = "Adventure", IsChecked = false},
                    new CheckboxModel {id = 16, Filter = "Animation", IsChecked = false},
                    new CheckboxModel {id = 35, Filter = "Comedy", IsChecked = false},
                    new CheckboxModel {id = 80, Filter = "Crime", IsChecked = false},
                    new CheckboxModel {id = 99, Filter = "Documentary", IsChecked = false},
                    new CheckboxModel {id = 18, Filter = "Drama", IsChecked = false},
                    new CheckboxModel {id = 10751, Filter = "Family", IsChecked = false},
                    new CheckboxModel {id = 14, Filter = "Fantasy", IsChecked = false},
                    new CheckboxModel {id = 36, Filter = "History", IsChecked = false},
                    new CheckboxModel {id = 27, Filter = "Horror", IsChecked = false},
                    new CheckboxModel {id = 10402, Filter = "Music", IsChecked = false},
                    new CheckboxModel {id = 9648, Filter = "Mystery", IsChecked = false},
                    new CheckboxModel {id = 10749, Filter = "Romance", IsChecked = false},
                    new CheckboxModel {id = 878, Filter = "Science Fiction", IsChecked = false},
                    new CheckboxModel {id = 10770, Filter = "TV Movie", IsChecked = false},
                    new CheckboxModel {id = 53, Filter = "Thriller", IsChecked = false},
                    new CheckboxModel {id = 10752, Filter = "War", IsChecked = false},
                    new CheckboxModel {id = 37, Filter = "Western", IsChecked = false},
                });
            genresList = new List<string>();
            ScoresDetailsList = new ObservableCollection<MovieDetailsModel>();
            SearchText = "";
            Apply(CheckboxList);
            LanguageVisible = false;
            AgeRatingVisible = false;
            SelectedCheckbox = CheckboxList[0];
            currentlySelectedCheckbox = SelectedCheckbox;
            GetRecommendationsCommand = new CommandBuilder().SetExecuteAsync(GetRecommendationsExecute).Build();
            GenreCheckboxPageCommand = new CommandBuilder().SetExecuteAsync(GenreCheckboxPageExecute).Build();
            GetPopularMoviesCommand = new CommandBuilder().SetExecuteAsync(GetPopularMoviesExecute).Build();
            SearchNextPageCommand = new CommandBuilder().SetExecuteAsync(GetPopularMoviesNextPageExecute).Build();
            SearchPreviousPageCommand = new CommandBuilder().SetExecuteAsync(GetPopularMoviesPreviousPageExecute).Build();
            GetDetailsCommand = new CommandBuilder().SetExecuteAsync(GetMovieDetailsExecute).Build();
            pageCounter = 0;

        }

        private async Task GetRecommendationsExecute()
        {
            if (Display == null)
            {
                await _alertService.DisplayAlertAsync("Message", "Please Select A Movie First", "Ok");
            }
            else
            {
                RecommendationModel recommendationResult;
                string recommendationSearch = Display.Title;
                for (recommendationResult = await _tastediveService.GetRecommendationsMovie(recommendationSearch); recommendationResult.Similar.Results.Count <= 0; recommendationSearch = RemoveLastWord(recommendationSearch))
                {
                    if (recommendationSearch == "")
                    {
                        await _alertService.DisplayAlertAsync("Message", "No Recommendations Found", "Ok");
                        break;
                    }
                    recommendationResult = await _tastediveService.GetRecommendationsMovie(recommendationSearch);
                }
                await _pageService.PushPageAsync<RecommendationPage, RecommendationPageVm>((vm) => vm.Init(recommendationResult));
            }


        }

        private async Task GetMovieDetailsExecute()
        {
            if (Display == null)
            {
                await _alertService.DisplayAlertAsync("Message", "Please Select A Movie First", "Ok");
            }else
            {      
                await _pageService.PushPageAsync<MovieDetailsPage, MovieDetailsPageVM>((vm) => vm.Init(Display));
            }


        }

        private async Task GenreCheckboxPageExecute()
        {
            await _pageService.PushPageAsync<GenreCheckbox, GenreCheckboxVm>((vm) => vm.init(CheckboxList));
        }

        private async Task GetPopularMoviesNextPageExecute()
        {
            if (pageCounter == 0)
            {
                await _alertService.DisplayAlertAsync("Message", "Please Search A Movie First", "Ok");
            }
            else
            {
                pageCounter++;
                await MovieSearch(genreListFormatted);
                if (Result.results.Count == 0)
                {
                    await _alertService.DisplayAlertAsync("Message", "No More Results Found", "Ok");
                    pageCounter--;

                }
            }
            

        }

        private async Task GetPopularMoviesPreviousPageExecute()
        {
            pageCounter--;
            if (pageCounter < 1)
            {
                await _alertService.DisplayAlertAsync("Message", "You cannot go back from here", "Ok");
                pageCounter++;
            }
            else
            {
                await MovieSearch(genreListFormatted);
            }

           
        }

        private async Task GetPopularMoviesExecute()
        {
            pageCounter = 1;
            if (LanguageVisible == true && SelectedLanguage == null)
            {
                await _alertService.DisplayAlertAsync("Message", "Please Select A Language Or Disable The Filter", "Ok");
            }else if (AgeRatingVisible == true && SelectedAgeRating == null)
            {
                await _alertService.DisplayAlertAsync("Message", "Please Select An AgeRating Or Disable The Filter", "Ok");

            }
            else 
            { 
                await MovieSearch(genreListFormatted); 
            }
            if (Result.results.Count == 0)
            {
                await _alertService.DisplayAlertAsync("Message", "No More Results Found", "Ok");

            }

        }

        private string RemoveLastWord(string title)
        {
            title = title.Trim();

            string newStr = "";
            if (title.Contains(" "))
            {
                newStr = title.Substring(0, title.LastIndexOf(' ')).TrimEnd();
            }
    
            newStr = newStr.TrimEnd(':');
            newStr = newStr.Replace('é', 'e');

            return newStr;
        }

        private async Task MovieSearch(string genreListFormatted)
        {
            ScoresDetailsList.Clear();
            if (LanguageVisible == true && AgeRatingVisible == true)
            {
                ThemovieDbModel result = await _themoviedbService.DiscoverMovies(SearchText, pageCounter, genreListFormatted, SelectedLanguage.LanguageCode, SelectedAgeRating.Rating);
                Result = result;
                AppendPosterURLToList();
                await CreateDisplayList(result);
            }
            else if (LanguageVisible == true && AgeRatingVisible == false)
            {
                ThemovieDbModel result = await _themoviedbService.DiscoverMovies(SearchText, pageCounter, genreListFormatted, SelectedLanguage.LanguageCode, "");
                Result = result;
                AppendPosterURLToList();
                await CreateDisplayList(result);

            }
            else if (LanguageVisible == false && AgeRatingVisible == true)
            {
                ThemovieDbModel result = await _themoviedbService.DiscoverMovies(SearchText, pageCounter, genreListFormatted, "", SelectedAgeRating.Rating);
                Result = result;
                AppendPosterURLToList();
                await CreateDisplayList(result);

            }
            else
            {
                ThemovieDbModel result = await _themoviedbService.DiscoverMovies(SearchText, pageCounter, genreListFormatted, "", "");
                Result = result;
                AppendPosterURLToList();
                await CreateDisplayList(result);


            }
        }

        public async Task CreateDisplayList(ThemovieDbModel result)
        {
            foreach (var item in result.results)
            {
                TheMovieDbDetailsModel detailResult = await _themoviedbService.DiscoverMoviesID(item.id.ToString());
                MovieDetailsModel result2 = await _omdbService.GetMovieDetailsWithIdAsync(detailResult.imdb_id);
                ScoresDetailsList.Add(result2);

            }

        }
        public void AppendPosterURLToList()
        {
            for(int i=0; i < Result.results.Count; i++)
            {
                Result.results[i].poster_path = Result.results[i].poster_path.Insert(0, "https://image.tmdb.org/t/p/w400");
            }
        }
        public string Apply(IList<CheckboxModel> checkboxList)
        {

            var genreListJoined = "";
            genreListFormatted = "";
            genresList.Clear();
            genresList.Add("");
            foreach (var genre in CheckboxList)
            {
                if (genre.IsChecked == true)
                {
                    genresList.Add($"{genre.id},");
                }
            }
            genreListJoined = string.Join("", genresList);
            genreListFormatted = genreListJoined == "" ? genreListJoined : genreListJoined.Remove(genreListJoined.Length - 1);
            var sb = new StringBuilder();
            CheckboxList = checkboxList;
            foreach (var genre in CheckboxList)
            {
                if (genre.IsChecked == true)
                {
                    sb.Append($" {genre.Filter},");
                }
            }
            GenreCheckboxText = sb.ToString() == "" ? GenreCheckboxText = "Genre: All " : sb.ToString();
            GenreCheckboxText = GenreCheckboxText.Remove(GenreCheckboxText.Length - 1); 
            return genreListFormatted;
        }
    }

}
