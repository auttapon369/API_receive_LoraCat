using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace API_receive_LoraCat
{
    public static class ControllerExtension
    {
        internal enum LogType { Info, Warning, Error };
        internal static Dictionary<string, byte> methods = new Dictionary<string, byte>() { { "GET", 1 }, { "POST", 2 }, { "PUT", 3 }, { "DELETE", 4 } };

        internal static void Log(this ControllerBase controller, object obj, LogType logType = LogType.Info, [CallerMemberName] string caller = "")
        {
            try
            {
                DateTime now = DateTime.Now;
                var path = controller.Url.ActionContext.HttpContext.Request.PathBase.HasValue ? $"{controller.Url.ActionContext.HttpContext.Request.PathBase.Value}\\{controller.Url.ActionContext.RouteData.Values["controller"]}" : controller.Url.ActionContext.RouteData.Values["controller"];
                FileInfo fileInfo = new FileInfo($"D:\\Logs\\{path}\\{now.ToString("yyyyMMdd")}.txt");
                if (!fileInfo.Directory.Exists)
                    fileInfo.Directory.Create();
                using (var sw = new StreamWriter(fileInfo.FullName, true))
                {
                    sw.WriteLine($"[{now.ToString("o")}] [{logType.ToString()}] [{caller}] {JsonConvert.SerializeObject(obj)}");
                }
            }
            catch { }
        }

        internal static byte GetMethod(this ControllerBase controller)
        {
            return methods[controller.Url.ActionContext.HttpContext.Request.Method];
        }
    }
}
