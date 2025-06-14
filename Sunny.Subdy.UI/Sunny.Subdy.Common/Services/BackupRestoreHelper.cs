using AutoAndroid;

namespace Sunny.Subdy.Common
{
    public class BackupRestoreHelper
    {
        private readonly ADBClient _Android;
        private const string Package_Facebook = "com.facebook.katana";
        private const string PackageInstagram = "com.instagram.android";
        private const string PackageTikTok = "com.ss.android.ugc.trill";
        public BackupRestoreHelper(DeviceModel device)
        {
            _Android = new ADBClient(device);
        }
        public bool BackupFacebook(string file)
        {
            for (int i = 0; i < 10; i++)
            {
                _Android.Shell("su -c 'rm -rf /data/data/" + Package_Facebook + "/*.tar.gz'");
                _Android.Shell("su -c 'rm -rf /sdcard/*.tar.gz'");
                _Android.Device.Status = "Đang backup facebook...";
                _Android.ADB.Shell("su root sh -c 'tar -czvpf /data/data/" + Package_Facebook + "/backup.tar.gz /data/data/" + Package_Facebook + "/databases /data/data/" + Package_Facebook + "/app_light_prefs /data/data/" + Package_Facebook + "/shared_prefs /data/data/" + Package_Facebook + "/files/mobileconfig'");
                _Android.Shell("su -c 'cp /data/data/" + Package_Facebook + "/backup.tar.gz /sdcard/backup.tar.gz'");
                _Android.ADB.CMD($"pull /sdcard/backup.tar.gz \"{file}\"", 300);
                _Android.Shell("su -c 'rm -rf /data/data/" + Package_Facebook + "/*.tar.gz'");
                _Android.Shell("su -c 'rm -rf /sdcard/*.tar.gz'");
                if (!File.Exists(file))
                {
                    continue;
                }
                _Android.Device.Status = "Đã backup facebook thành công";
                return true;
            }
            return false;
        }
        public bool RestoreFacebook(string file)
        {
            if (!File.Exists(file) || Path.GetExtension(file) != ".gz") return false;

            string fileName = Path.GetFileName(file);
            string escapedFile = file.Replace("\\", "/");

            _Android.Shell($"am force-stop {Package_Facebook}");
            _Android.Shell($"su -c 'killall -9 {Package_Facebook}'");

            for (int i = 0; i < 2; i++)
            {
                _Android.Device.Status = "Đang restore Facebook...";
                _Android.ADB.CMD($"push \"{escapedFile}\" /sdcard/{fileName}", 100);

                _Android.Shell($"su -c 'rm -rf /data/data/{Package_Facebook}/*'");
                _Android.Shell($"su -c 'cp /sdcard/{fileName} /data/data/{Package_Facebook}/profile.tar.gz'");
                _Android.Shell(" su -c 'tar -xpf /data/data/" + Package_Facebook + "/profile.tar.gz'");
                _Android.ADB.Shell($"su -c \"sh -c 'tar -xzf /data/data/{Package_Facebook}/profile.tar.gz -C /data/data/{Package_Facebook}/'\"");
                string owner = _Android.Shell($"su -c 'stat -c \"%U:%G\" /data/data/{Package_Facebook}'");
                if (string.IsNullOrWhiteSpace(owner)) continue;

                _Android.Shell($"su -c 'chown -R {owner} /data/data/{Package_Facebook}'");
                _Android.Shell($"su -c 'chmod -R 771 /data/data/{Package_Facebook}'");
                _Android.Shell($"su -c 'restorecon -R /data/data/{Package_Facebook}'");
                _Android.AppStart(Package_Facebook);
                _Android.LogHelper.SUCCESS("Restore Facebook thành công");
                return true;
            }

            return false;
        }


