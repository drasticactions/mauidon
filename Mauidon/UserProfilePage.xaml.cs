// <copyright file="UserProfilePage.xaml.cs" company="Drastic Actions">
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
    /// User Profile Page.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserProfilePage : ContentPage
    {
        private UserProfilePageViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfilePage"/> class.
        /// </summary>
        /// <param name="services">IServiceProvider.</param>
        public UserProfilePage(IServiceProvider services)
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = services.GetService<UserProfilePageViewModel>();
            this.vm?.RefreshFeed().FireAndForgetSafeAsync();
        }
    }
}
