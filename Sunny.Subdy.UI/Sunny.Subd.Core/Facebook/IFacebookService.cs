using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoAndroid;
using Sunny.Subd.Core.Models;
using Sunny.Subdy.Data.Models;

namespace Sunny.Subd.Core.Facebook
{
    public interface IFacebookService
    {
        Task<SubdyExtension> Login(ADBClient client, Account account, string json, CancellationToken ct);
    }
}
