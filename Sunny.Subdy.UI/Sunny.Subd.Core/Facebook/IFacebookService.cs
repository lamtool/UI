using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoAndroid;
using Sunny.Subd.Core.Models;
using Sunny.Subdy.Common.Json;
using Sunny.Subdy.Data.Models;

namespace Sunny.Subd.Core.Facebook
{
    public interface IFacebookService
    {
        Task<SubdyExtension> Login(ADBClient client, Account account, CancellationToken ct);
        Task<SubdyExtension> Reaction(ADBClient client, Account account, string type, CancellationToken ct);
        Task<SubdyExtension> Follow(ADBClient client, Account account, CancellationToken ct);
        Task<SubdyExtension> LikePage(ADBClient client, Account account, CancellationToken ct);
        Task<SubdyExtension> JoinGroup(ADBClient client, Account account, CancellationToken ct);
    }
}
