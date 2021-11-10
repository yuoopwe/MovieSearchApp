using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MovieSearchApp.Mvvm.Pages
{
    public class MyFlyoutPageFlyoutMenuItem
    {
        public MyFlyoutPageFlyoutMenuItem()
        {
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public string Icon { get; set; }
        public ICommand Command { get; set; }
    }
}