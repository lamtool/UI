﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace AutoAndroid
{
    public class MaxChangeService
    {
        ADBClient service;
        private readonly string path_MaxChange = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App", "LamToolChanger.apk");
        private readonly string path_DeviceInfoHW = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App", "DeviceInfoHW.apk");
        public static string package_MaxChange = "com.lamtool.changer";
        private readonly string package_Deviceinfohw = "ru.andr7e.deviceinfohw";
        public MaxChangeService(ADBClient service)
        {
            this.service = service;
        }
        private void Close()
        {
            service.StopApp(package_MaxChange);

        }
        public void Open()
        {
            for (int i = 0; i < 5; i++)
            {
                service.LogHelper.SUCCESS("Đang mở ứng dụng LamToolChanger");
                service.AppStart(package_MaxChange, true, true, wait: true);
                service.SetSize();
                if (service.AppWait(package_MaxChange))
                {
                    service.LogHelper.SUCCESS("Đã mở ứng dụng LamToolChanger");
                    break;
                }
            }

        }
        public bool Install()
        {
            List<string> list = service.AppList();
            for (int i = 0; i < 5; i++)
            {
                if (list.Contains(package_Deviceinfohw) && list.Contains(package_MaxChange))
                {
                    break;
                }
                service.LogHelper.SUCCESS("Cài đặt LamToolChanger");
                list = service.AppList();
                if (!list.Contains(package_Deviceinfohw))
                {
                    service.InstallApp(path_DeviceInfoHW);
                }
                if (!list.Contains(package_MaxChange))
                {
                    service.InstallApp(path_MaxChange);
                }
                Thread.Sleep(1000);
            }
            service.Shell($"pm grant {package_MaxChange} android.permission.READ_EXTERNAL_STORAGE");
            service.Shell($"pm grant {package_MaxChange} android.permission.WRITE_EXTERNAL_STORAGE");
            SetEnableModule();
            return true;
        }
        public bool Change(string pathDevice, bool backup, string brand, string country)
        {
            Install();
            bool flag = false;
            if (Restore(pathDevice))
            {
                string text = GetInfoDeviceName(10);
                flag = text != "";
            }
            if (!flag && (flag = ChangeDeviceName(brand, country)))
            {
                if (backup)
                {

                    BackupDeviceInfoChange(pathDevice);
                }

            }
            return flag;
        }
        private bool Restore(string pathDevice)
        {

            if (string.IsNullOrEmpty(pathDevice) || !File.Exists(pathDevice))
            {
                return false;
            }
            string fileName = Path.GetFileName(pathDevice);
            service.LogHelper.SUCCESS($"Restore device: {pathDevice}");
            service.Shell($" pm grant {package_MaxChange} android.permission.READ_EXTERNAL_STORAGE");
            service.Shell($" pm grant {package_MaxChange} android.permission.WRITE_EXTERNAL_STORAGE");
            bool flag = false;
            for (int i = 0; i < 10; i++)
            {
                service.Push(pathDevice, "/sdcard");
                string text2 = service.Shell($" su -c cp /sdcard/{fileName} /data/data/{package_MaxChange}/{fileName}");
                text2 = service.Shell($" su -c tar -xzvf /data/data/{package_MaxChange}/{fileName}");
                string text = " awk '{print $3\\\":\\\"$4}'\"";
                text2 = service.Shell($" su -c \"ls -l /data/data | grep {package_MaxChange} | {text}");
                flag = text2 != "";
                text2 = service.Shell($" su -c chown -R " + text2 + $" /data/data/{package_MaxChange}");
                if (!flag)
                {
                    Thread.Sleep(2000);
                    continue;
                }
                return true;
            }
            return false;
        }
        public string GetInfoDeviceName(int waitTimeInSeconds = 0)
        {
            //AppendLog("Get device name fake...");
            int startTime = Environment.TickCount;

            do
            {
                // Execute shell command to read the content of Device.xml,
                // which located within the shared_prefs directory of the {package_MaxChange} application.
                string fileContent = service.Shell($" su -c 'cat /data/data/{package_MaxChange}/shared_prefs/Device.xml'");

                if (fileContent != "")
                {
                    try
                    {
                        // Load the content of Device.xml into an XmlDocument
                        XmlDocument xmlDocument = new XmlDocument();
                        xmlDocument.LoadXml(fileContent);

                        // Extract the values of 'fingerprint' and 'time_check' from the loaded XML
                        XmlNode fingerprintNode = xmlDocument.SelectSingleNode("//*[@name='fingerprint']");
                        XmlNode timeCheckNode = xmlDocument.SelectSingleNode("//*[@name='time_check']");

                        // If both nodes are found and they have values, concatenate those values 
                        if (fingerprintNode != null && fingerprintNode.InnerText != "" &&
                            timeCheckNode != null && timeCheckNode.InnerText != "")
                        {
                            string deviceInfo = fingerprintNode.InnerText + timeCheckNode.InnerText;
                            service.LogHelper.SUCCESS($"{deviceInfo}");
                            return deviceInfo;
                        }
                    }
                    catch (Exception)
                    {
                        // Ignore exceptions, which might be due to incorrect XML format or missing elements.
                    }
                }

                if (waitTimeInSeconds == 0)
                {
                    break;
                }

                Thread.Sleep(2000);
            } while (Environment.TickCount - startTime < waitTimeInSeconds * 1000);

            return "";
        }

        private bool ChangeDeviceName(string brand, string country)
        {

            try
            {
                service.Shell($"pm grant {package_MaxChange} android.permission.READ_EXTERNAL_STORAGE");
                service.Shell($"pm grant {package_MaxChange} android.permission.WRITE_EXTERNAL_STORAGE");

                string text2 = $"am broadcast -a {package_MaxChange}.CHANGE -n {package_MaxChange}/.AdbCaller --es brand {brand} --ez on true --ez country {country}";
                bool flag2 = service.Shell(text2).Contains("Broadcast completed");
                if (flag2)
                {
                    string text4 = GetInfoDeviceName(10);
                    service.LogHelper.SUCCESS($"Thay đổi thành công [{text4}]");
                    return flag2;
                }
            }
            catch (Exception ex)
            {
                service.LogHelper.ERROR(ex.Message);
            }

            return true;
        }
        private bool BackupDeviceInfoChange(string pathDevice)
        {
            string fileName = Path.GetFileName(pathDevice);
            service.LogHelper.SUCCESS($"Backup change device: {pathDevice}");
            string directoryPath = Path.GetDirectoryName(pathDevice);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            FileHelper.DeleteFile(pathDevice);
            //  Close();
            for (int i = 0; i < 10; i++)
            {
                service.Shell($" su -c tar -czvf /data/data/{package_MaxChange}/{fileName} /data/data/{package_MaxChange}/shared_prefs/*");
                service.Shell($" su -c cp /data/data/{package_MaxChange}/{fileName} /sdcard/{fileName}");
                ProcessHelper.RunAdbWithTimeout($" -s {service.Device.Serial} pull /sdcard/{fileName} \"" + pathDevice + "\"");
                service.Shell($" su -c rm -rf /data/data/{package_MaxChange}/*.tar.gz");
                service.Shell($" su -c rm -rf /sdcard/*.tar.gz");
                if (!File.Exists(pathDevice))
                {
                    Thread.Sleep(3000);
                    continue;
                }
                return true;
            }
            return false;
        }
        public bool CheckInternet()
        {
            try
            {
                Install();
                service.Shell($"pm grant {package_MaxChange} android.permission.READ_EXTERNAL_STORAGE");
                service.Shell($"pm grant {package_MaxChange} android.permission.WRITE_EXTERNAL_STORAGE");

                string output = service.Shell($"am broadcast -a {package_MaxChange}.INTERNET -n {package_MaxChange}/.AdbCaller");

                if (string.IsNullOrEmpty(output))
                    return false;

                // Kiểm tra kết quả broadcast
                if (output.Contains("result=1"))
                {
                    // Tách message trong data="..."
                    Match match = Regex.Match(output, @"data=""(.*?)""");
                    if (match.Success)
                    {
                        string message = match.Groups[1].Value;
                        service.LogHelper.SUCCESS($"Internet status: {message}");
                    }

                    return true;
                }
                else
                {
                    // Nếu có result=0, cũng có thể tách message để log lỗi
                    Match match = Regex.Match(output, @"data=""(.*?)""");
                    if (match.Success)
                    {
                        string message = match.Groups[1].Value;
                        service.LogHelper.ERROR($"Internet status: {message}");
                    }
                }
            }
            catch (Exception ex)
            {
                service.LogHelper.ERROR($"Kiểm tra internet thất bại [{ex.Message}]");
            }

            return false;
        }
        private List<Modules> GetModules()
        {
            string command = "su -c \"sqlite3 /data/adb/lspd/config/modules_config.db 'SELECT mid, module_pkg_name, enabled FROM modules;'\"";
            string output = service.Shell(command);

            List<Modules> modules = new List<Modules>();
            output = service.Shell(command);
            // Phân tách chuỗi kết quả và tạo đối tượng Module
            string[] rows = output.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string row in rows)
            {
                var columns = row.Split('|');
                if (columns.Length == 3)
                {
                    Modules module = new Modules
                    {
                        mid = int.Parse(columns[0].Trim()),
                        modulePkgName = columns[1].Trim(),
                        enabled = int.Parse(columns[2].Trim())
                    };
                    modules.Add(module);
                }
            }

            return modules;
        }
        private List<Scope> GetScopes()
        {
            string command = "su -c \"sqlite3 /data/adb/lspd/config/modules_config.db 'SELECT mid, app_pkg_name, user_id FROM scope;'\"";
            string output = service.Shell(command);

            List<Scope> modules = new List<Scope>();

            // Phân tách chuỗi kết quả và tạo đối tượng Module
            string[] rows = output.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string row in rows)
            {
                var columns = row.Split('|');
                if (columns.Length == 3)
                {
                    Scope module = new Scope
                    {
                        mid = int.Parse(columns[0].Trim()),
                        app_pkg_name = columns[1].Trim(),
                        user_id = int.Parse(columns[2].Trim())
                    };
                    modules.Add(module);
                }
            }

            return modules;
        }
        private void UpdateModules(Modules module)
        {
            string command = $"su -c \"sqlite3 /data/adb/lspd/config/modules_config.db 'UPDATE modules SET enabled = 1 WHERE module_pkg_name = \\\"{module.modulePkgName}\\\";'\"\r\n";
            string output = service.Shell(command);
        }
        private void addMissingScopes(Modules module)
        {
            string[] requiredApps = { MaxChangeService.package_MaxChange, "com.facebook.katana", "ru.andr7e.deviceinfohw" };
            foreach (string requiredApp in requiredApps)
            {
                string command = $"su -c \"sqlite3 /data/adb/lspd/config/modules_config.db 'INSERT INTO scope (mid, app_pkg_name, user_id) VALUES ({module.mid}, \\\"{requiredApp}\\\", 0);'\"";
                string output = service.Shell(command);
            }
        }
        public void SetEnableModule()
        {
            List<Modules> modules = GetModules();
            if (!modules.Any())
            {
                modules = GetModules();
                service.LogHelper.ERROR("Chưa cài LSPosed");
                return;
            }
            Modules targetModule = modules.First(x => x.modulePkgName == package_MaxChange);
            if (targetModule == null)
            {
                service.LogHelper.ERROR("Chưa cài DTAChange");
                return;
            }
            if (targetModule.enabled == 0)
            {
                UpdateModules(targetModule);
            }
            addMissingScopes(targetModule);
        }
        public string GetIP()
        {
            string ip = "";
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    service.LogHelper.SUCCESS($"Đang check ip [{i + 1}]");
                    string result = service.Shell($"am broadcast -a {package_MaxChange}.GET_DEVICE_IP -n {package_MaxChange}/.AdbCaller");

                    if (string.IsNullOrEmpty(result) || !result.Contains("data") || result.Contains("Error"))
                    {
                        ip = "Error: " + result;
                        service.LogHelper.ERROR(ip);
                        continue;
                    }

                    string pattern = @"data=""({.*})""";
                    var match = Regex.Match(result, pattern);
                    if (match.Success)
                    {
                        string jsonData = match.Groups[1].Value;
                        var data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(jsonData);

                        if (data != null && data.TryGetValue("ip", out string ipValue))
                        {
                            ip = ipValue;
                            service.LogHelper.SUCCESS($"IP: {ip}");
                            return ip;
                        }
                        else
                        {
                            ip = "Không tìm thấy trường 'ip'";
                            service.LogHelper.ERROR(ip);
                        }
                    }
                }
                catch (Exception ex)
                {
                    service.LogHelper.ERROR("Lỗi khi check IP: " + ex.Message);
                }
            }
            return ip;
        }
    }
    public class Modules
    {
        public int mid { get; set; }
        public string modulePkgName { get; set; }
        public int enabled { get; set; }
    }
    public class Scope
    {
        public int mid;
        public string app_pkg_name;
        public int user_id;
    }
}
