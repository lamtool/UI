using AutoAndroid;
using Sunny.Subd.Core.Models;
using Sunny.Subdy.Data.Models;

namespace Sunny.Subd.Core.Facebook
{
    public interface IFacebookService
    {
        Task<SubdyExtension> HanderAccount(ADBClient client, int timeout);
        Task<SubdyExtension> Login(ADBClient client, Account account, CancellationToken ct);
        Task<Dictionary<string, string>> GetInfo(ADBClient client);
        Task<Dictionary<string, string>> UpateInfo(ADBClient client, string fullename, string bio, string username);
    }
}
