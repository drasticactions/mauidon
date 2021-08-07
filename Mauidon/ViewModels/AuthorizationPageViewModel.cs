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
        private MastodonClient client;
        private Account account;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationPageViewModel"/> class.
        /// </summary>
        /// <param name="services">IServiceProvider.</param>
        public AuthorizationPageViewModel(IServiceProvider services)
            : base(services)
        {
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
            (this.client, this.Account) = await this.Authorization.LoginWithCodeAsync(code);
            this.IsBusy = false;
        }

        /// <summary>
        /// Save and Login Async.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task SaveAndLoginAsync()
        {
            this.IsBusy = true;
            this.Database.AddAccount(new Models.MastoUserAccount()
            {
                AccountId = this.account.Id,
                Account = this.account,
                AppRegistrationId = this.client.AppRegistration.Id,
                UserAuthId = this.account.Id,
                AppRegistration = this.client.AppRegistration,
                UserAuth = UserAuth.GenerateUserAuth(this.account.Id, this.client.AuthToken),
            });
            this.IsBusy = false;
            this.Navigation.ReplaceMainPageInMainWindow(this.Services.GetService<MainTootPage>());
            await this.Navigation.PopModalPageInMainWindowAsync();
        }
    }
}
