using Sunny.Subdy.UI.View;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sunny.Subdy.UI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(defaultValue: false);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            ComWrappers.RegisterForMarshalling(WinFormsComInterop.WinFormsComWrappers.Instance);
            ApplicationConfiguration.Initialize();
            Application.Run(new fLoading());
        }
    }
}