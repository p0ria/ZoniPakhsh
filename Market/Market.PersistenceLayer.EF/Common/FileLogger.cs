using System;
using System.IO;
using System.Web;

namespace Market.PersistenceLayer.EF.Common
{
    public static class FileLogger
    {
        public static void Log(string msg)
        {
            string path = HttpContext.Current.Server.MapPath("~/" + "logs/log.txt");
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            File.AppendAllText(path, msg + Environment.NewLine);
        }
    }
}