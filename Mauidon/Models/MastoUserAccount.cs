// <copyright file="MastoUserAccount.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mastonet;
using Mastonet.Entities;

namespace Mauidon.Models
{
    /// <summary>
    /// Mastodon User Account.
    /// </summary>
    public class MastoUserAccount
    {
        /// <summary>
        /// Gets or sets the Account Id.
        /// </summary>
        public long AccountId { get; set; }

        /// <summary>
        /// Gets or sets the App Registration Id.
        /// </summary>
        public long AppRegistrationId { get; set; }

        /// <summary>
        /// Gets or sets the App Registration.
        /// </summary>
        public virtual AppRegistration AppRegistration { get; set; }

        /// <summary>
        /// Gets or sets the Account.
        /// </summary>
        public virtual Account Account { get; set; }

        /// <summary>
        /// Gets or sets the User Auth Id.
        /// </summary>
        public long UserAuthId { get; set; }

        /// <summary>
        /// Gets or sets the UserAuth.
        /// </summary>
        public virtual UserAuth UserAuth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the account is the default.
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Gets or sets the Client.
        /// </summary>
        [NotMapped]

        public MastodonClient Client { get; set; }
    }
}
