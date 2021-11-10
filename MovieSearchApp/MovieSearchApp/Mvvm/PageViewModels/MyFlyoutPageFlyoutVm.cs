using FunctionZero.CommandZero;
using FunctionZero.MvvmZero;
using MovieSearchApp.Mvvm.Pages;
using MovieSearchApp.Mvvm.PageViewModels;
using MovieSearchApp.Services;
using MovieSearchApp.Services.Alert_Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MovieSearchApp.Mvvm.PageViewModels
{
    class MyFlyoutPageFlyoutVm : MvvmZeroBaseVm
    {
        public OmdbService _omdbService;
        public IPageServiceZero _pageService;
        public IAlertService _alertService;
        public ICommand SearchPageCommand { get; }
        public ObservableCollection<MyFlyoutPageFlyoutMenuItem> MenuItems { get; set; }
        public MyFlyoutPageFlyoutVm(OmdbService omdbService, IPageServiceZero pageService, IAlertService alertService)
        {
            _omdbService = omdbService;
            _pageService = pageService;
            _alertService = alertService;

            SearchPageCommand = new CommandBuilder().SetExecuteAsync(SearchPageExecute).Build();
            MenuItems = new ObservableCollection<MyFlyoutPageFlyoutMenuItem>(new[]
            {
                    new MyFlyoutPageFlyoutMenuItem { Id = 0, Title = "" , Command=SearchPageCommand, Icon="Assets/search.png"},
                    new MyFlyoutPageFlyoutMenuItem { Id = 1, Title = "Page 2" },
                    new MyFlyoutPageFlyoutMenuItem { Id = 2, Title = "Page 3" },
                    new MyFlyoutPageFlyoutMenuItem { Id = 3, Title = "Page 4" },
                    new MyFlyoutPageFlyoutMenuItem { Id = 4, Title = "Page 5" },
                });

        }

        public async Task SearchPageExecute()
        {
            await _pageService.PushPageAsync<MovieDetailsPage, MovieDetailsPageVM>((vm) => { });
        }

       

    }
}
