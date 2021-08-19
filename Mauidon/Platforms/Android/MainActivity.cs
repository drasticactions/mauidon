// <copyright file="MainActivity.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Maui;

namespace Mauidon
{
    /// <summary>
    /// Main Android MAUI Activity.
    /// </summary>
    [Activity(Label = "Mauidon", Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : MauiAppCompatActivity
    {
        /// <inheritdoc/>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, savedInstanceState);
            base.OnCreate(savedInstanceState);
        }

        /// <summary>
        /// Custom URL Scheme Activity.
        /// </summary>
        [Activity(Label = "CustomUrlSchemeInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
        [IntentFilter(
            new[] { Intent.ActionView },
            Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
            DataSchemes = new[] { "mauidon" },
            DataPath = "/")]
        public class CustomUrlSchemeInterceptorActivity : Activity
        {
            /// <inheritdoc/>
            protected override void OnCreate(Bundle savedInstanceState)
            {
                base.OnCreate(savedInstanceState);

                // Convert Android.Net.Url to Uri
                var uri = new System.Uri(this.Intent.Data.ToString());

                // Load redirectUrl page
                var app = MauiApplication.Current.Application as App;
                var test = uri.AbsoluteUri.Split("code=").Last();
                app.ShowAuthorizationModalAsync(test);
                this.Finish();
            }
        }
    }
}