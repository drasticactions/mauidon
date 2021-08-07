﻿// <copyright file="AuthorizationService.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Mastonet;
using Mastonet.Entities;
using Mauidon.Tools;
using Microsoft.Maui.Essentials;

namespace Mauidon.Services
{
    /// <summary>
    /// Authorization Service.
    /// </summary>
    public class AuthorizationService : IAuthorizationService
    {
        private readonly string redirectUrl = "mauidon://";

        private string hostUrl = string.Empty;
        private AppRegistration appRegistration;
        private AuthenticationClient authClient;

        /// <inheritdoc/>
        public async Task SetupLogin(string serverBase)
        {
            this.appRegistration = await this.GetAppRegistrationAsync(serverBase);
            this.authClient = new AuthenticationClient(this.appRegistration);
            var oauthUrl = this.authClient.OAuthUrl(this.redirectUrl);
            await Launcher.OpenAsync(oauthUrl);
        }

        /// <inheritdoc/>
        public async Task<(MastodonClient Client, Account Account)> LoginWithCodeAsync(string code)
        {
            var auth = await this.authClient.ConnectWithCode(code, this.redirectUrl);
            var client = new MastodonClient(this.appRegistration, auth);
            var account = await client.GetCurrentUser();
            return (client, account);
        }

        private async Task<AppRegistration> GetAppRegistrationAsync(string serverBase)
        {
            Uri.TryCreate(serverBase, UriKind.RelativeOrAbsolute, out Uri serverBaseUri);
            if (serverBaseUri == null)
            {
                throw new InvalidServerUriException(serverBase);
            }

            this.hostUrl = serverBaseUri.IsAbsoluteUri ? serverBaseUri.Host : serverBaseUri.OriginalString;
            AppRegistration appRegistration;
            try
            {
                var initAuthClient = new AuthenticationClient(this.hostUrl);
                appRegistration = await initAuthClient.CreateApp("Mauidon", Scope.Read | Scope.Write | Scope.Follow, null, this.redirectUrl);
            }
            catch (Exception ex)
            {
                throw new AppRegistrationCreationFailureException(ex.Message, ex);
            }

            return appRegistration;
        }
    }
}
