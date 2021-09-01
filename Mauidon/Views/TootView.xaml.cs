// <copyright file="TootView.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mastonet.Entities;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace Mauidon.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TootView : ContentView
    {
        public TootView()
        {
            InitializeComponent();
        }

        public Status Status
        {
            get => (Status)this.GetValue(StatusProperty);
            set => this.SetValue(StatusProperty, value);
        }

        public static readonly BindableProperty StatusProperty = BindableProperty.Create(
                nameof(Status),
                typeof(Status),
                typeof(TootView),
                null,
                propertyChanging: (bindable, oldValue, newValue) =>
                {
                    var control = bindable as TootView;
                    control.Status = newValue as Status;
                });
    }
}
