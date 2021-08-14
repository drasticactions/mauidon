// <copyright file="INavigationService.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Microsoft.Maui.Controls;

namespace Mauidon.Services
{
    /// <summary>
    /// Navigation Service.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Display Alert to User.
        /// </summary>
        /// <param name="title">Title of message.</param>
        /// <param name="message">Message to user.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        Task DisplayAlertAsync(string title, string message);

        /// <summary>
        /// Replace Main Page In Window.
        /// </summary>
        /// <param name="page"><see cref="Page"/> to navigate to.</param>
        /// <param name="window">The <see cref="Window"/> with the  <see cref="Page"/> to replace.</param>
        public void ReplaceMainPageInWindow(Page page, Window window);

        /// <summary>
        /// Replace Main Page in the Main Window.
        /// </summary>
        /// <param name="page"><see cref="Page"/> to navigate to.</param>
        public void ReplaceMainPageInMainWindow(Page page);

        /// <summary>
        /// Push Modal Page to Main Window.
        /// </summary>
        /// <param name="page"><see cref="Page"/> to navigate to.</param>
        /// <returns><see cref="Task"/>.</returns>
        public Task PushModalPageInMainWindowAsync(Page page);

        /// <summary>
        /// Push Modal page to Window.
        /// </summary>
        /// <param name="page"><see cref="Page"/> to navigate to.</param>
        /// <param name="window"><see cref="Window"/> with Modal to push.</param>
        /// <returns><see cref="Task"/>.</returns>
        public Task PushModalPageInWindowAsync(Page page, Window window);

        /// <summary>
        /// Pop Modal page from Main Window.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public Task PopModalPageInMainWindowAsync();

        /// <summary>
        /// Pop Modal Page From Window.
        /// </summary>
        /// <param name="window"><see cref="Window"/> with Modal to pop.</param>
        /// <returns><see cref="Task"/>.</returns>
        public Task PopModalPageInWindowAsync(Window window);

        /// <summary>
        /// Push Page In Window.
        /// </summary>
        /// <param name="page"><see cref="Page"/> to navigate to.</param>
        /// <param name="window"><see cref="Window"/>.</param>
        /// <returns><see cref="Task"/>.</returns>
        public Task PushPageInWindowAsync(Page page, Window window);

        /// <summary>
        /// Push Page In Main Window.
        /// </summary>
        /// <param name="page"><see cref="Page"/> to navigate to.</param>
        /// <returns><see cref="Task"/>.</returns>
        public Task PushPageInMainWindowAsync(Page page);

        /// <summary>
        /// Reset to default page.
        /// </summary>
        /// <param name="window">Window to set the default page.</param>
        public void ResetToDefaultPage(Window window);

        /// <summary>
        /// Reset to default page in main window.
        /// </summary>
        public void ResetToDefaultPageInMainWindow();

        /// <summary>
        /// Gets the default page.
        /// </summary>
        /// <returns>The default page for the app.</returns>
        public Page GetDefaultPage();
    }
}
