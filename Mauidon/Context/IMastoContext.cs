// <copyright file="IMastoContext.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mauidon.Models;

namespace Mauidon.Context
{
    /// <summary>
    /// Masto Context.
    /// </summary>
    public interface IMastoContext
    {
        /// <summary>
        /// Gets a boolean to indicating if this install has accounts loaded.
        /// </summary>
        /// <returns>Boolean.</returns>
        public bool HasAccount();

        /// <summary>
        /// Gets the default account from the database context.
        /// </summary>
        /// <returns><see cref="MastoUserAccount"/>.</returns>
        public MastoUserAccount GetDefaultAccount();

        /// <summary>
        /// Adds a new account to the database context.
        /// </summary>
        /// <param name="account"><see cref="MastoUserAccount"/>.</param>
        /// <returns>The same <see cref="MastoUserAccount"/>.</returns>
        public MastoUserAccount AddAccount(MastoUserAccount account);

        /// <summary>
        /// Gets an account from the database context.
        /// </summary>
        /// <param name="id">The account id.</param>
        /// <returns><see cref="MastoUserAccount"/>.</returns>
        public MastoUserAccount GetAccount(long id);
    }

}
