using MovieSearchApp.Models;
using MovieSearchApp.Models.UserAccount;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearchApp.Mvvm.PageViewModels
{
    class SettingsPageVm
    {
        public AccountDetailsModel AccountDetails { get; set; }
        public List<JournalDetailsModel> JournalDetailsList { get; set; }


    }
}
