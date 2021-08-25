// <copyright file="RendererHelper.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using Microsoft.Maui.Graphics;

namespace Mauidon.Controls
{
    /// <summary>
    /// Renderer Helper.
    /// </summary>
    internal class RendererHelper
    {
        private static readonly string[] SupportedProperties =
            {
                Label.TextProperty.PropertyName,
                Label.FontAttributesProperty.PropertyName,
                Label.FontFamilyProperty.PropertyName,
                Label.FontSizeProperty.PropertyName,
                Label.HorizontalTextAlignmentProperty.PropertyName,
                Label.TextColorProperty.PropertyName,
                HtmlLabel.LinkColorProperty.PropertyName,
            };

        private readonly Label label;
        private readonly string runtimePlatform;
        private readonly bool isRtl;
        private readonly string text;
        private readonly IList<KeyValuePair<string, string>> styles;

        /// <summary>
        /// Initializes a new instance of the <see cref="RendererHelper"/> class.
        /// </summary>
        /// <param name="label">MAUI Label.</param>
        /// <param name="text">Text to be inserted.</param>
        /// <param name="runtimePlatform">The Runtime Platform.</param>
        /// <param name="isRtl">Is Right To Left.</param>
        public RendererHelper(Label label, string text, string runtimePlatform, bool isRtl)
        {
            this.label = label ?? throw new ArgumentNullException(nameof(label));
            this.runtimePlatform = runtimePlatform;
            this.isRtl = isRtl;
            this.text = text?.Trim();
            this.styles = new List<KeyValuePair<string, string>>();
        }

        /// <summary>
        /// Require Process.
        /// </summary>
        /// <param name="propertyName">Property Name to be processed.</param>
        /// <returns>Boolean.</returns>
        public static bool RequireProcess(string propertyName) => SupportedProperties.Contains(propertyName);

        /// <summary>
        /// Handles the Uri for the following types:
        /// - Web url
        /// - Email
        /// - Telephone
        /// - SMS
        /// - GEO.
        /// </summary>
        /// <param name="label">HtmlHabel.</param>
        /// <param name="url">Url.</param>
        /// <returns>true if the uri has been handled correctly, false if the uri is not handled because of an error.</returns>
        public static bool HandleUriClick(HtmlLabel label, string url)
        {
            if (url == null || !Uri.IsWellFormedUriString(WebUtility.UrlEncode(url), UriKind.RelativeOrAbsolute))
            {
                return false;
            }

            var args = new WebNavigatingEventArgs(WebNavigationEvent.NewPage, new UrlWebViewSource { Url = url }, url);

            label.SendNavigating(args);

            if (args.Cancel)
            {
                // Uri is handled because it is cancled;
                return true;
            }

            bool result = false;
            var uri = new Uri(url);

            if (uri.IsHttp())
            {
                uri.LaunchBrowser(label.BrowserLaunchOptions);
                result = true;
            }
            else if (uri.IsEmail())
            {
                result = uri.LaunchEmail();
            }
            else if (uri.IsTel())
            {
                result = uri.LaunchTel();
            }
            else if (uri.IsSms())
            {
                result = uri.LaunchSms();
            }
            else if (uri.IsGeo())
            {
                result = uri.LaunchMaps();
            }
            else
            {
                result = Launcher.TryOpenAsync(uri).Result;
            }

            // KWI-FIX What to do if the navigation failed? I assume not to spawn the SendNavigated event or introduce a fail bit on the args.
            label.SendNavigated(args);
            return result;
        }

        /// <summary>
        /// Add Font Attributes Style.
        /// </summary>
        /// <param name="fontAttributes">Font Attributes.</param>
        public void AddFontAttributesStyle(FontAttributes fontAttributes)
        {
            if (fontAttributes == FontAttributes.Bold)
            {
                this.AddStyle("font-weight", "bold");
            }
            else if (fontAttributes == FontAttributes.Italic)
            {
                this.AddStyle("font-style", "italic");
            }
        }

        /// <summary>
        /// Add Font Family Style.
        /// </summary>
        /// <param name="fontFamily">Font Family.</param>
        public void AddFontFamilyStyle(string fontFamily)
        {
            string GetSystemFont() => this.runtimePlatform switch
            {
                Device.macOS => "-apple-system",
                Device.iOS => "-apple-system",
                Device.Android => "Roboto",
                Device.UWP => "Segoe UI",
                _ => "system-ui",
            };

            var fontFamilyValue = string.IsNullOrWhiteSpace(fontFamily)
                 ? GetSystemFont()
                 : fontFamily;
            this.AddStyle("font-family", $"'{fontFamilyValue}'");
        }

        /// <summary>
        /// Add Font Size Style.
        /// </summary>
        /// <param name="fontSize">Font Size.</param>
        public void AddFontSizeStyle(double fontSize)
        {
            this.AddStyle("font-size", $"{fontSize}px");
        }

        /// <summary>
        /// Add Text Color Style.
        /// </summary>
        /// <param name="color">Color to add to text.</param>
        public void AddTextColorStyle(Color color)
        {
            if (Microsoft.Maui.Controls.ColorExtensions.IsDefault(color))
            {
                return;
            }

            var red = (int)(color.Red * 255);
            var green = (int)(color.Green * 255);
            var blue = (int)(color.Blue * 255);
            var alpha = color.Alpha;
            var hex = $"#{red:X2}{green:X2}{blue:X2}";
            var rgba = $"rgba({red},{green},{blue},{alpha})";
            this.AddStyle("color", hex);
            this.AddStyle("color", rgba);
        }

        /// <summary>
        /// Add Horizontal Text Align Style.
        /// </summary>
        /// <param name="textAlignment">Text Alignment.</param>
        public void AddHorizontalTextAlignStyle(TextAlignment textAlignment)
        {
            if (textAlignment == TextAlignment.Start)
            {
                this.AddStyle("text-align", this.isRtl ? "right" : "left");
            }
            else if (textAlignment == TextAlignment.Center)
            {
                this.AddStyle("text-align", "center");
            }
            else if (textAlignment == TextAlignment.End)
            {
                this.AddStyle("text-align", this.isRtl ? "left" : "right");
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(this.text))
            {
                return null;
            }

            this.AddFontAttributesStyle(this.label.FontAttributes);
            this.AddFontFamilyStyle(this.label.FontFamily);
            this.AddTextColorStyle(this.label.TextColor);
            this.AddHorizontalTextAlignStyle(this.label.HorizontalTextAlignment);
            this.AddFontSizeStyle(this.label.FontSize);

            var style = this.GetStyle();
            return $"<div style=\"{style}\" dir=\"auto\">{this.text}</div>";
        }

        /// <summary>
        /// Gets CSS Style.
        /// </summary>
        /// <returns>String.</returns>
        public string GetStyle()
        {
            var builder = new StringBuilder();

            foreach (KeyValuePair<string, string> style in this.styles)
            {
                _ = builder.Append($"{style.Key}:{style.Value};");
            }

            var css = builder.ToString();
            if (this.styles.Any())
            {
                css = css.Substring(0, css.Length - 1);
            }

            return css;
        }

        private void AddStyle(string selector, string value)
        {
            this.styles.Add(new KeyValuePair<string, string>(selector, value));
        }
    }
}
