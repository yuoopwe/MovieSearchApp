
using FunctionZero.MvvmZero;
using SimpleInjector;
using MovieSearchApp.Mvvm.Pages;
using MovieSearchApp.Mvvm.PageViewModels;
using MovieSearchApp.Services;
using MovieSearchApp.Services.Rest;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;
using MovieSearchApp.Services.Alert_Service;
using System;
using MovieSearchApp.Mvvm.Pages.PopularPage;
using MovieSearchApp.Mvvm.Pages.PopularPageFolder;

namespace MovieSearchApp.Boilerplate
{
    /// <summary>
    /// 
    /// </summary>
    public class Locator
	{
		private Container _IoCC;

		internal Locator(Application currentApplication)
		{
			// Create the IoC container that will contain all our configurable classes ...
			_IoCC = new Container();

			// Tell the IoC container what to do if asked for an IPageService
			_IoCC.Register<IPageServiceZero>(
				() =>
				{
					var pageService = new PageServiceZero(() => ((FlyoutPage)App.Current.MainPage).Detail.Navigation, (theType) => _IoCC.GetInstance(theType));
					return pageService;                   //() => App.Current.MainPage.Navigation   
				},
				Lifestyle.Singleton
			);

			// Tell the IoC container about our Pages.
			_IoCC.Register<SearchPage>(Lifestyle.Singleton);
			_IoCC.Register<MovieDetailsPage>(Lifestyle.Singleton);
			_IoCC.Register<MyFlyoutPageFlyout>(Lifestyle.Singleton);
			_IoCC.Register<RecommendationPage>(Lifestyle.Singleton);
			_IoCC.Register<PopularPage>(Lifestyle.Singleton);
			_IoCC.Register<GenreCheckbox>(Lifestyle.Singleton);
			_IoCC.Register<TrailerPage>(Lifestyle.Singleton);
			_IoCC.Register<LoginPage>(Lifestyle.Singleton);
			_IoCC.Register<ProfilePage>(Lifestyle.Singleton);
			_IoCC.Register<JournalPage>(Lifestyle.Singleton);
			_IoCC.Register<SettingsPage>(Lifestyle.Singleton);
			_IoCC.Register<AddToListPage>(Lifestyle.Singleton);
			_IoCC.Register<EditJournalItemPage>(Lifestyle.Singleton);
			_IoCC.Register<LandingPage>(Lifestyle.Singleton);






			// Tell the IoC container about our ViewModels.
			_IoCC.Register<MyFlyoutPageFlyoutVm>(Lifestyle.Singleton);
			_IoCC.Register<SearchPageVm>(Lifestyle.Singleton);
			_IoCC.Register<MovieDetailsPageVM>(Lifestyle.Singleton);
			_IoCC.Register<RecommendationPageVm>(Lifestyle.Singleton);
			_IoCC.Register<PopularPageVm>(Lifestyle.Singleton);
			_IoCC.Register<GenreCheckboxVm>(Lifestyle.Singleton);
			_IoCC.Register<TrailerPageVm>(Lifestyle.Singleton);
			_IoCC.Register<LoginPageVm>(Lifestyle.Singleton);
			_IoCC.Register<ProfilePageVm>(Lifestyle.Singleton);
			_IoCC.Register<JournalPageVm>(Lifestyle.Singleton);
			_IoCC.Register<SettingsPageVm>(Lifestyle.Singleton);
			_IoCC.Register<AddToListPageVm>(Lifestyle.Singleton);
			_IoCC.Register<EditJournalItemPageVm>(Lifestyle.Singleton);
			_IoCC.Register<LandingPageVm>(Lifestyle.Singleton);







			// Tell the IoC container about our Services!!!.
			_IoCC.Register<IRestService>(GetRestService, Lifestyle.Singleton);
			_IoCC.Register<OmdbService>(GetOmdbService, Lifestyle.Singleton);
			_IoCC.Register<IAlertService>(GetAlertService, Lifestyle.Singleton);
			_IoCC.Register<TastediveService>(GetTastediveService, Lifestyle.Singleton);
			_IoCC.Register<ThemoviedbService>(GetTheMovieDbService, Lifestyle.Singleton);
			_IoCC.Register<PasswordHasherService>(GetPasswordHasherService, Lifestyle.Singleton);
			_IoCC.Register<KeyVaultService>(GetKeyVaultService, Lifestyle.Singleton);
			


		}

        private IAlertService GetAlertService()
        {
			return new AlertService();
        }

        private OmdbService GetOmdbService()
		{
			return new OmdbService(_IoCC.GetInstance<IRestService>(), ApiConstants.OmdbBaseApiUrl, _IoCC.GetInstance<KeyVaultService>());
		}
		private ThemoviedbService GetTheMovieDbService()
		{
			return new ThemoviedbService(_IoCC.GetInstance<IRestService>(),ApiConstants.TheMovieDbBaseApiUrl, _IoCC.GetInstance<KeyVaultService>());
		}
		private TastediveService GetTastediveService()
		{
			return new TastediveService(_IoCC.GetInstance<IRestService>(),ApiConstants.TastediveBaseApiUrl, _IoCC.GetInstance<KeyVaultService>());
		}
		private PasswordHasherService GetPasswordHasherService()
		{
			return new PasswordHasherService(_IoCC.GetInstance<IRestService>());
		}
		private KeyVaultService GetKeyVaultService()
		{
			return new KeyVaultService(_IoCC.GetInstance<IRestService>());
		}



		private RestService GetRestService()
		{
			var httpClient = new HttpClient();
			return new RestService(httpClient);
		}



		internal async Task SetFirstPage()
		{
		
			var pageService = _IoCC.GetInstance<IPageServiceZero>();

			var flyoutPageFlyout = pageService.MakePage<MyFlyoutPageFlyout, MyFlyoutPageFlyoutVm>((vm) => { });

			var flyout = new FlyoutPage();

			flyout.Flyout = flyoutPageFlyout;

			var navPage = new NavigationPage();

			flyout.Detail = navPage;

			App.Current.MainPage = flyout;
			
			await _IoCC.GetInstance<IPageServiceZero>().PushPageAsync<LandingPage, LandingPageVm>((vm) => { });

		}




	}
}