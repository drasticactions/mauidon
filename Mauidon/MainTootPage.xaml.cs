// <copyright file="MainTootPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Mauidon.Tools;
using Mauidon.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace Mauidon
{
    /// <summary>
    /// Main MAUI Toot Page.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTootPage : ContentPage
    {
        private readonly HomeTootViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainTootPage"/> class.
        /// </summary>
        /// <param name="services">IServiceProvider.</param>
        public MainTootPage(IServiceProvider services)
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = services.GetService<HomeTootViewModel>();
            this.vm?.RefreshFeed().FireAndForgetSafeAsync();
        }
    }
}
