// <copyright file="BaseViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Mauidon.Services;
using Microsoft.Maui.Controls;

namespace Mauidon.ViewModels
{
    /// <summary>
    /// Base View Model.
    /// </summary>
    public abstract class BaseViewModel : ExtendedBindableObject
    {
        private bool isBusy;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">INavigationService.</param>
        public BaseViewModel()
        {
        }

        /// <summary>
        /// Gets or sets the Close Dialog Command.
        /// </summary>
        public Command CloseDialogCommand { get; set; }

        /// <summary>
        /// Gets or Sets a value indicating whether the view is busy.
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return this.isBusy;
            }

            set
            {
                this.isBusy = value;
                this.OnPropertyChanged("IsBusy");
            }
        }

        /// <summary>
        /// Load VM Async.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public virtual Task LoadAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Unload VM Async.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public virtual Task UnloadAsync()
        {
            return Task.CompletedTask;
        }
    }
}
