// <copyright file="HtmlLabel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using Microsoft.Maui.Graphics;

namespace Mauidon.Controls
{
    /// <inheritdoc />
    /// <summary>
    /// A label that is able to display HTML content.
    /// </summary>
    public class HtmlLabel : Label
    {
        /// <summary>
        /// Identify the UnderlineText property.
        /// </summary>
        public static readonly BindableProperty UnderlineTextProperty =
            BindableProperty.Create(nameof(UnderlineText), typeof(bool), typeof(HtmlLabel), true);

        /// <summary>
        /// Identify the LinkColor property.
        /// </summary>
        public static readonly BindableProperty LinkColorProperty =
            BindableProperty.Create(nameof(LinkColor), typeof(Color), typeof(HtmlLabel), default);

        /// <summary>
        /// Identify the BrowserLaunchOptions property.
        /// </summary>
        public static readonly BindableProperty BrowserLaunchOptionsProperty =
            BindableProperty.Create(nameof(BrowserLaunchOptions), typeof(BrowserLaunchOptions), typeof(HtmlLabel), default);

        /// <summary>
        /// Identify the AndroidLegacyMode property.
        /// </summary>
        public static readonly BindableProperty AndroidLegacyModeProperty =
            BindableProperty.Create(nameof(AndroidLegacyModeProperty), typeof(bool), typeof(HtmlLabel), default);

        /// <summary>
        /// Identify the AndroidListIndent property KWI-FIX.
        /// Default value = 20 (to continue support `old value`).
        /// </summary>
        public static readonly BindableProperty AndroidListIndentProperty =
            BindableProperty.Create(nameof(AndroidListIndentProperty), typeof(int), typeof(HtmlLabel), defaultValue: 20);

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlLabel"/> class.
        /// </summary>
        public HtmlLabel()
        {
        }

        /// <summary>
        /// Fires before the open URL request is done.
        /// </summary>
        public event EventHandler<WebNavigatingEventArgs> Navigating;

        /// <summary>
        /// Fires when the open URL request is done.
        /// </summary>
        public event EventHandler<WebNavigatingEventArgs> Navigated;

        /// <summary>
        /// Gets or sets a value indicating whether the hyperlinks are underlined.
        /// </summary>
        public bool UnderlineText
        {
            get { return (bool)this.GetValue(UnderlineTextProperty); }
            set { this.SetValue(UnderlineTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of hyperlinks.
        /// </summary>
        public Color LinkColor
        {
            get { return (Color)this.GetValue(LinkColorProperty); }
            set { this.SetValue(LinkColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the options to use when opening a web link. <see cref="https://docs.microsoft.com/en-us/xamarin/essentials/open-browser"/>
        /// </summary>
        public BrowserLaunchOptions BrowserLaunchOptions
        {
            get { return (BrowserLaunchOptions)this.GetValue(BrowserLaunchOptionsProperty); }
            set { this.SetValue(BrowserLaunchOptionsProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Android renderer separates block-level elements with blank lines.
        /// </summary>
        public bool AndroidLegacyMode
        {
            get { return (bool)this.GetValue(AndroidLegacyModeProperty); }
            set { this.SetValue(AndroidLegacyModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Android List Indent property KWI-FIX.
        /// </summary>
        public int AndroidListIndent
        {
            get { return (int)this.GetValue(AndroidListIndentProperty); }
            set { this.SetValue(AndroidListIndentProperty, value); }
        }

        /// <summary>
        /// Send the Navigating event.
        /// </summary>
        /// <param name="args">Arguments.</param>
        internal void SendNavigating(WebNavigatingEventArgs args)
        {
            this.Navigating?.Invoke(this, args);
        }

        /// <summary>
        /// Send the Navigated event.
        /// </summary>
        /// <param name="args">Arguments.</param>
        internal void SendNavigated(WebNavigatingEventArgs args)
        {
            this.Navigated?.Invoke(this, args);
        }
    }
}
