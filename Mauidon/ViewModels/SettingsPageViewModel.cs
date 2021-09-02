// <copyright file="SettingsPageViewModel.cs" company="Drastic Actions">
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
    /// Settings Page View Model.
    /// </summary>
    public class SettingsPageViewModel : BaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsPageViewModel"/> class.
        /// </summary>
        /// <param name="services">IServiceProvider.</param>
        public SettingsPageViewModel(IServiceProvider services)
            : base(services)
        {
            this.TestStatus = new Status()
            {
                Account = new Account()
                {
                    HeaderUrl = "https://files.mastodon.social/accounts/headers/000/458/416/original/1b44325f7ccb0b37.jpg",
                    AvatarUrl = "https://files.mastodon.social/accounts/avatars/000/458/416/original/c751c2d7145c883e.png",
                    UserName = "Drastic Actions",
                    AccountName = "drasticactions",
                },
                Content = "<p>Test Content! This is test content! Look at me! Test!</p>",
                CreatedAt = DateTime.UtcNow,
            };
        }

        /// <summary>
        /// Gets the Test Status.
        /// </summary>
        public Status TestStatus { get; private set; }
    }
}
