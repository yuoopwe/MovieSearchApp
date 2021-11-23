using FunctionZero.CommandZero;
using FunctionZero.MvvmZero;
using MovieSearchApp.Services.Alert_Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Configuration;
using MovieSearchApp.Models;

namespace MovieSearchApp.Mvvm.PageViewModels
{
    class LoginPageVm : MvvmZeroBaseVm
    {
        private string _passwordText;
        private string _usernameText;
        private readonly MyFlyoutPageFlyoutVm _flyoutVm;
        private readonly IAlertService _alertService;
        private readonly IPageServiceZero _pageService;
        private bool _loggedIn;
        private bool _signedOut;

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

        public LoginPageVm(IPageServiceZero pageService, IAlertService alertService, MyFlyoutPageFlyoutVm flyoutVm)
        {
            
            _alertService = alertService;
            _pageService = pageService;
            _flyoutVm = flyoutVm;
            LoggedIn = false;
            SignedOut = true;
            RegisterCommand = new CommandBuilder().SetExecuteAsync(RegisterExecute).Build();
            LoginCommand = new CommandBuilder().SetExecuteAsync(LoginExecute).Build();
            SignoutCommand = new CommandBuilder().SetExecuteAsync(SignoutExecute).Build();
        }

        public async Task SignoutExecute()
        {
            await _alertService.DisplayAlertAsync("Success", "You Have Signed Out", "Ok");
            SignedOut = true;
            LoggedIn = false;
            AccountDetails = null;
        }

        public async Task RegisterExecute()
        {
            SqlDataReader reader;
            SqlCommand command = new SqlCommand();
            string connectionString = ConfigurationManager.ConnectionStrings["Test"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            //Check account is made
            connection.Open();
            command.Connection = connection;
            command.CommandText = $"Select * from dbo.LoginTable WHERE username = '{UsernameText}'";
            SqlDataReader dr = command.ExecuteReader();
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
                command.CommandText = $"INSERT INTO[dbo].[LoginTable] (username, password) VALUES('{UsernameText}', '{PasswordText}')";
                command.ExecuteReader();

                connection.Close();
                connection.Open();


                //Check account is made
                command.CommandText = $"Select * from dbo.LoginTable WHERE username = '{UsernameText}'";
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (UsernameText == reader["username"].ToString().Trim())
                    {
                        connection.Close();
                        connection.Open();
                        command.CommandText = $"CREATE TABLE {UsernameText}Journal (MovieID varchar(255), MovieTitle varchar(255), MovieRating varchar(10), MovieComments varchar(255), MovieRuntime varchar(255));";
                        command.ExecuteReader();
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
            AccountDetails = new AccountDetailsModel();
            SqlConnection connection = new SqlConnection();
            SqlCommand command = new SqlCommand();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Test"].ConnectionString;
            connection.Open();
            command.Connection = connection;
            command.CommandText = $"Select * from dbo.LoginTable WHERE username = '{UsernameText}'";
            SqlDataReader dr = command.ExecuteReader();
            if (dr.Read())
            {
                if (UsernameText == dr["username"].ToString().Trim() && PasswordText == dr["password"].ToString().Trim())
                {
                    AccountDetails.Id = Convert.ToInt32(dr["id"]);
                    AccountDetails.ProfileName = dr["profile_name"].ToString().Trim();
                    AccountDetails.ProfileDescription = dr["profile_description"].ToString().Trim();
                    _flyoutVm.SetAccountDetails(AccountDetails);
                    await _alertService.DisplayAlertAsync("Success", "You have logged in Successfully", "Ok");
                    SignedOut = false;
                    LoggedIn = true;
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
