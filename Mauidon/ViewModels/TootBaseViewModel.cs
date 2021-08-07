﻿// <copyright file="TootBaseViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Mastonet.Entities;
using Mauidon.Context;
using Mauidon.Models;
using Mauidon.Tools;

namespace Mauidon.ViewModels
{
    /// <summary>
    /// Toot Base View Model.
    /// Used to host new Toot pages.
    /// </summary>
    public class TootBaseViewModel : BaseViewModel
    {
        private readonly IMastoContext db;

        private MastoUserAccount account;
        private MastodonList<Status> timeline;
        private TimelineType timelineType;

        /// <summary>
        /// Initializes a new instance of the <see cref="TootBaseViewModel"/> class.
        /// </summary>
        /// <param name="db">IMastoContext.</param>
        /// <param name="timelineType">TimelineType.</param>
        public TootBaseViewModel(IMastoContext db, TimelineType timelineType)
        {
            this.db = db;
            this.timelineType = timelineType;
            this.Account = this.db.GetDefaultAccount();
        }

        /// <summary>
        /// Gets or sets the Account.
        /// </summary>
        public MastoUserAccount Account
        {
            get => this.account;
            set => this.SetProperty(ref this.account, value);
        }

        /// <summary>
        /// Gets or sets the Timeline.
        /// </summary>
        public MastodonList<Status> Timeline
        {
            get => this.timeline;
            set => this.SetProperty(ref this.timeline, value);
        }

        /// <summary>
        /// Refresh Toot Feed.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RefreshFeed()
        {
            this.IsBusy = true;
            switch (this.timelineType)
            {
                case TimelineType.Public:
                    this.Timeline = await this.Account.Client.GetPublicTimeline();
                    break;
                case TimelineType.Home:
                    this.Timeline = await this.Account.Client.GetHomeTimeline();
                    break;
            }

            this.IsBusy = false;
        }
    }
}