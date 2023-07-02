using System;
using System.IO;
using System.Reflection;

namespace GModPrePubWPF.Classes
{
    public static class LogFile
    {
        private static string executablePath = Path.GetFullPath(Assembly.GetExecutingAssembly().Location);

        public static string finalPath = executablePath.Replace(@"\\", @"\").Remove(executablePath.Length - 18, 18);

        public static string currentSessionTime = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}";
        public static void WriteToFile(string logFileText)
        {
            Directory.CreateDirectory($@"{finalPath}\LogFiles");
            File.AppendAllText($@"{finalPath}\LogFiles\ConsoleOutput {currentSessionTime}.log", $"{logFileText}\n");
        }
    }
}
