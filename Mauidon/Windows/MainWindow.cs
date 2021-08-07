// <copyright file="MainWindow.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Microsoft.Maui.Controls;

namespace Mauidon.Windows
{
    /// <summary>
    /// Main Window.
    /// </summary>
    public class MainWindow : Window
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        /// <param name="page">Maui Page.</param>
        public MainWindow(Microsoft.Maui.Controls.Page page)
            : base(page)
        {
        }
    }
}
