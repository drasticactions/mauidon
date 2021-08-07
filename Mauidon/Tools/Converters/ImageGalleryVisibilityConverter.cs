// <copyright file="ImageGalleryVisibilityConverter.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mastonet.Entities;
using Microsoft.Maui.Controls;

namespace Mauidon.Tools
{
    /// <summary>
    /// Image Gallery Visibility Converter.
    /// </summary>
    public class ImageGalleryVisibilityConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var attachments = value as IEnumerable<Attachment>;
            if (attachments == null)
            {
                return false;
            }

            return attachments.Any();
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
