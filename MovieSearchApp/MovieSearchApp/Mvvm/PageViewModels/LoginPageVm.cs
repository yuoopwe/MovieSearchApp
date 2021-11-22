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
using Npgsql;

namespace MovieSearchApp.Mvvm.PageViewModels
{
    class LoginPageVm : MvvmZeroBaseVm
    {
        private string _passwordText;
        private string _usernameText;
        private readonly IAlertService _alertService;
        private readonly IPageServiceZero _pageService;

        public ICommand RegisterCommand { get; }
        public ICommand LoginCommand { get; }
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

        public LoginPageVm(IPageServiceZero pageService, IAlertService alertService )
        {
            _alertService = alertService;
            _pageService = pageService;
            RegisterCommand = new CommandBuilder().SetExecuteAsync(RegisterExecute).Build();
            LoginCommand = new CommandBuilder().SetExecuteAsync(LoginExecute).Build();
        }

        

        public async Task RegisterExecute()
        {
            SqlDataReader reader;
            SqlCommand command;
            string queryString;
            //string connectionString = "Data Source=moviesearchapp.database.windows.net;Initial Catalog=userDB;User ID=yuoopwe;Password=BeeliveryBoys001";
            string connectionString = ConfigurationManager.ConnectionStrings["Test"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Check account is made
                SqlConnection sqlConnection = new SqlConnection();
                SqlCommand sqlCommand = new SqlCommand();
                sqlConnection.ConnectionString = @"Data Source=moviesearchapp.database.windows.net;Initial Catalog=userDB;User ID=yuoopwe;Password=BeeliveryBoys001";
                sqlConnection.Open();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = $"Select * from dbo.LoginTable WHERE username = '{UsernameText}'";
                SqlDataReader dr = sqlCommand.ExecuteReader();
                if (dr.Read())
                {
 
                    if (UsernameText == dr["username"].ToString().Trim())
                    {
                        await _alertService.DisplayAlertAsync("Success", "Username Already Exists", "Ok");
                    }
                    
                }
                else
                {

                    // create account
                    queryString = $"INSERT INTO[dbo].[LoginTable] (username, password) VALUES('{UsernameText}', '{PasswordText}')";
                    command = new SqlCommand(queryString, connection);

                    connection.Open();
                    reader = command.ExecuteReader();
                    reader.Close();

                    //Check account is made
                    queryString = $"Select * from dbo.LoginTable WHERE username = '{UsernameText}'";
                    command = new SqlCommand(queryString, connection);
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        if (UsernameText == reader["username"].ToString().Trim())
                        {
                            await _alertService.DisplayAlertAsync("Success", "Account creation successful", "Ok");
                        }
                    }
                    else
                    {
                        await _alertService.DisplayAlertAsync("Success", "Account creation failed", "Ok");
                    }

                    connection.Close();
                }
                sqlConnection.Close();




            }

            return;
        }
        public async Task LoginExecute()
        {
            SqlConnection sqlConnection = new SqlConnection();
            SqlCommand sqlCommand = new SqlCommand();
            sqlConnection.ConnectionString = @"Data Source=moviesearchapp.database.windows.net;Initial Catalog=userDB;User ID=yuoopwe;Password=BeeliveryBoys001";
            sqlConnection.Open();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = $"Select * from dbo.LoginTable WHERE username = '{UsernameText}'";
            SqlDataReader dr = sqlCommand.ExecuteReader();
            if (dr.Read())
            {
                string yes = dr["username"].ToString();
                if (UsernameText == dr["username"].ToString().Trim() && PasswordText == dr["password"].ToString().Trim())
                {
                     await _alertService.DisplayAlertAsync("Success", "You have logged in Successfully", "Ok");
                }
                else
                {
                 await _alertService.DisplayAlertAsync("Success", "Login failed, please try again", "Ok");
                }
            }
            sqlConnection.Close();
            return;

        }
     

    }
}
