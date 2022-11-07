﻿using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Identity.Client;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

namespace MAUI.MSALClient
{
    /// <summary>
    /// Contains methods to initialize and call the various MS Graph SDK methods
    /// </summary>
    /// <autogeneratedoc />
    public class MSGraphHelper
    {
        public readonly MSGraphApiConfig MSGraphApiConfig;

        public MSALClientHelper MSALClient { get; }
        private GraphServiceClient _graphServiceClient;

        private string[] GraphScopes;
        private string MSGraphBaseUrl = "https://graph.microsoft.com/v1.0";

        public MSGraphHelper(MSALClientHelper msalClientHelper, string embeddedConfigFileName = "MauiAppWithBroker.appsettings.json")
        {
            if (msalClientHelper == null)
            {
                throw new ArgumentNullException(nameof(msalClientHelper));
            }

            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(embeddedConfigFileName);
            IConfiguration AppConfiguration = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            this.MSGraphApiConfig = AppConfiguration.GetSection("MSGraphApi").Get<MSGraphApiConfig>();

            this.MSALClient = msalClientHelper;
            this.GraphScopes = this.MSGraphApiConfig.ScopesArray;
            this.MSGraphBaseUrl = this.MSGraphApiConfig.MSGraphBaseUrl;
        }

        /// <summary>
        /// Calls the MS Graph /me endpoint
        /// </summary>
        /// <returns></returns>
        public async Task<User> GetMeAsync()
        {
            if (this._graphServiceClient == null)
            {
                await SignInAndInitializeGraphServiceClient();
            }

            User graphUser = null;

            // Call /me Api

            try
            {
                graphUser = await _graphServiceClient.Me.GetAsync();
            }
            catch (ServiceException ex) when (ex.Message.Contains("Continuous access evaluation resulted in claims challenge"))
            {
                this._graphServiceClient = await SignInAndInitializeGraphServiceClientPostCAE(ex);

                // Call the /me endpoint of Graph again with a fresh token
                graphUser = await _graphServiceClient.Me.GetAsync();
            }
            return graphUser;
        }

        /// <summary>
        /// Calls the MS Graph /me/photo endpoint
        /// </summary>
        /// <returns></returns>
        public async Task<Stream> GetMyPhotoAsync()
        {
            if (this._graphServiceClient == null)
            {
                await SignInAndInitializeGraphServiceClient();
            }

            Stream userPhoto = null;

            // Call /me/Photo Api

            try
            {
                userPhoto = await _graphServiceClient.Me.Photo.Content.GetAsync();
            }
            catch (ServiceException ex) when (ex.Message.Contains("Continuous access evaluation resulted in claims challenge"))
            {
                this._graphServiceClient = await SignInAndInitializeGraphServiceClientPostCAE(ex);

                // Call the /me endpoint of Graph again with a fresh token
                userPhoto = await _graphServiceClient.Me.Photo.Content.GetAsync();
            }
            return userPhoto;
        }

        //public async Task<T> CallMSGraph<T>(IBaseRequestBuilder baseRequestBuilder)
        //{
        //    if (this._graphServiceClient == null)
        //    {
        //        await SignInAndInitializeGraphServiceClient();
        //    }

        //    T dataToReturn = null;

        //    // Call /me Api

        //    try
        //    {
        //        dataToReturn = await baseRequestBuilder.GetAsync();
        //    }
        //    catch (ServiceException ex) when (ex.Message.Contains("Continuous access evaluation resulted in claims challenge"))
        //    {
        //        this._graphServiceClient = await SignInAndInitializeGraphServiceClientPostCAE(ex);

        //        // Call the /me endpoint of Graph again with a fresh token
        //        dataToReturn = await baseRequestBuilder.GetAsync();
        //    }
        //    return dataToReturn;
        //}

        /// <summary>
        /// Sign in user using MSAL and obtain a token for MS Graph
        /// </summary>
        /// <returns>GraphServiceClient</returns>
        private async Task<GraphServiceClient> SignInAndInitializeGraphServiceClient()
        {
            string token = await this.MSALClient.SignInUserAndAcquireAccessToken(this.GraphScopes);
            return await InitializeGraphServiceClientAsync(token);
        }

        /// <summary>
        /// Signs the in and initialize graph service client post a CAE event exception.
        /// </summary>
        /// <param name="ex">The Graph Service exception. Contains the header required to properly process a CAE event</param>
        /// <returns></returns>
        private async Task<GraphServiceClient> SignInAndInitializeGraphServiceClientPostCAE(ServiceException ex)
        {
            // Get challenge from response of Graph API
            var claimChallenge = WwwAuthenticateParameters.GetClaimChallengeFromResponseHeaders(ex.ResponseHeaders);

            string token = await this.MSALClient.SignInUserAndAcquireAccessToken(this.GraphScopes, claimChallenge);
            return await InitializeGraphServiceClientAsync(token);
        }

        /// <summary>
        /// Bootstraps the MS Graph SDK with the provided token and returns it for use
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>
        /// A GraphServiceClient (MS Graph SDK) instance
        /// </returns>
        private async Task<GraphServiceClient> InitializeGraphServiceClientAsync(string token)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            this._graphServiceClient = new GraphServiceClient(client);

            return await Task.FromResult(this._graphServiceClient);
        }
    }
}