// <copyright file="MauiProgram.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Mauidon.Context;
using Mauidon.Services;
using Mauidon.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Hosting;

[assembly: XamlCompilationAttribute(XamlCompilationOptions.Compile)]

namespace Mauidon
{
    /// <summary>
    /// Maui Program.
    /// </summary>
    public static class MauiProgram
    {
        /// <summary>
        /// Create Maui App.
        /// </summary>
        /// <returns>Maui App.</returns>
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.Services.AddSingleton<IMastoContext, MastoContext>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<IAuthorizationService, AuthorizationService>();
            builder.Services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
            builder.Services.AddTransient<HomeTootViewModel>();
            builder.Services.AddTransient<UserProfilePageViewModel>();
            builder.Services.AddTransient<AuthorizationPageViewModel>();
            builder.Services.AddTransient<LoginPageViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<AuthorizationPage>();
            builder.Services.AddTransient<MainTootPage>();
            builder.Services.AddTransient<MainFlyoutPage>();
            builder.Services.AddTransient<MainTabPage>();
            builder.Services.AddTransient<UserProfilePage>();
            builder.Services.AddTransient<SettingsPage>();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("fa-brands-400.ttf", "FontAwesomeBrands");
                    fonts.AddFont("fa-regular-400.ttf", "FontAwesomeRegular");
                    fonts.AddFont("fa-solid-900.ttf", "FontAwesomeSolid");
                });

            return builder.Build();
        }
    }
}