// <copyright file="AppDelegate.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using Microsoft.Maui;
using UIKit;

namespace Mauidon
{
    /// <summary>
    /// App Delegate.
    /// </summary>
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate<Startup>
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
             global::Xamarin.Auth.Presenters.XamarinIOS.AuthenticationConfiguration.Init();
             return base.FinishedLaunching(app, options);
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            var mapp = MauiUIApplicationDelegate.Current.Application as App;
            var test = url.AbsoluteUrl.ToString().Split("code=").Last();
            mapp.ShowAuthorizationModalAsync(test);
            return true;
        }
    }
}