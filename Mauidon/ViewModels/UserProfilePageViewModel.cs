// <copyright file="UserProfilePageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mastonet.Entities;

namespace Mauidon.ViewModels
{
    /// <summary>
    /// User Profile Page View Model.
    /// </summary>
    public class UserProfilePageViewModel : TootBaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfilePageViewModel"/> class.
        /// </summary>
        /// <param name="service">IServiceProvider.</param>
        public UserProfilePageViewModel(IServiceProvider service)
            : base(service, TimelineType.User)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfilePageViewModel"/> class.
        /// </summary>
        /// <param name="userAccount">User Account.</param>
        /// <param name="service">IServiceProvider.</param>
        public UserProfilePageViewModel(Account userAccount, IServiceProvider service)
            : base(service, TimelineType.User)
        {
            this.UserAccount = userAccount;
        }
    }
}
