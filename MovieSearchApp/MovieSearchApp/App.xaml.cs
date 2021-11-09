using MovieSearchApp.Boilerplate;
using System;
using Xamarin.Forms;
using MovieSearchApp.Services;
using Xamarin.Forms.Xaml;


namespace MovieSearchApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var locator = new Locator(this);

            _ = locator.SetFirstPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
