// <copyright file="Startup.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Mauidon.Context;
using Mauidon.Controls;
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
                    services.AddTransient<MainFlyoutPage>();
                    services.AddTransient<MainTabPage>();
                    services.AddTransient<UserProfilePage>();
                })
                .ConfigureMauiHandlers(handlers => {
//#if __ANDROID__
//                handlers.AddHandler(typeof(HtmlLabel), typeof(Controls.Droid.HtmlLabelHandler));
//#elif __IOS__
//                handlers.AddHandler(typeof(HtmlLabel), typeof(Controls.iOS.HtmlLabelHandler));
//#endif

                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("fa-brands-400.ttf", "FontAwesomeBrands");
                    fonts.AddFont("fa-regular-400.ttf", "FontAwesomeRegular");
                    fonts.AddFont("fa-solid-900.ttf", "FontAwesomeSolid");
                });
        }
    }
}