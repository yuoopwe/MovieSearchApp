using FunctionZero.MvvmZero;
using MovieSearchApp.Services;
using MovieSearchApp.Services.Alert_Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearchApp.Mvvm.PageViewModels
{
    class MyFlyoutPageFlyoutVm 
    {
        public OmdbService _omdbService;
        public IPageServiceZero _pageService;
        public IAlertService _alertService;

        public MyFlyoutPageFlyoutVm(OmdbService omdbService, IPageServiceZero pageService, IAlertService alertService)
        {
            _omdbService = omdbService;
            _pageService = pageService;
            _alertService = alertService;


        }
    }
}
