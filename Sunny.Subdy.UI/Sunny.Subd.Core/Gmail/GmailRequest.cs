using Sunny.Subdy.Common.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sunny.Subd.Core.Gmail
{
    public class GmailRequest
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        public static async Task<Dictionary<string, string>> CheckEmailsAsync(List<string> emails)
        {
            var result = new Dictionary<string, string>();

            try
            {
                string json = "[\"" + string.Join("\",\"", emails) + "\"]";
                var content = new StringContent(json, Encoding.UTF8, "text/plain");

                var request = new HttpRequestMessage(HttpMethod.Post, "https://checker.temp-mailfree.com/check")
                {
                    Content = content
                };

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string responseJson = await response.Content.ReadAsStringAsync();
                var matches = Regex.Matches(responseJson, "\\{\\s*\"email\"\\s*:\\s*\"(.*?)\"\\s*,\\s*\"status\"\\s*:\\s*\"(.*?)\"\\s*\\}");

                foreach (Match match in matches)
                {
                    if (match.Groups.Count == 3)
                    {
                        string email = match.Groups[1].Value;
                        string status = match.Groups[2].Value;
                        result[email] = status;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }

            return result;
        }
    }
}
