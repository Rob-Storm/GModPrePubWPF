using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;

namespace GModPrePubWPF.Classes
{
    internal class GMA
    {
        public string Name, BspPath, ThumbPath, GmadPath, OutputDirectory;

        public string Tag1, Tag2;

        public bool UsingThumb;

        private TextBlock _textblock;
        private ScrollViewer _scrollviewer;

        Nullable<bool> result;

        public enum FileType
        {
            Bsp,
            Folder,
            Png,
            Exe
        };

        public void Create(TextBlock TextBlock, ScrollViewer ScrollViewer)
        {
            _textblock = TextBlock;
            _scrollviewer = ScrollViewer;

            FolderSetup();
        }

        public void FolderSetup()
        {

            Debug.Log($"Setting up folders", Debug.DebugType.Information, _textblock, _scrollviewer);

            string rootFolder = OutputDirectory + "/" + Name;

            string mapsFolder = rootFolder + "/" + "maps";

            string mapFileTarget = mapsFolder + "/" + Path.GetFileName(BspPath);

            string thumbFolder = mapsFolder + "/" + "thumb";

            string customName = mapsFolder + "/" + Name;

            string customThumb = thumbFolder + "/" + Name;

            string addonFolder = OutputDirectory;

            Directory.CreateDirectory(rootFolder);
            Directory.CreateDirectory(mapsFolder);

            File.Copy(BspPath, mapFileTarget);

            File.Move(mapFileTarget, customName + ".bsp");

            if (UsingThumb)
            {
                Directory.CreateDirectory(thumbFolder);
                File.Copy(ThumbPath, thumbFolder + "/" + Path.GetFileName(ThumbPath));
                File.Move(thumbFolder + "/" + Path.GetFileName(ThumbPath), customThumb + ".png");
            }

            Debug.Log($"Creating addon.json file", Debug.DebugType.Information, _textblock, _scrollviewer);
            JSONCreation(rootFolder);
        }

        public void JSONCreation(string rootDirectory)
        {
            string[] addonTags = new string[2];

            if(Tag1 == "None")
            {
                Tag1 = string.Empty;
            }

            if (Tag2 == "None")
            {
                Tag2 = string.Empty;
            }

            addonTags[0] = Tag1;
            addonTags[1] = Tag2;

            JSONFile jsonFile = new JSONFile();
            {
                jsonFile.title = Name;

                jsonFile.type = "map";

                jsonFile.tags = addonTags;

                jsonFile.ignore = new List<string>()
                {
                    "*.psd",
                    "*.vcproj",
                    "*.svn*"
                };
            };

            string json = JsonConvert.SerializeObject(jsonFile, Formatting.Indented);

            File.WriteAllText(rootDirectory + "/" + "addon.json", json);

            Debug.Log($"Starting gmad.exe", Debug.DebugType.Information, _textblock, _scrollviewer);
            GMACreation(rootDirectory);
        }

        public void GMACreation(string rootFolder)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            Process process = new Process();
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;

            process.StartInfo.FileName = GmadPath;
            process.StartInfo.Arguments = "create " + "-folder " + rootFolder;
            process.Start();

            string output = process.StandardOutput.ReadLine();
            Debug.Log(output, Debug.DebugType.Information, _textblock, _scrollviewer);

            process.WaitForExit();
            Debug.Log($"gmad.exe has closed check {OutputDirectory} for your .gma file. There is a chance it may not have worked! For whatever reason some of the messages from gmad.exe are not being output, not my problem!", Debug.DebugType.Warning, _textblock, _scrollviewer);
            Debug.Log($"----------------------", Debug.DebugType.Information, _textblock, _scrollviewer);

        }

        public string Browse(FileType Type)
        {
            string Directory = string.Empty;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ValidateNames = false;
            openFileDialog.CheckFileExists = false;
            openFileDialog.CheckPathExists = false;


            switch (Type)
            {
                case FileType.Folder:

                    openFileDialog.FileName = "Folder Selection";
                    result = openFileDialog.ShowDialog();
                    string output;

                    if (result == true)
                    {
                        int pathLength;
                        output = openFileDialog.FileName;
                        pathLength = output.Length - 17;
                        output = output.Remove(pathLength, 17);
                        Directory = output;

                        // Jank code reused, if it aint broke dont fix it I guess
                    }

                    break;

                case FileType.Bsp:

                    openFileDialog.DefaultExt = ".bsp";
                    openFileDialog.Filter = "Compiled Map Files|*.bsp";
                    result = openFileDialog.ShowDialog();
                    if(result == true)
                    {
                        Directory = openFileDialog.FileName;
                    }

                    break;
                case FileType.Png:

                    openFileDialog.DefaultExt = ".png";
                    openFileDialog.Filter = "Portable Network Graphic Files|*.png";
                    result = openFileDialog.ShowDialog();
                    if (result == true)
                    {
                        Directory = openFileDialog.FileName;
                    }
                    break;

                case FileType.Exe:

                    openFileDialog.DefaultExt = ".exe";
                    openFileDialog.Filter = "Executable Files|*.exe";
                    result = openFileDialog.ShowDialog();
                    if (result == true)
                    {
                        Directory = openFileDialog.FileName;
                    }
                    break;

            }
            return Directory;
        }
    }

    public class JSONFile
    {
        public string title { get; set; }

        public string type { get; set; }

        public string[] tags { get; set; }

        public IList<string> ignore { get; set; }

    }

}
