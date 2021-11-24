using FunctionZero.CommandZero;
using FunctionZero.MvvmZero;
using MovieSearchApp.Models;
using MovieSearchApp.Models.OMDb;
using MovieSearchApp.Models.Tastedive;
using MovieSearchApp.Models.UserAccount;
using MovieSearchApp.Mvvm.Pages;
using MovieSearchApp.Mvvm.Pages.PopularPage;
using MovieSearchApp.Services.Alert_Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MovieSearchApp.Mvvm.PageViewModels
{
    class AddToListPageVm : MvvmZeroBaseVm
    {
        public AccountDetailsModel AccountDetails { get; set; }
        public List<JournalDetailsModel> JournalDetailsList { get; set; }
        public RecommendationModel RecommendationResult { get; set; }



        private IAlertService _alertService;
        private IPageServiceZero _pageService;
        private MyFlyoutPageFlyoutVm _flyoutVm;
        private string _movieComments;
        private string _movieRating;
        private MovieDetailsModel _detailsModel;

        public string CurrentPage { get; set; }

        public ICommand SaveCommand { get; }
        public MovieDetailsModel DetailsModel
        {
            get => _detailsModel;
            set => SetProperty(ref _detailsModel, value);
        }
        public string MovieRating
        {
            get => _movieRating;
            set => SetProperty(ref _movieRating, value);
        }
        public string MovieComments
        {
            get => _movieComments;
            set => SetProperty(ref _movieComments, value);
        }
        public AddToListPageVm(IPageServiceZero pageService, IAlertService alertService, MyFlyoutPageFlyoutVm flyoutVm)
        {

            _alertService = alertService;
            _pageService = pageService;
            _flyoutVm = flyoutVm;

            SaveCommand = new CommandBuilder().SetExecuteAsync(SaveExecute).Build();

        }

        public async Task SaveExecute()
        {
            SqlDataReader reader;
            SqlCommand command = new SqlCommand();
            string connectionString = ConfigurationManager.ConnectionStrings["Test"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            command.Connection = connection;
            command.CommandText = $"INSERT INTO [dbo].[{AccountDetails.Username}Journal] (MovieID, MovieTitle, MovieRating, MovieComments, MovieRuntime) VALUES ('{DetailsModel.imdbID}','{DetailsModel.Title}','{MovieRating}','{MovieComments}','{DetailsModel.Runtime}');";
            command.ExecuteReader();
            connection.Close();
            connection.Open();
            command.CommandText = $"SELECT * FROM [dbo].[{AccountDetails.Username}Journal] WHERE MovieID = '{DetailsModel.imdbID}'";
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                if (DetailsModel.imdbID == reader["MovieID"].ToString().Trim())
                {
                    JournalDetailsList.Add(new JournalDetailsModel { MovieID = DetailsModel.imdbID, MovieComments = MovieComments, MovieRating = MovieRating, MovieRuntime = DetailsModel.Runtime, MovieTitle = DetailsModel.Title });
                    await _alertService.DisplayAlertAsync("Message", "Movie Added To List Successfully","Ok");
                }
            }
            switch (CurrentPage)
            {
                case "Search":
                    await _pageService.PopToRootAsync();
                    await _pageService.PushPageAsync<SearchPage, SearchPageVm>((vm) => vm.Init(AccountDetails, JournalDetailsList));
                    break;
                case "Recommendations":
                    await _pageService.PopToRootAsync();
                    await _pageService.PushPageAsync<RecommendationPage, RecommendationPageVm>(async(vm) => await vm.Init(RecommendationResult, AccountDetails, JournalDetailsList));
                    break;
                case "Popular":
                    await _pageService.PopToRootAsync();
                    await _pageService.PushPageAsync<PopularPage, PopularPageVm>((vm) => vm.Init(AccountDetails, JournalDetailsList));
                    break;
                default:
                    break;
            }
        }

        public void Init(MovieDetailsModel detailsModel, List<JournalDetailsModel> journalList, AccountDetailsModel accountDetails, string PrevPage)
        {
            DetailsModel = detailsModel;
            AccountDetails = accountDetails;
            CurrentPage = PrevPage;
            JournalDetailsList = journalList;
            MovieRating = "";
            MovieComments = "";
        }

        public void RecommendationInit(MovieDetailsModel detailsModel, List<JournalDetailsModel> journalList, AccountDetailsModel accountDetails, string PrevPage, RecommendationModel recModel)
        {
            DetailsModel = detailsModel;
            AccountDetails = accountDetails;
            CurrentPage = PrevPage;
            JournalDetailsList = journalList;
            RecommendationResult = recModel;
            MovieRating = "";
            MovieComments = "";
        }
    }
}
