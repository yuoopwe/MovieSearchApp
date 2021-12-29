using FunctionZero.CommandZero;
using FunctionZero.MvvmZero;
using MovieSearchApp.Models;
using MovieSearchApp.Models.OMDb;
using MovieSearchApp.Models.UserAccount;
using MovieSearchApp.Mvvm.Pages;
using MovieSearchApp.Services;
using MovieSearchApp.Services.Alert_Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MovieSearchApp.Mvvm.PageViewModels
{
    class JournalPageVm : MvvmZeroBaseVm
    {
        private KeyVaultService _keyVaultService;
        private IAlertService _alertService;
        private IPageServiceZero _pageService;
        private OmdbService _omdbService; 
        private MyFlyoutPageFlyoutVm _flyoutVm;
        private List<JournalDetailsModel> _journalDetailsList;
        public bool OtherUser
        {
            get => _otherUser;
            set => SetProperty(ref _otherUser, value);
        }
        public bool CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        public ICommand DeleteItemCommand { get; }
        public ICommand EditItemCommand { get; }

        public ICommand GetDetailsCommand { get; }

        private MovieDetailsModel _detailsresult;
        private bool _otherUser;
        private bool _currentUser;

        public MovieDetailsModel DetailsResult
        {
            get => _detailsresult;
            set => SetProperty(ref _detailsresult, value);
        }

        public AccountDetailsModel AccountDetails { get; set; }


        public List<JournalDetailsModel> JournalDetailsList
        {
            get => _journalDetailsList;
            set => SetProperty(ref _journalDetailsList, value);
        }


        public JournalPageVm(IPageServiceZero pageService, IAlertService alertService, MyFlyoutPageFlyoutVm flyoutVm, OmdbService omdbService, KeyVaultService keyVaultService)
        {
            _keyVaultService = keyVaultService;
            _alertService = alertService;
            _pageService = pageService;
            _flyoutVm = flyoutVm;
            _omdbService = omdbService;

            DeleteItemCommand = new CommandBuilder().SetExecuteAsync(DeleteItemExecute).Build();
            EditItemCommand = new CommandBuilder().SetExecuteAsync(EditItemExecute).Build();
            GetDetailsCommand = new CommandBuilder().SetExecuteAsync(GetDetailsExecute).Build();


        }

        public async Task EditItemExecute(object item1)
        {
            JournalDetailsModel item2 = item1 as JournalDetailsModel;
            await _pageService.PushPageAsync<EditJournalItemPage, EditJournalItemPageVm>(vm => vm.Init(item2, AccountDetails, JournalDetailsList));
        }

        public async Task GetDetailsExecute(object item1)
        {

            JournalDetailsModel item2 = item1 as JournalDetailsModel;
            var movieId = item2.MovieID;
            MovieDetailsModel detailsResult = await _omdbService.GetMovieDetailsWithIdAsync(movieId);
            await _pageService.PushPageAsync<MovieDetailsPage, MovieDetailsPageVM>((vm) => vm.Init(detailsResult));
            DetailsResult = detailsResult;

        }

        public async  Task DeleteItemExecute(object item1)
        {
            var secrets = await _keyVaultService.GetKeysAsync();
            JournalDetailsModel item2 = item1 as JournalDetailsModel;
            SqlDataReader reader;
            SqlCommand deleteDbCommand = new SqlCommand();
            string connectionString = secrets.ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            deleteDbCommand.Connection = connection;
            deleteDbCommand.CommandText = $"DELETE FROM [dbo].[{AccountDetails.Username}Journal] WHERE MovieID = @MovieID;";
            deleteDbCommand.Parameters.Add("@MovieID", SqlDbType.NVarChar);
            deleteDbCommand.Parameters["@MovieID"].Value = item2.MovieID;
            deleteDbCommand.ExecuteReader();
            var itemToRemove = JournalDetailsList.Single(r => r.MovieID == item2.MovieID);
            JournalDetailsList.Remove(itemToRemove);
            _flyoutVm.SetAccountDetails(AccountDetails, JournalDetailsList);
            await _alertService.DisplayAlertAsync("Message", "Item deleted successfully", "Ok");
            await _pageService.PopToRootAsync();
            await _pageService.PushPageAsync<JournalPage, JournalPageVm>(vm => { });

        }

        public void Init(List<JournalDetailsModel> journalList, AccountDetailsModel accountDetails)
        {
            CurrentUser = true;
            OtherUser = false;
            JournalDetailsList = journalList;
            AccountDetails = accountDetails;

        }

        public void OtherUserInit(List<JournalDetailsModel> journalList, AccountDetailsModel accountDetails)
        {
            CurrentUser = false;
            OtherUser = true;
            JournalDetailsList = journalList;
            AccountDetails = accountDetails;

        }

    }
}
