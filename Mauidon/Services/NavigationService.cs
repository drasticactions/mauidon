// <copyright file="NavigationService.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;

namespace Mauidon.Services
{
    /// <summary>
    /// Navigation Service.
    /// </summary>
    public class NavigationService : INavigationService
    {
        private IServiceProvider services;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationService"/> class.
        /// </summary>
        /// <param name="services">IServiceProvider.</param>
        public NavigationService(IServiceProvider services)
        {
            this.services = services;
        }

        /// <inheritdoc/>
        public Task DisplayAlertAsync(string title, string message)
        {
            MainThread.BeginInvokeOnMainThread(async () => await App.Current.MainPage.DisplayAlert(title, message, Translations.Common.CloseButton).ConfigureAwait(false));
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task PopModalPageInWindowAsync(Window window)
        {
            return window.Navigation == null
                ? throw new ArgumentException("Window must have a NavigationPage as its base")
                : window.Navigation.PopModalAsync();
        }

        /// <inheritdoc/>
        public Task PopModalPageInMainWindowAsync()
        {
            return this.PopModalPageInWindowAsync(this.GetMainWindow());
        }

        /// <inheritdoc/>
        public void ReplaceMainPageInMainWindow(Page page)
        {
            this.ReplaceMainPageInWindow(page, this.GetMainWindow());
        }

        /// <inheritdoc/>
        public void ReplaceMainPageInWindow(Page page, Window window)
        {
            window.Page = page;
        }

        /// <inheritdoc/>
        public Task PushModalPageInWindowAsync(Page page, Window window)
        {
            return window.Navigation == null
                ? throw new ArgumentException("Window must have a NavigationPage as its base")
                : window.Navigation.PushModalAsync(page);
        }

        /// <inheritdoc/>
        public Task PushModalPageInMainWindowAsync(Page page)
        {
            return this.PushModalPageInWindowAsync(page, this.GetMainWindow());
        }

        /// <inheritdoc/>
        public void ResetToDefaultPage(Window window)
        {
            window.Page = this.GetDefaultPage();
        }

        /// <inheritdoc/>
        public void ResetToDefaultPageInMainWindow()
        {
            this.ResetToDefaultPage(this.GetMainWindow());
        }

        /// <inheritdoc/>
        public Page GetDefaultPage()
        {
            return this.services.GetService<UserProfilePage>();
        }

        private Window GetMainWindow()
        {
            return App.Current.Windows[0];
        }
    }
}
