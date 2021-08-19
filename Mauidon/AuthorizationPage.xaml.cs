// <copyright file="AuthorizationPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Mauidon.Tools;
using Mauidon.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace Mauidon
{
    /// <summary>
    /// Authorization Page.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthorizationPage : ContentPage
    {
        private readonly AuthorizationPageViewModel vm;
        private readonly string code;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationPage"/> class.
        /// </summary>
        /// <param name="code">The Authorization Code from OAuth.</param>
        /// <param name="services">IServiceProvider</param>
        public AuthorizationPage(string code, IServiceProvider services)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            this.InitializeComponent();
            this.code = code;
            this.BindingContext = this.vm = services.GetService<AuthorizationPageViewModel>();
            this.vm?.LoginViaCodeAsync(this.code).FireAndForgetSafeAsync();
        }
    }
}
