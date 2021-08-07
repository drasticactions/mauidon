// <copyright file="NavigationService.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Microsoft.Maui.Controls;

namespace Mauidon.Services
{
    /// <summary>
    /// Navigation Service.
    /// </summary>
    public class NavigationService : INavigationService
    {
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

        private Window GetMainWindow()
        {
            return App.Current.Windows[0];
        }
    }
}