        public bool BackupInstagram(string file)
        {
            for (int i = 0; i < 10; i++)
            {
                _Android.Device.Status = "Đang backup instagram...";
                _Android.Shell($"su -c \"rm -rf /data/data/{PackageInstagram}/*.tar.gz\"");
                _Android.Shell("su -c \"rm -rf /sdcard/*.tar.gz\"");
                _Android.ADB.Shell($"su -c 'tar -czvpf /sdcard/backup.tar.gz -C / /data/misc/keystore/user_0 /data/data/{PackageInstagram}'", 300);
                _Android.ADB.CMD($"pull /sdcard/backup.tar.gz \"{file}\"", 300);
                _Android.ADB.Shell($"su -c \"rm -rf /data/data/{PackageInstagram}/*.tar.gz\"");
                _Android.ADB.Shell("su -c \"rm -rf /sdcard/*.tar.gz\"");
                if (File.Exists(file))
                {
                    _Android.Device.Status = "Đã backup instagram thành công";
                    return true;
                }
            }
            return false;
        }
        public bool RestoreInstagram(string file)
        {
            string fileName = Path.GetFileName(file);
            string escapedFile = file.Replace("\\", "/");

            _Android.ADB.Shell($"am force-stop {PackageInstagram}");
            _Android.ADB.Shell($"su -c 'killall -9 {PackageInstagram}'");

            for (int i = 0; i < 5; i++)
            {
                _Android.Device.Status = "Đang restore instagram...";

                // Đẩy file vào thiết bị
                string pushResult = _Android.ADB.CMD($"push \"{escapedFile}\" /data/{fileName}", 60);
                if (!pushResult.ToLower().Contains("kb/s") && !pushResult.ToLower().Contains("mb/s"))
                {
                    _Android.LogHelper.ERROR("Push file không thành công.");
                    continue;
                }
                string tarResult = _Android.ADB.Shell($"su -c 'tar -xzpf /data/{fileName} -C /'");
                if (!string.IsNullOrEmpty(tarResult) && tarResult.ToLower().Contains("error"))
                {
                    _Android.LogHelper.ERROR("Giải nén thất bại: " + tarResult);
                    continue;
                }
                string check = _Android.ADB.Shell($"su -c 'ls /data/data/{PackageInstagram}'", 30);
                if (!string.IsNullOrEmpty(check))
                {
                    string owner = _Android.ADB.Shell($"su -c 'stat -c \"%U:%G\" /data/data/{PackageInstagram}'", 30);
                    if (!string.IsNullOrEmpty(owner))
                    {
                        _Android.ADB.Shell($"su -c 'chown -R {owner} /data/data/{PackageInstagram}'", 30);
                    }
                    _Android.Device.Status = "Đã restore instagram thành công...";
                    _Android.ADB.Shell("su -c 'rm -f /data/*.tar.gz'");
                    _Android.AppStart(PackageInstagram);
                    return true;
                }
            }

            return false;
        }


        public bool BackupTikTok(string file)
        {
            _Android.StopApp(PackageTikTok);
            for (int i = 0; i < 10; i++)
            {
                _Android.Device.Status = "Đang backup TikTok...";
                _Android.Shell($"su -c \"rm -rf /data/data/{PackageTikTok}/cache\"");
                _Android.Shell($"su -c \"rm -rf /data/data/{PackageTikTok}/*.tar.gz\"");
                _Android.Shell("su -c \"rm -rf /sdcard/*.tar.gz\"");
                _Android.Delay(1);

                // 2. Nén thư mục TikTok
                _Android.ADB.Shell($"su root sh -c 'tar -czvpf /sdcard/backup.tar.gz -C /data/data/{PackageTikTok} .'", 300);
                _Android.Delay(1);

                // 5. Pull về máy tính
                _Android.ADB.CMD($"pull /sdcard/backup.tar.gz \"{file}\"", 300);
                _Android.Delay(1);

                // 6. Dọn dẹp
                _Android.ADB.Shell($"su -c \"rm -rf /data/data/ {PackageTikTok}/*.tar.gz\"");
                _Android.ADB.Shell("su -c \"rm -rf /sdcard/*.tar.gz\"");

                // 7. Kiểm tra file thành công
                if (File.Exists(file))
                {
                    _Android.Device.Status = "Đã backup TikTok thành công";
                    return true;
                }

                _Android.Delay(3);
            }
            return false;
        }
        public bool RestoreTikTok(string file)
        {
            string shellOutput = string.Empty;
            _Android.StopApp(PackageTikTok);
            string fileName = Path.GetFileName(file);
            string escapedFile = file.Replace("\\", "/");
            bool flag = false;
            for (int i = 0; i < 2; i++)
            {
                _Android.Device.Status = "Đang restore TikTok...";
                //    _Android.Push(file, $"/sdcard/{fileName}");
                shellOutput = _Android.ADB.CMD($"push \"{escapedFile}\" /sdcard/{fileName}", 100);

                shellOutput = _Android.ADB.Shell($"su -c 'cp /sdcard/{fileName} /data/data/" + PackageTikTok + $"/{fileName}'", 300);
                _Android.Delay(1);
                shellOutput = _Android.ADB.Shell("su -c 'tar -xpf /data/data/" + PackageTikTok + $"/{fileName}'", 300);
                _Android.Delay(1);
                shellOutput = _Android.ADB.Shell($"su -c \"sh -c 'tar -xzf /data/data/{PackageTikTok}/{fileName} -C /data/data/{PackageTikTok}/'\"", 300);
                _Android.Delay(1);
                shellOutput = _Android.Shell("su -c \"ls -l /data/data | grep " + PackageTikTok + " | awk '{print $3\\\":\\\"$4}'\"", 30);
                flag = shellOutput != "";
                _Android.Delay(1);
                shellOutput = _Android.Shell("su -c chown -R " + shellOutput + " /data/data/" + PackageTikTok, 30);
                if (!flag)
                {
                    continue;
                }
                _Android.Device.Status = "Đã restore TikTok thành công";
                return true;
            }
            return false;
        }


    }
}
