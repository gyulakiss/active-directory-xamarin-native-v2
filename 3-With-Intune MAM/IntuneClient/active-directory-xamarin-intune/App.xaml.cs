﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace active_directory_xamarin_intune
{
    public partial class App : Application
    {
        public App()
        {
            Dispatcher.BeginInvokeOnMainThread(() =>
            {
                InitializeComponent();

                MainPage = new MainPage();
            });
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
