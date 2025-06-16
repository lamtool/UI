using Newtonsoft.Json.Linq;

namespace Sunny.Subdy.AutoUpdate.Api
{
    public class LamTool_API
    {
        private string _key;
        private string _nameApp;
        private string _versionApp;
        public string _updateUrl;
        public string _newVersion;
        public LamTool_API(string key, string nameApp, string version)
        {
            _key = key;
            _nameApp = nameApp;
            _versionApp = version;
        }
        public async Task<bool> GetApiResponseAsync()
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://lamtool.net/api/license/check?tool_slug={_nameApp}&device_code={_key}");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                var obj = JObject.Parse(json);
                bool success = obj["success"]?.Value<bool>() ?? false;
                _newVersion = obj["license"]?["tool"]?["version"]?.ToString() ?? "";
                _updateUrl = obj["license"]?["tool"]?["updateUrl"]?.ToString() ?? "";
                return success;

            }
            catch
            {
                
            }
            return false;
        }

        public bool IsNewerVersion()
        {
            string[] currentVersionParts = _versionApp.Split('.');
            string[] newVersionParts = _newVersion.Split('.');

            Array.Reverse(currentVersionParts);
            Array.Reverse(newVersionParts);

            int len = Math.Max(currentVersionParts.Length, newVersionParts.Length);

            for (int i = 0; i < len; i++)
            {
                int currentPart = i < currentVersionParts.Length ? int.Parse(currentVersionParts[i]) : 0;
                int newPart = i < newVersionParts.Length ? int.Parse(newVersionParts[i]) : 0;

                if (currentPart < newPart)
                    return true;
                if (currentPart > newPart)
                    return false;
            }

            return false; // bằng nhau
        }
    }
}
