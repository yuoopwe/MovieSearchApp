using FunctionZero.MvvmZero;
using MovieSearchApp.Services;
using MovieSearchApp.Services.Alert_Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearchApp.Mvvm.PageViewModels
{
    class LandingPageVm
    {
        private readonly IPageServiceZero _pageService;
        private readonly MyFlyoutPageFlyoutVm _flyoutVm;

        public LandingPageVm(IPageServiceZero pageService, IAlertService alertService, MyFlyoutPageFlyoutVm flyoutVm, OmdbService omdbService, KeyVaultService keyVaultService)
        {

            _pageService = pageService;
            _flyoutVm = flyoutVm;
      



        }
    }
}
