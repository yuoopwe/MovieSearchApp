using FunctionZero.MvvmZero;
using MovieSearchApp.Mvvm.Pages;
using MovieSearchApp.Services.Rest;
using SimpleInjector;
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

        }
        private IPageServiceZero GetPageService()
        {
            var pageService = new PageServiceZero(() => App.Current.MainPage.Navigation, (theType) => _iocc.GetInstance(theType));
            return pageService;
        }
    }
}