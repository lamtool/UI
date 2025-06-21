using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoAndroid;
using Sunny.Subd.Core.Models;
using Sunny.Subdy.Data.Models;

namespace Sunny.Subd.Core.Facebook.ScriptActions
{
    public interface IActionHandler
    {
        string TypeAction { get; }
        Task<SubdyExtension> ExecuteAsync(string json, Account account, ADBClient device);
    }

}
