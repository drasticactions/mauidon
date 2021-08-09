// <copyright file="UserProfileHeaderView.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Mastonet.Entities;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using static System.Net.Mime.MediaTypeNames;

namespace Mauidon.Views
{
    /// <summary>
    /// User Profile Header View.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserProfileHeaderView : ContentView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileHeaderView"/> class.
        /// </summary>
        /// <param name="account">User Account.</param>
        public UserProfileHeaderView()
        {
            this.InitializeComponent();
        }

        public Account Account
        {
            get => (Account)this.GetValue(AccountProperty);
            set => this.SetValue(AccountProperty, value);
        }

        public static readonly BindableProperty AccountProperty = BindableProperty.Create(
                nameof(Account),
                typeof(Account),
                typeof(UserProfileHeaderView),
                null,
                propertyChanging: (bindable, oldValue, newValue) =>
                {
                    var control = bindable as UserProfileHeaderView;
                    control.Account = newValue as Account;
                });
    }
}
