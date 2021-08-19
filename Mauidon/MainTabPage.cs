// <copyright file="MainTabPage.cs" company="Drastic Actions">
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
    /// Main Tab Page.
    /// </summary>
    public class MainTabPage : Microsoft.Maui.Controls.TabbedPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainTabPage"/> class.
        /// </summary>
        /// <param name="services">IServiceProvider.</param>
        public MainTabPage(IServiceProvider services)
        {
            this.Children.Add(new NavigationPage(services.GetService<MainTootPage>()) { Title = Translations.Common.HomeTitle });
            this.Children.Add(new NavigationPage() { Title = Translations.Common.DiscoverTitle });
            this.Children.Add(new NavigationPage() { Title = Translations.Common.NotificationsTitle });
            this.Children.Add(new NavigationPage() { Title = Translations.Common.MessagesTitle });
        }
    }
}
