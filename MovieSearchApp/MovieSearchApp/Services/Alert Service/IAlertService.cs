using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearchApp.Services.Alert_Service
{
    public interface IAlertService
    {
        Task DisplayAlertAsync(string title, string message, string cancel);

        Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel);

        Task DisplayActionSheetAsync(string title, string cancel, string destruction, params string[] buttons);
    }
}
