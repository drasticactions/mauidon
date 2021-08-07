// <copyright file="AuthorizationPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Mastonet;
using Mastonet.Entities;
using Mauidon.Context;
using Mauidon.Models;
using Mauidon.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;

namespace Mauidon.ViewModels
{
    /// <summary>
    /// Authorization Page View Model.
    /// </summary>
    public class AuthorizationPageViewModel : BaseViewModel
    {
        private readonly IServiceProvider service;
        private readonly IAuthorizationService authService;
        private readonly INavigationService navigationService;
        private readonly IMastoContext db;
        private MastodonClient client;
        private Account account;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationPageViewModel"/> class.
        /// </summary>
        /// <param name="services">IServiceProvider.</param>
        /// <param name="db">IMastroContext.</param>
        /// <param name="navigation">INavigationService.</param>
        /// <param name="authorizationService">IAuthorizationService.</param>
        public AuthorizationPageViewModel(IServiceProvider services, IMastoContext db, INavigationService navigation, IAuthorizationService authorizationService)
        {
            this.service = services;
            this.db = db;
            this.authService = authorizationService;
            this.navigationService = navigation;
            this.StartLoginCommand = new Command(
                async () => await this.SaveAndLoginAsync(),
                () => true);
        }

        /// <summary>
        /// Gets or sets the Account.
        /// </summary>
        public Account Account
        {
            get => this.account;
            set => this.SetProperty(ref this.account, value);
        }

        /// <summary>
        /// Gets or sets the Start Login Command.
        /// </summary>
        public Command StartLoginCommand { get; set; }

        /// <summary>
        /// Login Via Code.
        /// </summary>
        /// <param name="code">Mastodon OAuth Code to login with.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task LoginViaCodeAsync(string code)
        {
            if (this.IsBusy || this.Account != null)
            {
                return;
            }

            this.IsBusy = true;
            (this.client, this.Account) = await this.authService.LoginWithCodeAsync(code);
            this.IsBusy = false;
        }

        /// <summary>
        /// Save and Login Async.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task SaveAndLoginAsync()
        {
            this.IsBusy = true;
            this.db.AddAccount(new Models.MastoUserAccount()
            {
                AccountId = this.account.Id,
                Account = this.account,
                AppRegistrationId = this.client.AppRegistration.Id,
                UserAuthId = this.account.Id,
                AppRegistration = this.client.AppRegistration,
                UserAuth = UserAuth.GenerateUserAuth(this.account.Id, this.client.AuthToken),
            });
            this.IsBusy = false;
            this.navigationService.ReplaceMainPageInMainWindow(new MainTootPage(this.service));
            await this.navigationService.PopModalPageInMainWindowAsync();
        }
    }
}
