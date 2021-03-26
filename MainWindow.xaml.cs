using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Drawing;
using ImageProcessor;

namespace ImageConverter
{

    #region PicsConfig class definition  
    // default image sizes and output directory
    public static class PicsConfig
    {

        static bool _ChangesMustBeSaved = false;

        public const int MainPicWidth = 250;
        public const int MainPicHeight = 166;
        public const int StandartPicWidth = 1024;
        public const int StandartPicHeight = 768;
        public const int SmallPicWidth = 200;
        public const int SmallPicHeight = 133;

        public static int MainPic_Width
        {
            get { if (Properties.Settings.Default.MainPicW == 0) return MainPicWidth; else return Properties.Settings.Default.MainPicW; }
            set
            {
                Properties.Settings.Default.MainPicW = value;
                _ChangesMustBeSaved = true;
            }
        }

        public static int MainPic_Height
        {
            get { if (Properties.Settings.Default.MainPicH == 0) return MainPicHeight; else return Properties.Settings.Default.MainPicH;   }
            set
            {
                Properties.Settings.Default.MainPicH = value;
                _ChangesMustBeSaved = true;
            }
        }

        public static int StandartPic_Width
        {
            get { if (Properties.Settings.Default.StandartPicW == 0) return StandartPicWidth; else return Properties.Settings.Default.StandartPicW; }
            set
            {
                Properties.Settings.Default.StandartPicW = value;
                _ChangesMustBeSaved = true;
            }
        }

        public static int StandartPic_Height
        {
            get { if (Properties.Settings.Default.StandartPicH == 0) return StandartPicHeight; else return Properties.Settings.Default.StandartPicH; }
            set
            {
                Properties.Settings.Default.StandartPicH = value;
                _ChangesMustBeSaved = true;
            }
        }

        public static int SmallPic_Width
        {
            get { if (Properties.Settings.Default.SmallPicW == 0) return SmallPicWidth; else return Properties.Settings.Default.SmallPicW; }
            set
            {
                Properties.Settings.Default.SmallPicW = value;
                _ChangesMustBeSaved = true;
            }
        }

        public static int SmallPic_Height
        {
            get { if (Properties.Settings.Default.SmallPicH == 0) return SmallPicHeight; else return Properties.Settings.Default.SmallPicH; }
            set
            {
                Properties.Settings.Default.SmallPicH = value;
                _ChangesMustBeSaved = true;
            }
        }

        public static string DefaultOutputDirectory
        {
            get  
            {
                if (String.IsNullOrEmpty(Properties.Settings.Default.ExportDir)) return Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                else return Properties.Settings.Default.ExportDir; 
            }
            set
            {
                Properties.Settings.Default.ExportDir = value;
                _ChangesMustBeSaved = true;
            }
        }

        public static string UserDesktopPath
        {

            get { return Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory); }

        }

