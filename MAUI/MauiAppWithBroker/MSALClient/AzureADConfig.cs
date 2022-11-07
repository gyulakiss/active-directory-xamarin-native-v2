﻿using System.Collections.Generic;

namespace MAUI.MSALClient
{
    /// <summary>
    /// App settings, populated from appsettings.json file
    /// </summary>
    public class AzureADConfig
    {
        /// <summary>
        /// Gets or sets the Azure AD authority.
        /// </summary>
        /// <value>
        /// The Azure AD authority URL.
        /// </value>
        /// <remarks>
        ///   - For Work or School account in your org, use your tenant ID, or domain
        ///   - for any Work or School accounts, use organizations
        ///   - for any Work or School accounts, or Microsoft personal account, use common
        ///   - for Microsoft Personal account, use consumers
        /// </remarks>
        public string Authority { get; set; }

        /// <summary>
        /// Gets or sets the client Id (App Id) from the app registration in the Azure AD portal.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the tenant identifier (tenant Id/directory id) of the Azure AD tenant where the app registration exists.
        /// </summary>
        /// <value>
        /// The tenant identifier.
        /// </value>
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the redirect URI of this app as provided in the app registration portal.
        /// </summary>
        /// <value>
        /// The redirect URI.
        /// </value>
        public string RedirectURI { get; set; }

        /// <summary>
        /// Gets or sets the file name of the token cache file.
        /// </summary>
        /// <value>
        /// The name of the cache file.
        /// </value>
        public string CacheFileName { get; set; }

        /// <summary>
        /// Gets or sets the token cache file dir.
        /// </summary>
        /// <value>
        /// The cache dir.
        /// </value>
        public string CacheDir { get; set; }


        public string AndroidRedirectUri { get; set; }

        public string iOSRedirectUri { get; set; }
    }
}