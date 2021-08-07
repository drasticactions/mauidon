// <copyright file="LightTheme.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace Mauidon.Styles
{
    /// <summary>
    /// Light Theme.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LightTheme : ResourceDictionary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LightTheme"/> class.
        /// </summary>
        public LightTheme()
        {
            this.InitializeComponent();
        }
    }
}
