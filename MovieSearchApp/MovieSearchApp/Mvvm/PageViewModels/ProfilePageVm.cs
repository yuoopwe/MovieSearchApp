using FunctionZero.CommandZero;
using FunctionZero.MvvmZero;
using MovieSearchApp.Models;
using MovieSearchApp.Models.UserAccount;
using MovieSearchApp.Mvvm.Pages;
using MovieSearchApp.Services;
using MovieSearchApp.Services.Alert_Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using System.Collections.ObjectModel;

namespace MovieSearchApp.Mvvm.PageViewModels
{
    class ProfilePageVm : MvvmZeroBaseVm
    {

        private readonly KeyVaultService _keyVaultService;
        private readonly IAlertService _alertService;
        private readonly IPageServiceZero _pageService;
        private string _profileDescriptionText;
        private string _profileNameText;
        private string _totalTimeWatched;
        private string _newTotal;
        private string _searchText;
        private bool _otherUser;
        private bool _currentUser;
        private string _newFriendsListString;

        private ObservableCollection<FriendsDetailsModel> _FriendsObjectList; 
        public ObservableCollection<FriendsDetailsModel> FriendsObjectList
        {
            get => _FriendsObjectList;
            set => SetProperty(ref _FriendsObjectList, value);
        }
        public AccountDetailsModel SearchAccountDetails { get; set; }
        public List<JournalDetailsModel> SearchJournalDetailsList { get; set; }
        public JournalDetailsModel SearchJournalDetails { get; set; }

        public string NewFriendsListString
        {
            get => _newFriendsListString;
            set => SetProperty(ref _newFriendsListString, value);
        }
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

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public string ProfileDescriptionText
        {
            get => _profileDescriptionText;
            set => SetProperty(ref _profileDescriptionText, value);
        }
        public string TotalTimeWatched
        {
            get => _totalTimeWatched;
            set => SetProperty(ref _totalTimeWatched, value);
        }

        public string ProfileNameText
        {
            get => _profileNameText;
            set => SetProperty(ref _profileNameText, value);
        }
        public AccountDetailsModel DisplayAccountDetails { get; set; }
        public AccountDetailsModel AccountDetails { get; set; }

        public List<JournalDetailsModel> JournalDetailsList { get; set; }

        public ICommand ChangeProfileNameCommand { get; }
        public ICommand ChangeProfileDescriptionCommand { get; }

        public ICommand UserSearchCommand { get; }

        public ICommand AddFriendCommand { get; }


        public ProfilePageVm(IPageServiceZero pageService, IAlertService alertService, KeyVaultService keyVaultService)
        {
            _keyVaultService = keyVaultService;
            _alertService = alertService;
            _pageService = pageService;
            FriendsObjectList = new ObservableCollection<FriendsDetailsModel>();
            ChangeProfileNameCommand = new CommandBuilder().SetExecuteAsync(ChangeProfileNameExecute).Build();
            ChangeProfileDescriptionCommand = new CommandBuilder().SetExecuteAsync(ChangeProfileDescriptionExecute).Build();
            UserSearchCommand = new CommandBuilder().SetExecuteAsync(UserSearchExecute).Build();
            AddFriendCommand = new CommandBuilder().SetExecuteAsync(AddFriendExecute).Build(); 

        }

