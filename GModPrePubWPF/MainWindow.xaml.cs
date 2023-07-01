using GModPrePubWPF.Classes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Drawing;
using System.Windows.Media;
using System.IO;
using System;
using System.Data;
using System.Threading.Tasks;

namespace GModPrePubWPF
{
    public partial class MainWindow : Window
    {

        private bool firstTime = true;

        GMA gma;
        public MainWindow()
        {
            InitializeComponent();

            txtbxThumbnailPath.IsEnabled = false;
            btnThumbnailBrowse.IsEnabled = false;

            //Fun fact: if I dont do this it causes a null reference exception! because wpf is trying to use the selection changed method before the combo box has even been initialized! such fun!
            Delay();


            gma = new GMA();

            //Data Binding
            DataContext = new GMA { BspPath = string.Empty };

        }

        public async void Delay()
        {
            await Task.Delay(1);
            cmbbxTag1.SelectedItem = itemNone;
            cmbbxTag2.SelectedItem = item2None;
            firstTime = false;
        }


        #region Name
        private void txtbxName_Changed(object sender, TextChangedEventArgs e)
        {
            gma.Name = txtbxName.Text;
        }

        private void txtbxName_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter) 
            {
                Debug.Log($"Name is '{gma.Name}'", Debug.DebugType.Information, txtInfoPanel, sclvwrConsoleScroll);
            }
        }
        private void txtbxName_LostFocus(object sender, RoutedEventArgs e)
        {
            Debug.Log($"Name is '{gma.Name}'", Debug.DebugType.Information, txtInfoPanel, sclvwrConsoleScroll);

        }
        #endregion

        #region Bsp

        private void btnBspBrowse_Click(object sender, RoutedEventArgs e)
        {
            gma.BspPath = gma.Browse(GMA.FileType.Bsp);
            txtbxBspPath.Text = gma.BspPath;
            CheckBSPPath();
        }
        private void txtbxBspPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            gma.BspPath = txtbxBspPath.Text;
        }
        private void txtbxBspPath_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                CheckBSPPath();
            }
        }
        private void txtbxBspPath_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckBSPPath();
        }

        private void CheckBSPPath()
        {
            if((gma.BspPath == null) || (gma.BspPath == string.Empty))
                Debug.Log($"BSP path is null or empty!", Debug.DebugType.Warning, txtInfoPanel, sclvwrConsoleScroll);
            else if(!File.Exists(gma.BspPath))
                Debug.Log($"BSP path is not valid!", Debug.DebugType.Error, txtInfoPanel, sclvwrConsoleScroll);
            else
                Debug.Log($"BSP path has been set to {gma.BspPath}", Debug.DebugType.Information, txtInfoPanel, sclvwrConsoleScroll);
        }

        #endregion

        #region Thumbnail

        private void chkbxUsingThumbnail_Checked(object sender, RoutedEventArgs e)
        {
            CheckUsingThumbail();
        }
        private void chkbxUsingThumbnail_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckUsingThumbail();
        }

        void CheckUsingThumbail()
        {
            if ((bool)chkbxUsingThumbnail.IsChecked)
            {
                gma.UsingThumb = true;
                txtbxThumbnailPath.IsEnabled = true;
                btnThumbnailBrowse.IsEnabled = true;
            }
            else
            {
                gma.UsingThumb = false;
                txtbxThumbnailPath.IsEnabled = false;
                btnThumbnailBrowse.IsEnabled = false;
            }
            Debug.Log($"Using Thumbnail has been set to {gma.UsingThumb}", Debug.DebugType.Information, txtInfoPanel, sclvwrConsoleScroll);

        }
        private void btnThumbnailBrowse_Click(object sender, RoutedEventArgs e)
        {
            gma.ThumbPath = gma.Browse(GMA.FileType.Png);
            txtbxThumbnailPath.Text = gma.ThumbPath;
            CheckThumbnail();
        }

        private void CheckThumbnail()
        {
            if(File.Exists(gma.ThumbPath))
            {
                System.Drawing.Image thumb = System.Drawing.Image.FromFile(gma.ThumbPath);

                if (Path.GetExtension(gma.ThumbPath) != ".png")
                {
                    Debug.Log($"Incorrect image format! Use .png!", Debug.DebugType.Error, txtInfoPanel, sclvwrConsoleScroll);
                }
                else
                {
                    if (thumb.Width != thumb.Height)
                    {
                        Debug.Log($"Thumbnail aspect ratio incorrect, use 1:1! (e.g 1024x1024)", Debug.DebugType.Error, txtInfoPanel, sclvwrConsoleScroll);
                    }
                    else
                    {
                        Debug.Log($"Thumbnail at {gma.ThumbPath} has been loaded", Debug.DebugType.Information, txtInfoPanel, sclvwrConsoleScroll);
                    }
                }
            }
            else if(!File.Exists(gma.ThumbPath)) 
            {
                Debug.Log($"Thumbnail could not be loaded, use a correct path or disable thumbnail", Debug.DebugType.Warning, txtInfoPanel, sclvwrConsoleScroll);
            }
        }

        private void txtbxThumbnailPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CheckThumbnail();
            }
        }

        private void txtbxThumbnailPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            gma.ThumbPath = txtbxThumbnailPath.Text;
        }

        private void txtbxThumbnailPath_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckThumbnail();
        }

        #endregion

        #region gmad.exe
        private void txtbxGmadPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            gma.GmadPath = txtbxGmadPath.Text;
        }

        private void txtbxGmadPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CheckGmadPath();
            }
        }

        private void txtbxGmadPath_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckGmadPath();
        }
        private void btnGmadDefaultPath_Click(object sender, RoutedEventArgs e)
        {
            txtbxGmadPath.Text = @"C:\Program Files (x86)\Steam\steamapps\common\GarrysMod\bin\gmad.exe";
            gma.GmadPath = txtbxGmadPath.Text;
            CheckGmadPath();
        }
        private void btnGmadBrowse_Click(object sender, RoutedEventArgs e)
        {
            gma.GmadPath = gma.Browse(GMA.FileType.Exe);
            txtbxGmadPath.Text = gma.GmadPath;
            CheckGmadPath();
        }

        private void CheckGmadPath()
        {
            if(!File.Exists(gma.GmadPath))
            {
                Debug.Log($"Gmad path is invalid!", Debug.DebugType.Error, txtInfoPanel, sclvwrConsoleScroll);
            }
            else if((gma.GmadPath == null) || (gma.GmadPath == string.Empty))
            {
                Debug.Log($"Gmad path is null or empty", Debug.DebugType.Warning, txtInfoPanel, sclvwrConsoleScroll);
            }
            else
            {
                Debug.Log($"Gmad path has been set to {gma.GmadPath}", Debug.DebugType.Information, txtInfoPanel, sclvwrConsoleScroll);
            }
        }

        #endregion

        #region JSON Tags

        private void cmbbxTag1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(!firstTime)
            {
                if (cmbbxTag1.SelectedItem == itemNone)
                {
                    gma.Tag1 = null;
                }
                else
                {
                    gma.Tag1 = cmbbxTag1.SelectedItem.ToString();
                    CheckTags(1, gma.Tag1, true);
                }
            }
            
        }

        private void cmbbxTag2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(!firstTime)
            {
                if(cmbbxTag2.SelectedItem == item2None)
                {
                    gma.Tag2 = null;
                }
                else
                {
                    gma.Tag2 = cmbbxTag2.SelectedItem.ToString();
                    CheckTags(2, gma.Tag2, true);
                }
            }
        }

        private void CheckTags(int TagNumber, string boxInput, bool noTag)
        {
            if(noTag)
            {
                Debug.Log($"Tag {TagNumber} has been set to None", Debug.DebugType.Information, txtInfoPanel, sclvwrConsoleScroll);
            }
            else
            {
                string boxOutput = boxInput.Remove(0, 38);
                Debug.Log($"Tag {TagNumber} has been set to {boxOutput}", Debug.DebugType.Information, txtInfoPanel, sclvwrConsoleScroll);
            }
            

        }

        #endregion


        #region Output and GMA Creation
        private void txtbxOutputDirectory_TextChanged(object sender, TextChangedEventArgs e)
        {
            gma.OutputDirectory = txtbxOutputDirectory.Text;
        }

        private void txtbxOutputDirectory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CheckOutputDirectory();
            }
        }

        private void txtbxOutputDirectory_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckOutputDirectory();
        }

        private void btnOutputDefaultPath_Click(object sender, RoutedEventArgs e)
        {
            gma.OutputDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            txtbxOutputDirectory.Text = gma.OutputDirectory;
            CheckOutputDirectory();
        }

        private void btnOutputBrowse_Click(object sender, RoutedEventArgs e)
        {
            gma.GmadPath = gma.Browse(GMA.FileType.Folder);
            txtbxOutputDirectory.Text = gma.OutputDirectory;
            CheckOutputDirectory();
        }

        private void btnCreateGMA_Click(object sender, RoutedEventArgs e)
        {
            Debug.Log($"Launching gmad.exe...", Debug.DebugType.Information, txtInfoPanel, sclvwrConsoleScroll);
            gma.Create(gma.Name, gma.BspPath, gma.ThumbPath, gma.GmadPath, gma.OutputDirectory);

        }

        private void CheckOutputDirectory()
        {
            Debug.Log($"Output directory has been set to {gma.OutputDirectory}", Debug.DebugType.Information, txtInfoPanel, sclvwrConsoleScroll);
        }

        #endregion


    }
}
