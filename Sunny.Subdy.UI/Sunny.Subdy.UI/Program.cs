using Sunny.Subdy.Common.Logs;
using Sunny.Subdy.UI.View;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WinFormsComInterop;

namespace Sunny.Subdy.UI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
           
            int currentId = Process.GetCurrentProcess().Id;

            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    if (process.Id == currentId) continue;

                    string exeName = Path.GetFileNameWithoutExtension(process.MainModule.FileName);
                    if (exeName.Contains("LamToolAutoPhone", StringComparison.OrdinalIgnoreCase))
                    {
                        process.Kill();
                    }
                }
                catch { }
            }

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                File.WriteAllText("fatal.log", e.ExceptionObject.ToString());
            };

            Application.ThreadException += (s, e) =>
            {
                File.WriteAllText("ui-crash.log", e.Exception.ToString());
                LogManager.Error(e.Exception);
            };

            try
            {
              
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                ComWrappers.RegisterForMarshalling(WinFormsComWrappers.Instance);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                PreserveGdiResources();
                var loading = new fLoading();
                loading.ShowDialog();
                Application.Run(loading.MainForm);
            }
            catch (Exception ex)
            {
                File.WriteAllText("app-crash.log", ex.ToString());
                LogManager.Error(ex);
            }
        }
        [DynamicDependency(DynamicallyAccessedMemberTypes.PublicConstructors, typeof(Bitmap))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.PublicConstructors, typeof(Image))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.PublicConstructors, typeof(ImageFormat))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.PublicConstructors, typeof(MemoryStream))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.PublicConstructors, typeof(ImageConverter))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.PublicConstructors, typeof(Graphics))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.PublicMethods, typeof(Image))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.PublicMethods, typeof(Bitmap))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.PublicFields, typeof(ImageFormat))]
        static void PreserveGdiResources()
        {
            _ = new Bitmap(1, 1);
            _ = new MemoryStream();
            _ = new ImageConverter();
            _ = ImageFormat.Png;
        }
    }
}