using System;
using System.Collections.Generic;
using System.IO;
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


namespace Notepad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            txtBasicInfo.Text = "Line 0 Char 0 Letters 0 Vowels a e i o u";
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            CursorProp.row = txtMainArea.GetLineIndexFromCharacterIndex(txtMainArea.CaretIndex);
            CursorProp.col = txtMainArea.CaretIndex - txtMainArea.GetCharacterIndexFromLineIndex(CursorProp.row);
            ChangeStatusInfo();

            //txtBasicInfo.Text = "Line " + (row + 1) + ", Char " + (col + 1);
        }

        private void ChangeStatusInfo()
        {
            txtBasicInfo.Text = "Line " + (CursorProp.row + 1) + " Char " + (CursorProp.col + 1) + 
                                " Letters " + TextProp.LettersCount + " Vowels " + TextProp.Vowels;
        }

        private void TxtMainArea_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!this.Title.ElementAt(this.Title.Length - 1).Equals('*'))
            {
                this.Title += '*';
            }
            TextProp.LettersCount = txtMainArea.Text.Where(a => !char.IsWhiteSpace(a)).Count();
            TextProp.VowelCounter(txtMainArea.Text);
            ChangeStatusInfo();
        }

        private void SmallToBig_Click(object sender, RoutedEventArgs e)
        {
            txtMainArea.SelectedText = txtMainArea.SelectedText.ToUpper();
        }

        private void BigToSmall_Click(object sender, RoutedEventArgs e)
        {
            txtMainArea.SelectedText = txtMainArea.SelectedText.ToLower();
        }

        private void MenuOpen_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                string filename = fileDialog.FileName;
                string message = "Czy zapisac ten plik";
                var result = MessageBox.Show(message, "Save file", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if(result.ToString() == "Yes")
                {
                    Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                    saveFileDialog.FileName = this.Title;
                    saveFileDialog.DefaultExt = ".txt";
                    var Saveresult = saveFileDialog.ShowDialog();
                    if(Saveresult == true)
                    {
                        string path = saveFileDialog.FileName;
                        if (!File.Exists(path))
                        {
                            // Create a file to write to.
                            using (StreamWriter sw = File.CreateText(path))
                            {
                                sw.WriteLine(txtMainArea.Text);
                            }
                        }
                    }
                }

                using (StreamReader readLines = File.OpenText(fileDialog.FileName))
                {
                    string line;
                    string getAllLines = "";
                    while((line = readLines.ReadLine()) != null)
                    {
                        getAllLines += line;
                        getAllLines += Environment.NewLine;
                    }
                    txtMainArea.Text = getAllLines;
                    this.Title = System.IO.Path.GetFileNameWithoutExtension(fileDialog.FileName);
                }
            }
        }

        private void MenuNew_Click(object sender, RoutedEventArgs e)
        {
            string message = "Czy zapisac ten plik";
            var result = MessageBox.Show(message, "Save file", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result.ToString() == "Yes")
            {
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.FileName = this.Title;
                saveFileDialog.DefaultExt = ".txt";
                var Saveresult = saveFileDialog.ShowDialog();
                
                if (Saveresult == true)
                {
                    string path = saveFileDialog.FileName;
                    if (!File.Exists(path))
                    {
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path))
                        {
                            sw.WriteLine(txtMainArea.Text);
                        }
                    }
                }
            }
            
            txtMainArea.Text = string.Empty;
            this.Title = "New";
        }

        private void MenuSave_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.FileName = this.Title;
            saveFileDialog.DefaultExt = ".txt";
            var Saveresult = saveFileDialog.ShowDialog();
            if (Saveresult == true)
            {
                string path = saveFileDialog.FileName;
                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(txtMainArea.Text);
                    }
                }

                this.Title = System.IO.Path.GetFileNameWithoutExtension(saveFileDialog.FileName);
            }
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            var c = txtMainArea.Text != string.Empty;
            bool  g = this.Title.ElementAt(this.Title.Length - 1) == '*';

            if (c || g )
            {
                string message = "Czy zapisac ten plik";
                var result = MessageBox.Show(message, "Save file", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result.ToString() == "Yes")
                {
                    Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                    saveFileDialog.FileName = this.Title;
                    saveFileDialog.DefaultExt = ".txt";
                    var Saveresult = saveFileDialog.ShowDialog();

                    if (Saveresult == true)
                    {
                        string path = saveFileDialog.FileName;
                        if (!File.Exists(path))
                        {
                            // Create a file to write to.
                            using (StreamWriter sw = File.CreateText(path))
                            {
                                sw.WriteLine(txtMainArea.Text);
                            }
                        }
                    }
                }
            }
            this.Close();
        }

        private void MenuSelectAll_Click(object sender, RoutedEventArgs e)
        {
            txtMainArea.SelectAll();
        }

        private void MenuFind_Click(object sender, RoutedEventArgs e)
        {
            //MainScroll.Focus();
            //MainScroll.
        }
    }
}
