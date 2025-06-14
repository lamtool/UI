using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;

namespace AutoAndroid
{
    public class FileHelper
    {
        public static bool UnzipTgz(string zipPath, string goalFolder)
        {
            System.IO.Stream inStream = null;
            System.IO.Stream gzipStream = null;
            TarArchive tarArchive = null;
            try
            {
                using (inStream = File.OpenRead(zipPath))
                {
                    using (gzipStream = new GZipInputStream(inStream))
                    {
                        tarArchive = TarArchive.CreateInputTarArchive(gzipStream);
                        tarArchive.ExtractContents(goalFolder);
                        tarArchive.Close();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                tarArchive?.Close();
                gzipStream?.Close();
                inStream?.Close();
            }
        }
        public static bool DeleteFile(string path)
        {
            if (!File.Exists(path))
            {
                return true;
            }
            try
            {
                File.Delete(path);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
