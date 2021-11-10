using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MovieSearchApp.Models
{
    public class FlyoutMenuModel
    {
        public FlyoutMenuModel()
        {
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public ImageSource Icon { get; set; }
        public ICommand Command { get; set; }
    
    }
}