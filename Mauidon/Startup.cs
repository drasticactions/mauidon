// <copyright file="Startup.cs" company="Drastic Actions">
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
    /// MAUI Startup.
    /// </summary>
    public class Startup : IStartup
    {
        /// <inheritdoc/>
        public void Configure(IAppHostBuilder appBuilder)
        {
            appBuilder
                .UseMauiApp<App>()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IMastoContext, MastoContext>();
                    services.AddSingleton<INavigationService, NavigationService>();
                    services.AddSingleton<IAuthorizationService, AuthorizationService>();
                    services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
                    services.AddTransient<HomeTootViewModel>();
                    services.AddTransient<UserProfilePageViewModel>();
                    services.AddTransient<AuthorizationPageViewModel>();
                    services.AddTransient<LoginPageViewModel>();
                    services.AddTransient<LoginPage>();
                    services.AddTransient<AuthorizationPage>();
                    services.AddTransient<MainTootPage>();
                    services.AddTransient<MainTabPage>();
                    services.AddTransient<UserProfilePage>();
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });
        }
    }
}