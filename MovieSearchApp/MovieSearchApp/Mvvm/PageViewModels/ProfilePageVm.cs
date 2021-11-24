using FunctionZero.CommandZero;
using FunctionZero.MvvmZero;
using MovieSearchApp.Models;
using MovieSearchApp.Models.UserAccount;
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
    class ProfilePageVm : MvvmZeroBaseVm
    {
        private readonly IAlertService _alertService;
        private readonly IPageServiceZero _pageService;
        private string _profileDescriptionText;
        private string _profileNameText;
        private int _totalTimeWatched;

        public string ProfileDescriptionText
        {
            get => _profileDescriptionText;
            set => SetProperty(ref _profileDescriptionText, value);
        }
        public int TotalTimeWatched
        {
            get => _totalTimeWatched;
            set => SetProperty(ref _totalTimeWatched, value);
        }
        public string ProfileNameText
        {
            get => _profileNameText;
            set => SetProperty(ref _profileNameText, value);
        }
        public AccountDetailsModel AccountDetails { get; set; }
        public List<JournalDetailsModel> JournalDetailsList { get; set; }

        public ICommand ChangeProfileNameCommand { get; }
        public ICommand ChangeProfileDescriptionCommand { get; }


        public ProfilePageVm(IPageServiceZero pageService, IAlertService alertService)
        {

            _alertService = alertService;
            _pageService = pageService;
  
            ChangeProfileNameCommand = new CommandBuilder().SetExecuteAsync(ChangeProfileNameExecute).Build();
            ChangeProfileDescriptionCommand = new CommandBuilder().SetExecuteAsync(ChangeProfileDescriptionExecute).Build();

        }

        public async Task ChangeProfileDescriptionExecute()
        {
            SqlDataReader reader;
            SqlCommand command = new SqlCommand();
            string connectionString = ConfigurationManager.ConnectionStrings["Test"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            //Check account is made
            connection.Open();
            command.Connection = connection;
            command.CommandText = $"UPDATE [dbo].[LoginTable] Set profile_description = '{ProfileDescriptionText}' Where id = '{AccountDetails.Id}'; ";
            command.ExecuteReader();
            connection.Close();
            connection.Open();
            command.CommandText = $"Select * from dbo.LoginTable WHERE id = '{AccountDetails.Id}'";
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                if (ProfileDescriptionText.Trim() == reader["profile_description"].ToString().Trim())
                {
                    AccountDetails.ProfileDescription = ProfileDescriptionText;
                    await _alertService.DisplayAlertAsync("Success", "Profile Description Change Successful", "Ok");
                    AccountDetails.ProfileName = ProfileNameText;
                }
                else
                {
                    await _alertService.DisplayAlertAsync("Success", "Profile Description Change Fauiled", "Ok");

                }
            }

        }
        public async Task ChangeProfileNameExecute()
        {
            SqlDataReader reader;
            SqlCommand command = new SqlCommand();
            string connectionString = ConfigurationManager.ConnectionStrings["Test"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            //Check account is made
            connection.Open();
            command.Connection = connection;
            command.CommandText = $"UPDATE [dbo].[LoginTable] Set profile_name = '{ProfileNameText}' Where id = '{AccountDetails.Id}'; ";
            command.ExecuteReader();
            connection.Close();
            connection.Open();
            command.CommandText = $"Select * from dbo.LoginTable WHERE id = '{AccountDetails.Id}'";
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                if (ProfileNameText == reader["profile_name"].ToString().Trim())
                {
                    AccountDetails.ProfileName = ProfileNameText;
                    await _alertService.DisplayAlertAsync("Success", "Profile Name Change Successful", "Ok");
                }
                else
                {
                    await _alertService.DisplayAlertAsync("Success", "Profile Name Change Fauiled", "Ok");

                }
            }

        }
        public void Init(AccountDetailsModel accountDetails, List<JournalDetailsModel> journalDetails)
        {
            TotalTimeWatched = 0;
            AccountDetails = accountDetails;
            ProfileNameText = accountDetails.ProfileName;
            ProfileDescriptionText = accountDetails.ProfileDescription;
            JournalDetailsList = journalDetails;
            foreach (var item in JournalDetailsList)
            {
                string timeWatched = item.MovieRuntime.Replace(" min", "");
                TotalTimeWatched = TotalTimeWatched + Int32.Parse(timeWatched);

            }
        }

    }
}
