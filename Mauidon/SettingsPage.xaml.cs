// <copyright file="SettingsPage.xaml.cs" company="Drastic Actions">
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
    /// Settings Page.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private readonly SettingsPageViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsPage"/> class.
        /// </summary>
        /// <param name="services">IServiceProvider.</param>
        public SettingsPage(IServiceProvider services)
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = services.GetService<SettingsPageViewModel>();
        }
    }
}
