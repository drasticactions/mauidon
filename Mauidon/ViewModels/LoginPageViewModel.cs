// <copyright file="LoginPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Mauidon.Services;
using Mauidon.Tools;
using Microsoft.Maui.Controls;

namespace Mauidon.ViewModels
{
    /// <summary>
    /// Login Page View Model.
    /// </summary>
    public class LoginPageViewModel : BaseViewModel
    {
        private string serverBaseUrl = "mastodon.social";

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPageViewModel"/> class.
        /// </summary>
        /// <param name="services">IServiceProvider.</param>
        public LoginPageViewModel(IServiceProvider services)
            : base(services)
        {
            this.StartLoginCommand = new AsyncCommand(
                async () => await this.ExecuteStartLoginCommand(),
                () => !string.IsNullOrEmpty(this.ServerBaseUrl),
                this.Error);
        }

        /// <summary>
        /// Gets or sets the server base url.
        /// </summary>
        public string ServerBaseUrl
        {
            get => this.serverBaseUrl;
            set
            {
                this.SetProperty(ref this.serverBaseUrl, value);
                this.StartLoginCommand?.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Start Login Command.
        /// </summary>
        public AsyncCommand StartLoginCommand { get; set; }

        private async Task ExecuteStartLoginCommand()
        {
            if (!string.IsNullOrEmpty(this.ServerBaseUrl))
            {
                await this.Authorization.SetupLogin(this.ServerBaseUrl);
            }
        }
    }
}
