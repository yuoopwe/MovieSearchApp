using FunctionZero.CommandZero;
using FunctionZero.MvvmZero;
using MovieSearchApp.Models;
using MovieSearchApp.Models.UserAccount;
using MovieSearchApp.Mvvm.Pages;
using MovieSearchApp.Services.Alert_Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MovieSearchApp.Mvvm.PageViewModels
{
    class EditJournalItemPageVm : MvvmZeroBaseVm
    {
        private IAlertService _alertService;
        private IPageServiceZero _pageService;
        private MyFlyoutPageFlyoutVm _flyoutVm;
        private string _movieRating;
        private string _movieComments;
        private JournalDetailsModel _editItem;

        public JournalDetailsModel EditItem
        {
            get => _editItem;
            set => SetProperty(ref _editItem, value);
        }
        public AccountDetailsModel AccountDetails { get; set; }
        public List<JournalDetailsModel> JournalDetailsList { get; set; }



        public string MovieComments
        {
            get => _movieComments;
            set => SetProperty(ref _movieComments, value);
        }

        public string MovieRating
        {
            get => _movieRating;
            set => SetProperty(ref _movieRating, value);
        }
        public ICommand SaveCommand { get; }

        public EditJournalItemPageVm(IPageServiceZero pageService, IAlertService alertService, MyFlyoutPageFlyoutVm flyoutVm)
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
            command.CommandText = $"UPDATE [dbo].[{AccountDetails.Username}Journal] SET MovieComments = '{MovieComments}', MovieRating = '{MovieRating}' WHERE MovieID = '{EditItem.MovieID}';";
            command.ExecuteReader();
            connection.Close();
            connection.Open();
            command.CommandText = $"SELECT * FROM [dbo].[{AccountDetails.Username}Journal] WHERE MovieID = '{EditItem.MovieID}'";
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                if (MovieComments.Trim() == reader["MovieComments"].ToString().Trim() || MovieRating.Trim() == reader["MovieRating"])
                {
                    EditItem.MovieComments = MovieComments;
                    EditItem.MovieRating = MovieRating;
                    var obj = JournalDetailsList.FirstOrDefault(x => x.MovieID == EditItem.MovieID);
                    if (obj != null) 
                    {
                        
                        obj.MovieComments = EditItem.MovieComments;
                        obj.MovieRating = EditItem.MovieRating;
                    }
                    
                    await _alertService.DisplayAlertAsync("Message", "Movie Edited Successfully", "Ok");
                }
            }
            _flyoutVm.SetAccountDetails(AccountDetails, JournalDetailsList);
            await _pageService.PopToRootAsync();
            await _pageService.PushPageAsync<JournalPage, JournalPageVm>((vm) => vm.Init(JournalDetailsList, AccountDetails));
            await _pageService.PopToRootAsync();
            await _pageService.PushPageAsync<JournalPage, JournalPageVm>((vm) => { });
        }

        public void Init(JournalDetailsModel item, AccountDetailsModel accountDetails, List<JournalDetailsModel> journalList)
        {
            EditItem = item;
            AccountDetails = accountDetails;
            MovieRating = EditItem.MovieRating;
            MovieComments = EditItem.MovieComments;
            JournalDetailsList = journalList;
        }
    }
}
