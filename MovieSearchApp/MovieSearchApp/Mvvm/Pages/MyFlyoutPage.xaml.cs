﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MovieSearchApp.Mvvm.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyFlyoutPage : FlyoutPage
    {
        public MyFlyoutPage()
        {
            InitializeComponent();
        }

    }
}