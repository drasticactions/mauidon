// <copyright file="App.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Mauidon.Context;
using Mauidon.Services;
using Mauidon.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Application = Microsoft.Maui.Controls.Application;

namespace Mauidon
{
    /// <summary>
    /// MAUI App.
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider services;
        private readonly INavigationService navigation;
        private readonly IMastoContext db;

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        /// <param name="services">IServiceProvider.</param>
        public App(IServiceProvider services)
        {
            this.services = services;
            this.navigation = services.GetService<INavigationService>();
            this.db = services.GetService<IMastoContext>();
            this.InitializeComponent();
        }

        /// <summary>
        /// Show Authorization Modal Async.
        /// </summary>
        /// <param name="code">Authorization Code.</param>
        public async void ShowAuthorizationModalAsync(string code)
        {
            await this.navigation.PushModalPageInMainWindowAsync(new AuthorizationPage(code, this.services));
        }

        /// <inheritdoc/>
        protected override Window CreateWindow(IActivationState activationState)
        {
            var hasAccounts = this.db.HasAccount();
            if (hasAccounts)
            {
                return new MainWindow(this.navigation.GetDefaultPage());
            }
            else
            {
                return new MainWindow(this.services.GetService<LoginPage>());
            }
        }
    }
}
