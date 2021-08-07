using System;
using Android.App;
using Android.Runtime;
using Microsoft.Maui;

namespace Mauidon
{
    /// <summary>
    /// Main Application.
    /// </summary>
    [Application]
    public class MainApplication : MauiApplication<Startup>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainApplication"/> class.
        /// </summary>
        /// <param name="handle">IntPtr Handle.</param>
        /// <param name="ownership">Ownership.</param>
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }
    }
}