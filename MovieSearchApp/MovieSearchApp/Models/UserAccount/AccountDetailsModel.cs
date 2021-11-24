using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearchApp.Models
{
    class AccountDetailsModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string ProfileName { get; set; }
        public string ProfileDescription { get; set; }
        public bool IsLoggedIn { get; set; }

    }
}
