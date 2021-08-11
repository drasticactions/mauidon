// <copyright file="UriExtensions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Maui.Essentials;

namespace Mauidon.Controls
{
    /// <summary>
    /// Uri Extensions.
    /// </summary>
    public static class UriExtensions
    {
        /// <summary>
        /// Is Http.
        /// </summary>
        /// <param name="uri">URI To Check.</param>
        /// <returns>Boolean.</returns>
        public static bool IsHttp(this Uri uri)
        {
            return uri != null && uri.Scheme.ToUpperInvariant().Contains("HTTP");
        }

        /// <summary>
        /// Is Email.
        /// </summary>
        /// <param name="uri">URI To Check.</param>
        /// <returns>Boolean.</returns>
        public static bool IsEmail(this Uri uri)
        {
            return uri.MatchSchema("mailto");
        }

        /// <summary>
        /// Is Telephone Number.
        /// </summary>
        /// <param name="uri">URI To Check.</param>
        /// <returns>Boolean.</returns>
        public static bool IsTel(this Uri uri)
        {
            return uri.MatchSchema("tel");
        }

        /// <summary>
        /// Is SMS.
        /// </summary>
        /// <param name="uri">URI To Check.</param>
        /// <returns>Boolean.</returns>
        public static bool IsSms(this Uri uri)
        {
            return uri.MatchSchema("sms");
        }

        /// <summary>
        /// Is Geography.
        /// </summary>
        /// <param name="uri">URI To Check.</param>
        /// <returns>Boolean.</returns>
        public static bool IsGeo(this Uri uri)
        {
            return uri.MatchSchema("geo");
        }

        /// <summary>
        /// Launch Browser.
        /// </summary>
        /// <param name="uri">URI To Launch.</param>
        /// <param name="options">Browser Launch Options.</param>
        public static void LaunchBrowser(this Uri uri, BrowserLaunchOptions options)
        {
            if (options == null)
            {
                Browser.OpenAsync(uri);
            }
            else
            {
                Browser.OpenAsync(uri, options);
            }
        }

        /// <summary>
        /// Lauch Email.
        /// </summary>
        /// <param name="uri">URI To Launch.</param>
        /// <returns>Boolean if Email launched.</returns>
        public static bool LaunchEmail(this Uri uri)
        {
            if (uri == null)
            {
                return false;
            }

            var qParams = uri.ParseQueryString();
            var to = uri.Target();
            try
            {
                var message = new EmailMessage
                {
                    To = new List<string> { to },
                    Subject = qParams.GetFirst("subject") ?? string.Empty,
                    Body = qParams.GetFirst("body") ?? string.Empty,
                    Cc = qParams.Get("cc") ?? new List<string>(),
                    Bcc = qParams.Get("bcc") ?? new List<string>(),
                };
                Email.ComposeAsync(message);
                return true;
            }
            catch (FeatureNotSupportedException ex)
            {
                System.Diagnostics.Debug.WriteLine(@"            ERROR: ", ex.Message);
                return false;
            }

        }

        /// <summary>
        /// Launch Phone.
        /// </summary>
        /// <param name="uri">URI to launch.</param>
        /// <returns>Boolean if Phone launched.</returns>
        public static bool LaunchTel(this Uri uri)
        {
            if (uri == null)
            {
                return false;
            }

            var to = uri.Target();
            try
            {
                PhoneDialer.Open(to);
                return true;
            }
            catch (FeatureNotSupportedException ex)
            {
                System.Diagnostics.Debug.WriteLine(@"            ERROR: ", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Launch SMS.
        /// </summary>
        /// <param name="uri">URI to launch.</param>
        /// <returns>Boolean if SMS launched.</returns>
        public static bool LaunchSms(this Uri uri)
        {
            if (uri == null)
            {
                return false;
            }

            var qParams = uri.ParseQueryString();
            var to = uri.Target();
            try
            {
                var messageText = qParams.GetFirst("body");
                var message = new SmsMessage(messageText, new[] { to });
                Sms.ComposeAsync(message);
                return true;
            }
            catch (FeatureNotSupportedException ex)
            {
                System.Diagnostics.Debug.WriteLine(@"            ERROR: ", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Launch Maps.
        /// </summary>
        /// <param name="uri">URI to launch.</param>
        /// <returns>Boolean if the maps launched.</returns>
        public static bool LaunchMaps(this Uri uri)
        {
            if (uri == null)
            {
                return false;
            }

            var target = uri.Target();
            try
            {
                var coordinates = target.Split(',');
                var latitude = double.Parse(coordinates[0], CultureInfo.InvariantCulture.NumberFormat);
                var longitude = double.Parse(coordinates[1].Split(';')[0], CultureInfo.InvariantCulture.NumberFormat);
                var location = new Location(latitude, longitude);
                Map.OpenAsync(location);
                return true;
            }
            catch (FeatureNotSupportedException ex)
            {
                System.Diagnostics.Debug.WriteLine(@"            ERROR: ", ex.Message);
                return false;
            }
        }

        private static string Target(this Uri uri)
        {
            return Uri.UnescapeDataString(uri.AbsoluteUri.Substring(uri.Scheme.Length + 1).Split('?')[0].Replace("/", ""));
        }

        private static bool MatchSchema(this Uri uri, string schema)
        {
            return uri != null && uri.Scheme.Equals(schema, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
