using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace GModPrePubWPF.Classes
{
    internal class GMA
    {
        public string Name, BspPath, ThumbPath, GmadPath, OutputDirectory;

        public string Tag1, Tag2;

        public bool UsingThumb;

        Nullable<bool> result;

        public enum FileType
        {
            Bsp,
            Folder,
            Png,
            Exe
        };

        public void Create(string name, string bsppath, string thumbpath, string gmadpath, string outputdirectory)
        {

        }

        public void FolderSetup()
        {
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

            JSONCreation(rootFolder);
        }

        public void JSONCreation(string rootDirectory)
        {
            string[] addonTags = new string[2];

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
        }

        public void GMACreation(string rootFolder)
        {
            var process = Process.Start(GmadPath, "create " + "-folder " + rootFolder);

            process.WaitForExit();
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
