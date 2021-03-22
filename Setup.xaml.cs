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
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using Ookii.Dialogs.Wpf;

namespace ImageConverter
{
    /// <summary>
    /// Interaction logic for Setup.xaml
    /// </summary>
    public partial class frm_Setup : Window
    {

        private bool CanCheckSaveConfigButtonState = false;

        public frm_Setup()
        {
            InitializeComponent();
        }

        // checking the correctness of entering a key in the text box. this method returns true if the user press the number key not the character key into the text box
        private bool TextIsDigit(TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            return regex.IsMatch(e.Text);
        }

        // Set form field values: IsConfigFile = true - values get from the user config file; IsConfigFile = false - used default values from the static class PicsConfig
        private void SetupControlValues(bool IsConfigFile = true)
        {
            
            CanCheckSaveConfigButtonState = false;
            
            if (IsConfigFile)
            {
                tb_MainPicW.Text = PicsConfig.MainPic_Width.ToString();
                tb_MainPicH.Text = PicsConfig.MainPic_Height.ToString();
                tb_StandartW.Text = PicsConfig.StandartPic_Width.ToString();
                tb_StandartH.Text = PicsConfig.StandartPic_Height.ToString();
                tb_SmallPicW.Text = PicsConfig.SmallPic_Width.ToString();
                tb_SmallPicH.Text = PicsConfig.SmallPic_Height.ToString();
                tb_ExportDirectory.Text = PicsConfig.DefaultOutputDirectory;
            }
            else
            {
                tb_MainPicW.Text = PicsConfig.MainPicWidth.ToString();
                tb_MainPicH.Text = PicsConfig.MainPicHeight.ToString();
                tb_StandartW.Text = PicsConfig.StandartPicWidth.ToString();
                tb_StandartH.Text = PicsConfig.StandartPicHeight.ToString();
                tb_SmallPicW.Text = PicsConfig.SmallPicWidth.ToString();
                tb_SmallPicH.Text = PicsConfig.SmallPicHeight.ToString();
                tb_ExportDirectory.Text = PicsConfig.UserDesktopPath;
            }

            CanCheckSaveConfigButtonState = true;
        }

        private bool IsValidUserData()
        {
           return ((tb_MainPicW.Text.Length != 0) &&  (tb_MainPicH.Text.Length != 0) && (tb_StandartW.Text.Length != 0) && (tb_StandartH.Text.Length != 0) &&
                   (tb_SmallPicW.Text.Length != 0) && (tb_SmallPicH.Text.Length != 0) && (tb_ExportDirectory.Text.Length != 0));
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void tb_MainPicW_Loaded(object sender, RoutedEventArgs e)
        {

            // initialization of form's fields.
            SetupControlValues();

            tb_MainPicW.Focus();

        }

        private void tb_MainPicW_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            e.Handled = TextIsDigit(e);

        }

        private void tb_MainPicH_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = TextIsDigit(e);
        }

        private void tb_StandartW_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = TextIsDigit(e);
        }

        private void tb_StandartH_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = TextIsDigit(e);
        }

        private void tb_SmallPicW_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = TextIsDigit(e);
        }

        private void tb_SmallPicH_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = TextIsDigit(e);
        }

        private void tb_ExportDirectory_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // user can not change the value of text box by keyboard. user can change the value of the text box only using the browse button.
            e.Handled = true;
        }

        private void btn_DefaultValues_Click(object sender, RoutedEventArgs e)
        {
                        
            SetupControlValues(false);
            btn_SaveConfig.IsEnabled = true;

        }

        private void btn_SaveConfig_Click(object sender, RoutedEventArgs e)
        {

            PicsConfig.MainPic_Width = Int32.Parse(tb_MainPicW.Text);
            PicsConfig.MainPic_Height = Int32.Parse(tb_MainPicH.Text);
            PicsConfig.StandartPic_Width = Int32.Parse(tb_StandartW.Text);
            PicsConfig.StandartPic_Height = Int32.Parse(tb_StandartH.Text);
            PicsConfig.SmallPic_Width = Int32.Parse(tb_SmallPicW.Text);
            PicsConfig.SmallPic_Height = Int32.Parse(tb_SmallPicH.Text);
            PicsConfig.DefaultOutputDirectory = tb_ExportDirectory.Text;

            PicsConfig.SaveConfig();

            this.DialogResult = true;

        }

        private void tb_MainPicW_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            if (CanCheckSaveConfigButtonState)  btn_SaveConfig.IsEnabled = IsValidUserData();

        }

        private void tb_MainPicH_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (CanCheckSaveConfigButtonState) btn_SaveConfig.IsEnabled = IsValidUserData();

        }

        private void tb_StandartW_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CanCheckSaveConfigButtonState) btn_SaveConfig.IsEnabled = IsValidUserData();
        }

        private void tb_StandartH_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CanCheckSaveConfigButtonState) btn_SaveConfig.IsEnabled = IsValidUserData();
        }

        private void tb_SmallPicW_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CanCheckSaveConfigButtonState) btn_SaveConfig.IsEnabled = IsValidUserData();
        }

        private void tb_SmallPicH_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CanCheckSaveConfigButtonState) btn_SaveConfig.IsEnabled = IsValidUserData();
        }

        private void tb_ExportDirectory_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CanCheckSaveConfigButtonState) btn_SaveConfig.IsEnabled = IsValidUserData();
        }

        private void btn_BrowseDirectory_Click(object sender, RoutedEventArgs e)
        {
            
            VistaFolderBrowserDialog vista_dialog = null;
            System.Windows.Forms.FolderBrowserDialog old_dialog = null;
            
            if (VistaFolderBrowserDialog.IsVistaFolderDialogSupported)
            {
                vista_dialog = new VistaFolderBrowserDialog();
                vista_dialog.Description = "Please select a folder";
                vista_dialog.UseDescriptionForTitle = true; // This applies to the Vista style dialog only, not the old dialog
                if ((bool)vista_dialog.ShowDialog(this)) tb_ExportDirectory.Text = vista_dialog.SelectedPath;
            }
            else
            {
                old_dialog = new System.Windows.Forms.FolderBrowserDialog();
                old_dialog.Description = "Please select a folder";
                if (old_dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) tb_ExportDirectory.Text = old_dialog.SelectedPath;
            }

        }
    }
}
