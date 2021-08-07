// <copyright file="MastoContext.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using LiteDB;
using Mastonet;
using Mauidon.Models;
using Microsoft.Maui.Controls;

namespace Mauidon.Context
{
    /// <summary>
    /// Masto Context.
    /// </summary>
    public class MastoContext : IMastoContext
    {
        private const string UseraccountDB = "useraccounts";
        private const string UserauthDB = "userauth";
        private const string DatabaseName = "database.db";

        private LiteDatabase db;
        private string databasePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="MastoContext"/> class.
        /// </summary>
        public MastoContext()
        {
            this.OnConfiguring();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MastoContext"/> class.
        /// </summary>
        /// <param name="dbPath">Overridden Database Path.</param>
        public MastoContext(string dbPath)
        {
            this.OnConfiguring(dbPath);
        }

        /// <summary>
        /// Gets the User Accounts.
        /// </summary>
        public ILiteCollection<MastoUserAccount> UserAccounts => this.db.GetCollection<MastoUserAccount>(UseraccountDB);

        /// <inheritdoc/>
        public bool HasAccount()
        {
            return this.UserAccounts.FindAll().Any();
        }

        /// <inheritdoc/>
        public MastoUserAccount GetDefaultAccount()
        {
            var account = this.UserAccounts.Include(n => n.UserAuth).Include(n => n.AppRegistration).FindOne(n => n.IsDefault);
            if (account != null)
            {
                account.Client = new MastodonClient(account.AppRegistration, UserAuth.GenerateAuth(account.UserAuth));
            }

            return account;
        }

        /// <inheritdoc/>
        public MastoUserAccount AddAccount(MastoUserAccount account)
        {
            if (!this.UserAccounts.FindAll().Any())
            {
                account.IsDefault = true;
            }

            if (!this.UserAccounts.FindAll().Any(n => n.AccountId == account.AccountId))
            {
                this.UserAccounts.Insert(account);
            }

            return account;
        }

        /// <inheritdoc/>
        public MastoUserAccount GetAccount(long id)
        {
            var account = this.UserAccounts.Include(n => n.UserAuth).Include(n => n.AppRegistration).FindOne(n => n.AccountId == id);
            if (account != null)
            {
                account.Client = new MastodonClient(account.AppRegistration, UserAuth.GenerateAuth(account.UserAuth));
            }

            return account;
        }

        private void OnConfiguring(string databasePath = "")
        {
            if (string.IsNullOrEmpty(databasePath))
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.macOS:
                    case Device.iOS:
                        this.databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", DatabaseName);
                        break;
                    case Device.Android:
                        this.databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseName);
                        break;
                    default:
                        throw new NotImplementedException("Platform not supported");
                }
            }
            else
            {
                this.databasePath = Path.Combine(this.databasePath, DatabaseName);
            }

            this.db = new LiteDatabase(this.databasePath);
        }
    }
}
