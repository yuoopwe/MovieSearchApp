using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearchApp.Services.Alert_Service
{
    class AlertService : IAlertService
    {
        
        public AlertService()
        {
           
        }
        public async Task DisplayAlertAsync(string title, string message, string cancel)
        {
            await App.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public async Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel)
        {
            return await App.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }
        public async Task DisplayActionSheetAsync(string title, string cancel, string destruction, params string[] buttons) 
        { 
            await App.Current.MainPage.DisplayActionSheet(null, cancel, null, buttons);
        }
    }
}