        #region CHANGING PROFILE NAME & CHANGING PROFILE DESCRIPTION
        public async Task ChangeProfileDescriptionExecute()
        {
            var secrets = await _keyVaultService.GetKeysAsync();
            SqlDataReader reader;
            SqlCommand command = new SqlCommand();
            string connectionString = secrets.ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            //Check account is made
            connection.Open();
            command.Connection = connection;
            command.CommandText = $"UPDATE [dbo].[LoginTable] Set profile_description = '{ProfileDescriptionText}' Where id = '{DisplayAccountDetails.Id}'; ";
            command.ExecuteReader();
            connection.Close();
            connection.Open();
            command.CommandText = $"Select * from dbo.LoginTable WHERE id = '{DisplayAccountDetails.Id}'";
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                if (ProfileDescriptionText.Trim() == reader["profile_description"].ToString().Trim())
                {
                    DisplayAccountDetails.ProfileDescription = ProfileDescriptionText;
                    await _alertService.DisplayAlertAsync("Success", "Profile Description Change Successful", "Ok");
                    DisplayAccountDetails.ProfileName = ProfileNameText;
                }
                else
                {
                    await _alertService.DisplayAlertAsync("Success", "Profile Description Change Failed", "Ok");

                }
            }

        }
        public async Task ChangeProfileNameExecute()
        {
            var secrets = await _keyVaultService.GetKeysAsync();
            SqlDataReader reader;
            SqlCommand command = new SqlCommand();
            string connectionString = secrets.ConnectionString;
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
        #endregion

        public async Task AddFriendExecute()
        {
            FriendsDetailsModel newFriend = new FriendsDetailsModel();
            newFriend.Name = SearchAccountDetails.ProfileName;
            FriendsObjectList.Add(newFriend); 

            NewFriendsListString = SearchAccountDetails.ProfileName;
            await _alertService.DisplayAlertAsync("Message", $"You have added {SearchAccountDetails.ProfileName} succesfully!", "Ok");

            
        }


        //Allows you to search for another users profile
        public async Task UserSearchExecute()
        {
            var secrets = await _keyVaultService.GetKeysAsync();
            SearchAccountDetails = new AccountDetailsModel();
            SearchJournalDetailsList = new List<JournalDetailsModel>();
            SqlDataReader reader;
            SqlCommand command = new SqlCommand();
            SqlCommand readJournalCommand = new SqlCommand();
            string connectionString = secrets.ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            command.Connection = connection;
            command.CommandText = $"Select * from dbo.LoginTable WHERE profile_name = @Username";
            command.Parameters.Add("@Username", SqlDbType.NVarChar);
            command.Parameters["@Username"].Value = SearchText;
            if(SearchText.Trim().ToLower() == AccountDetails.ProfileName.ToLower())
            {
                await _alertService.DisplayAlertAsync("Failure", "You cannot search your own profile name", "Ok");
                goto end;
            }
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                if (SearchText.ToLower() == reader["profile_name"].ToString().Trim().ToLower())
                {
                    SearchAccountDetails.Id = Convert.ToInt32(reader["id"]);
                    SearchAccountDetails.Username = (string)reader["username"];
                    SearchAccountDetails.ProfileName = reader["profile_name"].ToString().Trim();
                    SearchAccountDetails.ProfileDescription = reader["profile_description"].ToString().Trim();
                    SearchAccountDetails.FriendsListString = reader["FriendsListString"].ToString().Trim(); 
                    reader.Close();
                    connection.Close();
                    connection.Open();
                    readJournalCommand.Connection = connection;
                    readJournalCommand.CommandText = $"Select * from dbo.{SearchAccountDetails.Username}Journal";
                    reader = readJournalCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        SearchJournalDetails = new JournalDetailsModel();
                        SearchJournalDetails.MovieID = (string)reader["MovieID"];
                        SearchJournalDetails.MovieTitle = (string)reader["MovieTitle"];
                        SearchJournalDetails.MovieRating = (string)reader["MovieRating"];
                        SearchJournalDetails.MovieComments = (string)reader["MovieComments"];
                        SearchJournalDetails.MovieRuntime = (string)reader["MovieRuntime"];
                        SearchJournalDetails.MoviePoster = (string)reader["MoviePoster"];
                        SearchJournalDetailsList.Add(SearchJournalDetails);
                    }
                }
                reader.Close();
                connection.Close();
                OtherUserInit(SearchAccountDetails, SearchJournalDetailsList);
                await _alertService.DisplayAlertAsync("Success", "User found successfully", "Ok");

            }
            else
            {
                reader.Close();
                connection.Close();
                await _alertService.DisplayAlertAsync("Error", "This profile name does not exist", "Ok");
            }
            end:;

        }
        public void Init(AccountDetailsModel accountDetails, List<JournalDetailsModel> journalDetails)
        {
            CurrentUser = true;
            OtherUser = false;
            TotalTimeWatched = "";
            _newTotal = "";
            AccountDetails = accountDetails;
            DisplayAccountDetails = accountDetails;
            ProfileNameText = accountDetails.ProfileName;
            ProfileDescriptionText = accountDetails.ProfileDescription;
            JournalDetailsList = journalDetails;
            


            foreach (var item in JournalDetailsList)
            {
                
                string timeWatched = item.MovieRuntime.Replace(" min", "");
                if (timeWatched == "N/A")
                {
                  
                }
                else
                {
                    if (_newTotal != "") 
                        _newTotal = "" + (Int32.Parse(_newTotal) + Int32.Parse(timeWatched));
                    else
                        _newTotal = "" + Int32.Parse(timeWatched);
                }
                

            }
            TotalTimeWatched = $"Time Watched: {_newTotal} (mins)";

        }
        public void OtherUserInit(AccountDetailsModel accountDetails, List<JournalDetailsModel> journalDetails)
        {
            OtherUser = true;
            CurrentUser = false;
            TotalTimeWatched = "";
            _newTotal = "";
            DisplayAccountDetails = accountDetails;
            ProfileNameText = accountDetails.ProfileName;
            ProfileDescriptionText = accountDetails.ProfileDescription;
            


            JournalDetailsList = journalDetails;
            foreach (var item in JournalDetailsList)
            {

                string timeWatched = item.MovieRuntime.Replace(" min", "");
                if (timeWatched == "N/A")
                {

                }
                else
                {
                    if (_newTotal != "")
                        _newTotal = "" + (Int32.Parse(_newTotal) + Int32.Parse(timeWatched));
                    else
                        _newTotal = "" + Int32.Parse(timeWatched);
                }


            }
            TotalTimeWatched = $"Time Watched: {_newTotal} (mins)";

        }

    }
}
