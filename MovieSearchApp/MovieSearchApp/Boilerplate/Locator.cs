
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
					// This is how we create an instance of PageServiceZero.
					// The PageService needs to know how to get the current NavigationPage it is to interact with.
					// (If you have a FlyoutPage at the root, the navigationGetter should return the current Detail item)
					// It also needs to know how to get Page and ViewModel instances so we provide it with a factory
					// that uses the IoC container. We could easily provide any sort of factory, we don't need to use an IoC container.
					var pageService = new PageServiceZero(() => ((FlyoutPage)App.Current.MainPage).Detail.Navigation, (theType) => _IoCC.GetInstance(theType));
					return pageService;                   //() => App.Current.MainPage.Navigation   
				},
				// One only ever will be created.
				Lifestyle.Singleton
			);

			// Tell the IoC container about our Pages.
			_IoCC.Register<SearchPage>(Lifestyle.Singleton);
			_IoCC.Register<MovieDetailsPage>(Lifestyle.Singleton);
			_IoCC.Register<MyFlyoutPageFlyout>(Lifestyle.Singleton);
			_IoCC.Register<RecommendationPage>(Lifestyle.Singleton);
			_IoCC.Register<PopularPage>(Lifestyle.Singleton);
			_IoCC.Register<GenreCheckbox>(Lifestyle.Singleton);



			// Tell the IoC container about our ViewModels.
			_IoCC.Register<MyFlyoutPageFlyoutVm>(Lifestyle.Singleton);
			_IoCC.Register<SearchPageVm>(Lifestyle.Singleton);
			_IoCC.Register<MovieDetailsPageVM>(Lifestyle.Singleton);
			_IoCC.Register<RecommendationPageVm>(Lifestyle.Singleton);
			_IoCC.Register<PopularPageVm>(Lifestyle.Singleton);
			_IoCC.Register<GenreCheckboxVm>(Lifestyle.Singleton);


			// Tell the IoC container about our Services!!!.
			_IoCC.Register<IRestService>(GetRestService, Lifestyle.Singleton);
			_IoCC.Register<OmdbService>(GetOmdbService, Lifestyle.Singleton);
			_IoCC.Register<IAlertService>(GetAlertService, Lifestyle.Singleton);
			_IoCC.Register<TastediveService>(GetTastediveService, Lifestyle.Singleton);
			_IoCC.Register<ThemoviedbService>(GetTheMovieDbService, Lifestyle.Singleton);
			



			// Optionally add more to the IoC conatainer, e.g. loggers, Http comms objects etc. E.g.
			// IoCC.Register<ILogger, MyLovelyLogger>(Lifestyle.Singleton);
		}

		private INavigation GetNavigationPage()
		{
			// => App.Current.MainPage.Navigation
			var flyout = (FlyoutPage)App.Current.MainPage.Navigation;


			var currentPage = flyout;

			var navPage = (INavigation)currentPage;

			return navPage;


		}

        private IAlertService GetAlertService()
        {
			return new AlertService();
        }

        private OmdbService GetOmdbService()
		{
			return new OmdbService(_IoCC.GetInstance<IRestService>(), ApiConstants.OmdbApiKey, ApiConstants.OmdbBaseApiUrl);
		}
		private ThemoviedbService GetTheMovieDbService()
		{
			return new ThemoviedbService(_IoCC.GetInstance<IRestService>(), ApiConstants.TheMovieDbApiKey, ApiConstants.TheMovieDbBaseApiUrl);
		}
		private TastediveService GetTastediveService()
		{
			return new TastediveService(_IoCC.GetInstance<IRestService>(), ApiConstants.TastediveApiKey, ApiConstants.TastediveBaseApiUrl);
		}


		private RestService GetRestService()
		{
			var httpClient = new HttpClient();
			return new RestService(httpClient);
		}



		/// <summary>
		/// This is called once during application startup
		/// </summary>
		internal async Task SetFirstPage()
		{
			// Create and assign a top-level NavigationPage.
			// If you use a FlyoutPage instead then its Detail item will need to be a NavigationPage
			// and you will need to modify the 'navigationGetter' provided to the PageServiceZero instance to 
			// something like this:
			// () => ((FlyoutPage)App.Current.MainPage).Detail.Navigation

		
			var pageService = _IoCC.GetInstance<IPageServiceZero>();

			var flyoutPageFlyout = pageService.MakePage<MyFlyoutPageFlyout, MyFlyoutPageFlyoutVm>((vm) => { });

			var flyout = new FlyoutPage();

			flyout.Flyout = flyoutPageFlyout;


		
			var navPage = new NavigationPage();
			flyout.Detail = navPage;
			App.Current.MainPage = flyout;
			


			await _IoCC.GetInstance<IPageServiceZero>().PushPageAsync<SearchPage, SearchPageVm>((vm) => { });

		}

		/// <summary>
		/// For debug purposes to let us know when a Page is assembled by the PageService
		/// </summary>
		/// <param name="thePage">A reference to the page that has been presented</param>
		private void PageCreated(Page thePage)
		{
			Debug.WriteLine(thePage);
		}
	}
}