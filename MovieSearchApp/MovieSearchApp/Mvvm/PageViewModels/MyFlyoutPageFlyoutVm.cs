using FunctionZero.CommandZero;
using FunctionZero.MvvmZero;
using MovieSearchApp.Models;
using MovieSearchApp.Mvvm.Pages;
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
        public ICommand PopularPageCommand { get; }

        public ObservableCollection<FlyoutMenuModel> MenuItems { get; set; }
        public MyFlyoutPageFlyoutVm(OmdbService omdbService, IPageServiceZero pageService, IAlertService alertService)
        {
            _omdbService = omdbService;
            _pageService = pageService;
            _alertService = alertService;

            PopularPageCommand = new CommandBuilder().SetExecuteAsync(PopularPageExecute).Build();
            SearchPageCommand = new CommandBuilder().SetExecuteAsync(SearchPageExecute).Build();
            tgBtn = ImageSource.FromResource("MovieSearchApp.Images.menu96.png");
            MenuItems = new ObservableCollection<FlyoutMenuModel>(new[]
            {
                    new FlyoutMenuModel { Id = 0, Title = "Search" , Command=SearchPageCommand, Icon=ImageSource.FromResource("MovieSearchApp.Images.search1.png")},
                    new FlyoutMenuModel { Id = 1, Title = "Popular Movies", Command=PopularPageCommand, Icon=ImageSource.FromResource("MovieSearchApp.Images.popular.png") },
                    new FlyoutMenuModel { Id = 2, Title = "Journal",Icon=ImageSource.FromResource("MovieSearchApp.Images.Journal.png") },
                    new FlyoutMenuModel { Id = 3, Title = "Profile" ,Icon=ImageSource.FromResource("MovieSearchApp.Images.profile1.png")},
                    new FlyoutMenuModel { Id = 4, Title = "Settings",Icon=ImageSource.FromResource("MovieSearchApp.Images.Settings1.png") },
                    new FlyoutMenuModel { Id = 5, Title = "Sign out",Icon=ImageSource.FromResource("MovieSearchApp.Images.signout.png") },

                });

        }

        public async Task SearchPageExecute()
        {
            await _pageService.PushPageAsync<SearchPage, SearchPageVm>((vm) => { });
        }
        public async Task PopularPageExecute()
        {
            await _pageService.PushPageAsync<PopularPage, PopularPageVm>((vm) => { });
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



    }
}
