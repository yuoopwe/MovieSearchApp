using FunctionZero.CommandZero;
using FunctionZero.MvvmZero;
using MovieSearchApp.Services.Alert_Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Configuration;
using MovieSearchApp.Models;
using MovieSearchApp.Models.UserAccount;
using MovieSearchApp.Services;
using Xamarin.Forms;

namespace MovieSearchApp.Mvvm.PageViewModels
{
    class LoginPageVm : MvvmZeroBaseVm
    {
        private string _passwordText;
        private string _usernameText;
        private readonly MyFlyoutPageFlyoutVm _flyoutVm;
        private readonly KeyVaultService _keyVaultService;
        private readonly PasswordHasherService _passwordHasher;
        private readonly IAlertService _alertService;
        private readonly IPageServiceZero _pageService;
        private bool _loggedIn;
        private bool _signedOut;
        private bool _isPassword;
        private bool _isNotPassword;

        private ImageSource _eyeIconOpen;

        public bool IsPassword
        {
            get => _isPassword;
            set => SetProperty(ref _isPassword, value);
        }
        public bool IsNotPassword
        {
            get => _isNotPassword;
            set => SetProperty(ref _isNotPassword, value);
        }
        public bool LoggedIn
        {
            get => _loggedIn;
            set => SetProperty(ref _loggedIn, value);
        }

        public bool SignedOut
        {
            get => _signedOut;
            set => SetProperty(ref _signedOut, value);
        }
        public AccountDetailsModel AccountDetails { get; set; }
        public JournalDetailsModel JournalDetails { get; set; }
        public List<JournalDetailsModel> JournalDetailsList { get; set; }

        public ICommand ShowPasswordCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand LoginCommand { get; }
        public ICommand SignoutCommand { get; }

        public string PasswordText
        {
            get => _passwordText;
            set => SetProperty(ref _passwordText, value);
        }
        public string UsernameText
        {
            get => _usernameText;
            set => SetProperty(ref _usernameText, value);
        }

        public ImageSource EyeIconOpen { get; set; }
        public ImageSource EyeIconClosed { get; set; }

        public LoginPageVm(IPageServiceZero pageService, IAlertService alertService, MyFlyoutPageFlyoutVm flyoutVm, PasswordHasherService passwordHasher, KeyVaultService keyVaultService)
        {
            _keyVaultService = keyVaultService;
            _passwordHasher = passwordHasher;
            _alertService = alertService;
            _pageService = pageService;
            _flyoutVm = flyoutVm;
            IsPassword = true;
            IsNotPassword = false;
            LoggedIn = false;
            SignedOut = true;
            RegisterCommand = new CommandBuilder().SetExecuteAsync(RegisterExecute).Build();
            LoginCommand = new CommandBuilder().SetExecuteAsync(LoginExecute).Build();
            SignoutCommand = new CommandBuilder().SetExecuteAsync(SignoutExecute).Build();
            ShowPasswordCommand = new CommandBuilder().SetExecuteAsync(ShowPasswordExecute).Build();
            EyeIconOpen = ImageSource.FromResource("MovieSearchApp.Images.eye-outline.png");
            EyeIconClosed = ImageSource.FromResource("MovieSearchApp.Images.eye-off-outline.png");

        }

        private async Task ShowPasswordExecute()
        {
            if(IsPassword == true)
            {
                IsPassword = false;
                IsNotPassword = true;
            }
            else
            {
                IsPassword = true;
                IsNotPassword = false;
            }
        }

        public async Task SignoutExecute()
        {
            await _alertService.DisplayAlertAsync("Success", "You Have Signed Out", "Ok");
            SignedOut = true;
            LoggedIn = false;
            IsPassword = true;
            IsNotPassword = false;
            AccountDetails = new AccountDetailsModel(); 
            AccountDetails.IsLoggedIn = false;
            JournalDetailsList.Clear();
            _flyoutVm.SetAccountDetails(AccountDetails, JournalDetailsList);
        }

