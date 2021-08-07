﻿// <copyright file="HomeTootViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Mauidon.Context;
using Mauidon.Services;
using Mauidon.Tools;

namespace Mauidon.ViewModels
{
    /// <summary>
    /// Home Toot View Model.
    /// </summary>
    public class HomeTootViewModel : TootBaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeTootViewModel"/> class.
        /// </summary>
        /// <param name="db">IMastoContext.</param>
        public HomeTootViewModel(IMastoContext db)
            : base(db, TimelineType.Home)
        {
        }
    }
}