
using FunctionZero.MvvmZero;
using SimpleInjector;
using MovieSearchApp.Mvvm.Pages;
using MovieSearchApp.Mvvm.PageViewModels;
using MovieSearchApp.Services;
using MovieSearchApp.Services.Rest;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MovieSearchApp.Boilerplate
{
/// <summary>
/// 
/// </summary>
    public class Locator
    {
        private Container _iocc;
        public Locator(Application currentApplication)
        {
            _iocc = new Container();

            _iocc.Register<IPageServiceZero>(GetPageService, Lifestyle.Singleton);
            _iocc.Register<SearchPage>(Lifestyle.Singleton);
            _iocc.Register<SearchPageVm>(Lifestyle.Singleton);

        }

        public async Task SetFirstPage()
        {
            App.Current.MainPage = new NavigationPage();
            await _iocc.GetInstance<IPageServiceZero>().PushPageAsync<SearchPage, SearchPageVm>((vm) => { }) ;
        }
        private IPageServiceZero GetPageService()
        {
            App.Current.MainPage = new NavigationPage();
            var pageService = new PageServiceZero(() => App.Current.MainPage.Navigation, (theType) => _iocc.GetInstance(theType));
            return pageService;
        }
    }
}