using System;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace GModPrePubWPF.Classes
{
    public static class LogFile
    {
        private static string executablePath = Path.GetFullPath(Assembly.GetExecutingAssembly().Location.Replace(@"\\",@"\");
        public static void WriteToFile(string logFileText)
        {
            File.AppendAllText(executablePath, logFileText);
        }
    }
}
