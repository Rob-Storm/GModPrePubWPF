using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace GModPrePubWPF.Classes
{
    public static class Debug
    {
        public enum DebugType
        {
            Information,Warning,Error
        };

        public static void Log(string Text, DebugType Type, TextBlock TextBlock, ScrollViewer ScrollViewer)
        {
            string output = $"[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {Text}";

            switch (Type) 
            {
                case DebugType.Information:
                    TextBlock.Inlines.Add(new Run($"\nInfo: {output}") { Foreground = Brushes.CornflowerBlue });
                    break;

                case DebugType.Warning:
                    TextBlock.Inlines.Add(new Run($"\nWarning: {output}") { Foreground = Brushes.Yellow });
                    break;

                case DebugType.Error:
                    TextBlock.Inlines.Add(new Run($"\nError!: {output}") { Foreground = Brushes.Red });
                    break;

            }
            LogFile.WriteToFile(output);

            ScrollViewer.ScrollToBottom();
        }
    }
}
