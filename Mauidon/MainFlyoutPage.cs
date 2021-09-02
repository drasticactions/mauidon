// <copyright file="MainFlyoutPage.cs" company="Drastic Actions">
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
    /// Main Flyout Page.
    /// </summary>
    public class MainFlyoutPage : FlyoutPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainFlyoutPage"/> class.
        /// </summary>
        /// <param name="services">IServiceProvider.</param>
        public MainFlyoutPage(IServiceProvider services)
        {
            this.Title = "Mauidon";
            this.Flyout = new ContentPage() { Title = "Mauidon" };
            this.Detail = new NavigationPage(services.GetService<MainTabPage>());
        }
    }
}
