using FunctionZero.CommandZero;
using FunctionZero.MvvmZero;
using MovieSearchApp.Models;
using MovieSearchApp.Models.UserAccount;
using MovieSearchApp.Mvvm.Pages;
using MovieSearchApp.Mvvm.Pages.PopularPage;
using MovieSearchApp.Mvvm.PageViewModels;
using MovieSearchApp.Services;
using MovieSearchApp.Services.Alert_Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


namespace MovieSearchApp.Mvvm.PageViewModels
{
    class MyFlyoutPageFlyoutVm : MvvmZeroBaseVm
    {
        public OmdbService _omdbService;
        public IPageServiceZero _pageService;
        public IAlertService _alertService;
        public ImageSource tgBtn { get; set; }
        private FlyoutMenuModel _selectedDetailItem;
        public AccountDetailsModel AccountDetails { get; set; }
        public List<JournalDetailsModel> JournalDetailsList { get; set; }
        public FlyoutMenuModel SelectedDetailItem { get => _selectedDetailItem; 
            set
            {
                if (_selectedDetailItem != value)
                {
                    _selectedDetailItem = value;
                    this.OnPropertyChanged();
                }
            }
        }
        public ICommand SearchPageCommand { get; }
        public ICommand LoginPageCommand { get; }
        public ICommand ProfilePageCommand { get; }
        public ICommand PopularPageCommand { get; }
        public ICommand JournalPageCommand { get; }
        public ICommand SettingsPageCommand { get; }



        public ObservableCollection<FlyoutMenuModel> MenuItems { get; set; }
        public MyFlyoutPageFlyoutVm(OmdbService omdbService, IPageServiceZero pageService, IAlertService alertService)
        {
            _omdbService = omdbService;
            _pageService = pageService;
            _alertService = alertService;
          
            LoginPageCommand = new CommandBuilder().SetExecuteAsync(LoginPageExecute).Build();
            PopularPageCommand = new CommandBuilder().SetExecuteAsync(PopularPageExecute).Build();
            SearchPageCommand = new CommandBuilder().SetExecuteAsync(SearchPageExecute).Build();
            ProfilePageCommand = new CommandBuilder().SetExecuteAsync(ProfilePageExecute).Build();
            JournalPageCommand = new CommandBuilder().SetExecuteAsync(JournalPageExecute).Build();
            SettingsPageCommand = new CommandBuilder().SetExecuteAsync(SettingsPageExecute).Build();

            AccountDetails = new AccountDetailsModel();
            AccountDetails.IsLoggedIn = false;


            tgBtn = ImageSource.FromResource("MovieSearchApp.Images.menu96.png");
            MenuItems = new ObservableCollection<FlyoutMenuModel>(new[]
            {
                    new FlyoutMenuModel { Id = 0, Title = "Search" , Command=SearchPageCommand, Icon=ImageSource.FromResource("MovieSearchApp.Images.search1.png")},
                    new FlyoutMenuModel { Id = 1, Title = "Popular Movies", Command=PopularPageCommand, Icon=ImageSource.FromResource("MovieSearchApp.Images.popular.png") },
                    new FlyoutMenuModel { Id = 2, Title = "Journal", Command=JournalPageCommand, Icon=ImageSource.FromResource("MovieSearchApp.Images.Journal.png") },
                    new FlyoutMenuModel { Id = 3, Title = "Profile" , Command=ProfilePageCommand, Icon=ImageSource.FromResource("MovieSearchApp.Images.profile1.png")},
                    new FlyoutMenuModel { Id = 4, Title = "Settings", Command=SettingsPageCommand, Icon=ImageSource.FromResource("MovieSearchApp.Images.Settings1.png") },
                    new FlyoutMenuModel { Id = 5, Title = "Sign out", Command=LoginPageCommand, Icon=ImageSource.FromResource("MovieSearchApp.Images.signout.png") },

                });

        }

        public async Task SearchPageExecute()
        {
            await _pageService.PopToRootAsync();
            await _pageService.PushPageAsync<SearchPage, SearchPageVm>((vm) => vm.Init(AccountDetails, JournalDetailsList));
        }

        public async Task ProfilePageExecute()
        {
            if (AccountDetails.IsLoggedIn == false)
            {
                await _alertService.DisplayAlertAsync("Not Logged In", "You must login to access this page", "Ok");
            }
            else
            {
                await _pageService.PopToRootAsync();
                await _pageService.PushPageAsync<ProfilePage, ProfilePageVm>((vm) => vm.Init(AccountDetails, JournalDetailsList));
            }
            
        }
        public async Task PopularPageExecute()
        {
            await _pageService.PopToRootAsync();
            await _pageService.PushPageAsync<PopularPage, PopularPageVm>((vm) =>  vm.Init(AccountDetails, JournalDetailsList) );
        }
        public async Task SettingsPageExecute()
        {
            await _pageService.PopToRootAsync();
            await _pageService.PushPageAsync<SettingsPage, SettingsPageVm>((vm) => { });
        }
        public async Task JournalPageExecute()
        {
            if ( AccountDetails.IsLoggedIn == false)
            {
                await _alertService.DisplayAlertAsync("Not Logged In", "You must login to access this page", "Ok");
            }
            else
            {
                await _pageService.PopToRootAsync();
                await _pageService.PushPageAsync<JournalPage, JournalPageVm>((vm) => vm.Init(JournalDetailsList, AccountDetails));
            }
           
        }

        public async Task LoginPageExecute()
        {
            await _pageService.PopToRootAsync();
            await _pageService.PushPageAsync<LoginPage, LoginPageVm>((vm) => { });
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(SelectedDetailItem))
            {
                if (SelectedDetailItem != null)
                {
                    SelectedDetailItem.Command.Execute(null);
                    SelectedDetailItem = null;
                }
            }
        }

        public void SetAccountDetails(AccountDetailsModel AccountDetail, List<JournalDetailsModel> JournalList)
        {
            AccountDetails = AccountDetail;
            JournalDetailsList = JournalList;
        }

    }
}
