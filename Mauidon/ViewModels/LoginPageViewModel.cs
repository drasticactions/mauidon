// <copyright file="LoginPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Mauidon.Services;
using Microsoft.Maui.Controls;

namespace Mauidon.ViewModels
{
    /// <summary>
    /// Login Page View Model.
    /// </summary>
    public class LoginPageViewModel : BaseViewModel
    {
        private readonly IAuthorizationService authService;

        private string serverBaseUrl = "mastodon.social";

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPageViewModel"/> class.
        /// </summary>
        /// <param name="authorizationService">Authorization Service.</param>
        public LoginPageViewModel(IAuthorizationService authorizationService)
        {
            this.authService = authorizationService;
            this.StartLoginCommand = new Command(
                async () => await this.ExecuteStartLoginCommand(),
                () => !string.IsNullOrEmpty(this.ServerBaseUrl));
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
                this.StartLoginCommand?.ChangeCanExecute();
            }
        }

        /// <summary>
        /// Gets or sets the Start Login Command.
        /// </summary>
        public Command StartLoginCommand { get; set; }

        private async Task ExecuteStartLoginCommand()
        {
            if (!string.IsNullOrEmpty(this.ServerBaseUrl))
            {
                await this.authService.SetupLogin(this.ServerBaseUrl);
            }
        }
    }
}
