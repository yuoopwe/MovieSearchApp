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
    class JournalPageVm : MvvmZeroBaseVm
    {
        private IAlertService _alertService;
        private IPageServiceZero _pageService;
        private MyFlyoutPageFlyoutVm _flyoutVm;
        private List<JournalDetailsModel> _journalDetailsList;


        public ICommand DeleteItemCommand { get; }
        public ICommand EditItemCommand { get; }
        public AccountDetailsModel AccountDetails { get; set; }


        public List<JournalDetailsModel> JournalDetailsList
        {
            get => _journalDetailsList;
            set => SetProperty(ref _journalDetailsList, value);
        }


        public JournalPageVm(IPageServiceZero pageService, IAlertService alertService, MyFlyoutPageFlyoutVm flyoutVm)
        {

            _alertService = alertService;
            _pageService = pageService;
            _flyoutVm = flyoutVm;

            DeleteItemCommand = new CommandBuilder().SetExecuteAsync(DeleteItemExecute).Build();
            EditItemCommand = new CommandBuilder().SetExecuteAsync(EditItemExecute).Build();


        }

        public async Task EditItemExecute(object item1)
        {
            JournalDetailsModel item2 = item1 as JournalDetailsModel;
            await _pageService.PushPageAsync<EditJournalItemPage, EditJournalItemPageVm>(vm => vm.Init(item2, AccountDetails, JournalDetailsList));
        }

        public async  Task DeleteItemExecute(object item1)
        {
            JournalDetailsModel item2 = item1 as JournalDetailsModel;
            SqlDataReader reader;
            SqlCommand command = new SqlCommand();
            string connectionString = ConfigurationManager.ConnectionStrings["Test"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            command.Connection = connection;
            command.CommandText = $"DELETE FROM [dbo].[{AccountDetails.Username}Journal] WHERE MovieID='{item2.MovieID}';";
            command.ExecuteReader();
            var itemToRemove = JournalDetailsList.Single(r => r.MovieID == item2.MovieID);
            JournalDetailsList.Remove(itemToRemove);
            _flyoutVm.SetAccountDetails(AccountDetails, JournalDetailsList);
            await _alertService.DisplayAlertAsync("Message", "Item deleted successfully", "Ok");
            await _pageService.PopToRootAsync();
            await _pageService.PushPageAsync<JournalPage, JournalPageVm>(vm => { });

        }

        public void Init(List<JournalDetailsModel> journalList, AccountDetailsModel accountDetails)
        {
            JournalDetailsList = journalList;
            AccountDetails = accountDetails;

        }


    }
}
