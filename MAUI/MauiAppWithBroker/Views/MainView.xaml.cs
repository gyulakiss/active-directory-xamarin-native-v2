﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using MauiAppWithBroker.MSALClient;
using Microsoft.Identity.Client;

namespace MauiAppWithBroker.Views
{
    public partial class MainView : ContentPage
    {
        public MainView()
        {
            InitializeComponent();
        }

        private async void OnSignInClicked(object sender, EventArgs e)
        {
            try
            {
                AuthenticationResult result = await PCAWrapper.Instance.AcquireTokenSilentAsync(AppConstants.Scopes);
            }
            catch (MsalUiRequiredException)
            {
                // This executes UI interaction to obtain token
                AuthenticationResult result = await PCAWrapper.Instance.AcquireTokenInteractiveAsync(AppConstants.Scopes);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception during signin", ex.Message, "OK").ConfigureAwait(false);
                return;
            }

            await Shell.Current.GoToAsync("userview");
        }
        protected override bool OnBackButtonPressed() { return true; }

    }
}