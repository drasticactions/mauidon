// <copyright file="MainFlyoutPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace Mauidon
{
    /// <summary>
    /// Main Flyout Page.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainFlyoutPage : FlyoutPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainFlyoutPage"/> class.
        /// </summary>
        public MainFlyoutPage()
        {
            this.InitializeComponent();
        }
    }
}
