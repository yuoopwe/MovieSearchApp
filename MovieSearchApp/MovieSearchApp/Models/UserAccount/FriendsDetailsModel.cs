using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearchApp.Models.UserAccount
{
    public class FriendsDetailsModel
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}

