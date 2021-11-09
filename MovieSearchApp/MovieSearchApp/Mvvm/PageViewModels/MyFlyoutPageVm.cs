using FunctionZero.MvvmZero;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearchApp.Mvvm.PageViewModels
{
    class MyFlyoutPageVm : MvvmZeroBaseVm
    {
        public IPageServiceZero _pageService;

        public MyFlyoutPageVm(IPageServiceZero pageService)
        {

            _pageService = pageService;

        }
    }
}