        public static void SaveConfig()
        {
            if (_ChangesMustBeSaved) Properties.Settings.Default.Save();
        }

    }
    #endregion

    #region Worker class definition  
    class Worker
    {


        //converting graphic files, the names of which are specified in the FileNames array into the ResultDirectory directory.
        public static bool ConvertPhotos(ItemCollection FileNames, string ResultDirectory, IProgress<int> progress)
        {

            System.Drawing.Size MainPic, StandartPic, SmallPic;
            ImageProcessor.Imaging.Formats.ISupportedImageFormat newformat;

            StandartPic = new System.Drawing.Size(PicsConfig.StandartPicWidth, PicsConfig.StandartPicHeight);
            MainPic = new System.Drawing.Size(PicsConfig.MainPicWidth, PicsConfig.MainPicHeight);
            SmallPic = new System.Drawing.Size(PicsConfig.SmallPicWidth, PicsConfig.SmallPicHeight);
            newformat = new ImageProcessor.Imaging.Formats.JpegFormat();

            using (ImageFactory Image_Factory = new ImageFactory(preserveExifData: true))
            {

                for (int Index = 0; Index < FileNames.Count; ++Index)
                {

                    Image_Factory.Load(FileNames[Index].ToString());
                    Image_Factory.Resize(StandartPic);
                    Image_Factory.BackgroundColor(System.Drawing.Color.White);
                    Image_Factory.Format(newformat);
                    Image_Factory.Quality(100);
                    Image_Factory.Save(ResultDirectory + "\\" + (Index + 1).ToString("D4") + ".jpg");

                    if (Index == 0)
                    {
                        Image_Factory.Resize(MainPic);
                        Image_Factory.BackgroundColor(System.Drawing.Color.White);
                        Image_Factory.Save(ResultDirectory + "\\" + (Index + 1).ToString("D4") + "_f.jpg");

                    }

                    Image_Factory.Resize(SmallPic);
                    Image_Factory.BackgroundColor(System.Drawing.Color.White);
                    Image_Factory.Save(ResultDirectory + "\\" + (Index + 1).ToString("D4") + "_sm.jpg");

                    progress.Report(Index + 1);

                }

            }

            MessageBox.Show("Image conversion is completed", "Information!", MessageBoxButton.OK, MessageBoxImage.Information);
            return true;

        }

    }

    #endregion

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        private string SelectedFileName = "";  // the file in the file list that currently has focus
        private string LastFileDirOpened = ""; // the directory of last file which was added to the file list

        public MainWindow()
        {
            InitializeComponent();
        }

        // verifying that the file is an image
        private bool FileIsImage(string FileName)
        {

            bool result = false;

            try
            {
                System.Drawing.Image ImgFile = System.Drawing.Image.FromFile(FileName);
                System.Drawing.Imaging.ImageFormat ImgType = ImgFile.RawFormat;
                ImgFile.Dispose();
                result = true;
            }
            catch 
            {
                result = false;
            }
            return result;

        }


        // checking status of the removeFileFromList button and the clearList button.
        private void CheckButtonStatus()
        {
            btn_RemoveFileFromList.IsEnabled = (lb_FileList.Items.IsEmpty == false);
            btn_ClearList.IsEnabled = btn_RemoveFileFromList.IsEnabled;
            btn_StartProcess.IsEnabled = btn_RemoveFileFromList.IsEnabled;
        }

        private void btn_AddFileToList_Click(object sender, RoutedEventArgs e)
        {

            // 1. initialization
            string DuplicateFileNames = "";
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.Filter = "Іmage files|*.jpg;*.jpeg;*.png|All Files (*.*)|*.*";
            openFileDlg.DefaultExt = "*.jpg";
            openFileDlg.Multiselect = true;
            openFileDlg.Title = "Add new file";
            if (string.IsNullOrEmpty(LastFileDirOpened) == false) openFileDlg.InitialDirectory = LastFileDirOpened;
            if (openFileDlg.ShowDialog() == false) return;

            // 2. Adding files to the list. An attempt to add twice the same files or add the non-graphic files to the list is blocked. 
            foreach (string FileName in openFileDlg.FileNames)
            {
                if (lb_FileList.Items.Contains(FileName) == false)
                {
                    if (FileIsImage(FileName)) lb_FileList.Items.Add(FileName);
                    else DuplicateFileNames = DuplicateFileNames + " \n" + FileName;
                    LastFileDirOpened = System.IO.Path.GetDirectoryName(FileName);
                }
                else DuplicateFileNames = DuplicateFileNames + " \n" + FileName;
            }
            if (string.IsNullOrEmpty(DuplicateFileNames) == false)
                MessageBox.Show($"The following files: \n {DuplicateFileNames} \n\n were not added to the list because they are already in the list or are not images", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);

            // 3. enable the removeFileFromList button and the clearList button if the list of filenames is not empty.
            CheckButtonStatus();
        }

        private void lb_FileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            // Displaying the content of a graphic file by clicking on its name in the list (only if e.AddedItems != 0).
            if (e.AddedItems.Count != 0)
            {
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                SelectedFileName = (string)e.AddedItems[0];
                bi.UriSource = new Uri(SelectedFileName, UriKind.Absolute);
                bi.EndInit();
                img_CurrentFile.Source = bi;
            }
            else img_CurrentFile.Source = null;

        }

        private void btn_ClearList_Click(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("Do you really want to clear the list of graphic files?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;
            lb_FileList.Items.Clear();
            pb_Progress.Value = 0;

            CheckButtonStatus();

        }

        private void btn_RemoveFileFromList_Click(object sender, RoutedEventArgs e)
        {

            // removing the file from the file list
            if (string.IsNullOrEmpty(SelectedFileName))
            {
                MessageBox.Show("You have to select the file which will be removed from the file list!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            int SelectedFileNamePos = lb_FileList.Items.IndexOf(SelectedFileName);
            if (SelectedFileNamePos >= 0)
            {
                lb_FileList.Items.Remove(lb_FileList.Items[SelectedFileNamePos]);
                CheckButtonStatus();
            }
            else MessageBox.Show("You have to select the file which will be removed from the file list!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

        }

        private void btn_Exit_Click(object sender, RoutedEventArgs e)
        {

            Application.Current.Shutdown();

        }

        private void btn_Config_Click(object sender, RoutedEventArgs e)
        {

            frm_Setup SetupDlg = new frm_Setup();
            SetupDlg.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private async void btn_StartProcess_Click(object sender, RoutedEventArgs e)
        {
            string OutputDir = "";
            var progress = new Progress<int>(s => pb_Progress.Value = s);

            pb_Progress.Maximum = lb_FileList.Items.Count;
            OutputDir = PicsConfig.DefaultOutputDirectory + "\\result";
            btn_StartProcess.IsEnabled = await Task.Factory.StartNew<bool>(() => Worker.ConvertPhotos(lb_FileList.Items, OutputDir, progress), TaskCreationOptions.LongRunning); 
            //MessageBox.Show(lb_FileList.Items.Count.ToString());

        }
    }
}