        public async Task RegisterExecute()
        {
            var secrets = await _keyVaultService.GetKeysAsync();
            SqlDataReader reader;
            SqlCommand checkAccountCommand = new SqlCommand();
            SqlCommand makeAccountCommand = new SqlCommand();
            SqlCommand createJournalCommand = new SqlCommand();
            string connectionString = secrets.ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            //Check account is made
            connection.Open();
            checkAccountCommand.Connection = connection;
            checkAccountCommand.CommandText = $"Select * from dbo.LoginTable WHERE username = @Username";
            checkAccountCommand.Parameters.Add("@Username", SqlDbType.NVarChar);
            checkAccountCommand.Parameters["@Username"].Value = UsernameText;
            SqlDataReader dr = checkAccountCommand.ExecuteReader();
            if (dr.Read())
            {
 
                if (UsernameText == dr["username"].ToString().Trim())
                {
                    await _alertService.DisplayAlertAsync("Failure", "Username Already Exists", "Ok");
                    
                }
                dr.Close();
            }
            else
            {
                connection.Close();
                connection.Open();

                // create account
                makeAccountCommand.Connection = connection;
                makeAccountCommand.CommandText = $"INSERT INTO[dbo].[LoginTable] (username, password) VALUES(@Username, @Password)";
                makeAccountCommand.Parameters.Add("@Username", SqlDbType.NVarChar);
                makeAccountCommand.Parameters["@Username"].Value = UsernameText;
                // Hash password
                var newPassword = _passwordHasher.Hash(PasswordText);

                makeAccountCommand.Parameters.Add("@Password", SqlDbType.NVarChar);
                makeAccountCommand.Parameters["@Password"].Value = newPassword;
                makeAccountCommand.ExecuteReader();

                connection.Close();
                connection.Open();


                //Check account is made
                checkAccountCommand.CommandText = $"Select * from dbo.LoginTable WHERE username = @Username";
                reader = checkAccountCommand.ExecuteReader();
                if (reader.Read())
                {
                    if (UsernameText == reader["username"].ToString().Trim())
                    {
                        connection.Close();
                        connection.Open();
                        createJournalCommand.Connection = connection;
                        createJournalCommand.CommandText = $"CREATE TABLE {UsernameText}Journal (MovieID varchar(255) NOT NULL, MovieTitle varchar(255), MovieRating varchar(10), MovieComments varchar(255), MovieRuntime varchar(255), PRIMARY KEY (MovieID), MoviePoster varchar(255));";
                        createJournalCommand.ExecuteReader();
                        await _alertService.DisplayAlertAsync("Success", "Account creation successful", "Ok");
                        
                    }
                }
                else
                {
                    await _alertService.DisplayAlertAsync("Success", "Account creation failed", "Ok");
                }
                dr.Close();

            }
            connection.Close();


            return;
        }
        public async Task LoginExecute()
        {
            var secrets = await _keyVaultService.GetKeysAsync();
            JournalDetailsList = new List<JournalDetailsModel>();
            AccountDetails = new AccountDetailsModel();
            SqlConnection connection = new SqlConnection();
            SqlCommand checkAccountCommand = new SqlCommand();
            SqlCommand readJournalCommand = new SqlCommand();
            connection.ConnectionString = secrets.ConnectionString;
            connection.Open();
            checkAccountCommand.Connection = connection;
            checkAccountCommand.CommandText = $"Select * from dbo.LoginTable WHERE username = @Username";
            checkAccountCommand.Parameters.Add("@Username", SqlDbType.NVarChar);
            checkAccountCommand.Parameters["@Username"].Value = UsernameText;

            SqlDataReader dr = checkAccountCommand.ExecuteReader();
            if (dr.Read())
            {
                if (UsernameText == dr["username"].ToString().Trim() && _passwordHasher.Verify(PasswordText,dr["password"].ToString().Trim()))
                {
                    AccountDetails.Id = Convert.ToInt32(dr["id"]);
                    AccountDetails.ProfileName = dr["profile_name"].ToString().Trim();
                    AccountDetails.ProfileDescription = dr["profile_description"].ToString().Trim();
                    AccountDetails.Username = UsernameText.Trim();
                    AccountDetails.IsLoggedIn = true;
                    dr.Close();
                    connection.Close();
                    connection.Open();
                    readJournalCommand.Connection = connection;
                    readJournalCommand.CommandText = $"Select * from dbo.{UsernameText}Journal";
                    dr = readJournalCommand.ExecuteReader();
                    while (dr.Read())
                    {
                        JournalDetails = new JournalDetailsModel();
                        JournalDetails.MovieID = (string)dr["MovieID"];
                        JournalDetails.MovieTitle = (string)dr["MovieTitle"];
                        JournalDetails.MovieRating = (string)dr["MovieRating"];
                        JournalDetails.MovieComments = (string)dr["MovieComments"];
                        JournalDetails.MovieRuntime = (string)dr["MovieRuntime"];
                        JournalDetails.MoviePoster = (string)dr["MoviePoster"]; 
                        JournalDetailsList.Add(JournalDetails);
                        

                    }
                    _flyoutVm.SetAccountDetails(AccountDetails, JournalDetailsList);
                    await _alertService.DisplayAlertAsync("Success", "You have logged in Successfully", "Ok");
                    SignedOut = false;
                    LoggedIn = true;
                    IsPassword = false;
                    IsNotPassword = false;
                }
                else
                {
                    await _alertService.DisplayAlertAsync("Success", "Login failed, please try again", "Ok");
                }
            }

            connection.Close();
            return;

        }

      

    }
}
