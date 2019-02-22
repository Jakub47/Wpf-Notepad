using Notepad.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public static Window window = new Window();
        public string temporaryString { get; set; } = "";
        public int IndexOfSelection { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            IndexOfSelection = -1;
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
            //List<Word> words = new List<Word>();
            ////txtMainArea.Text.Split(new[] { Environment.NewLine },StringSplitOptions.None).ToList().ForEach(a => words.Add(new Word(a)));

            //using (System.IO.StringReader reader = new System.IO.StringReader(txtMainArea.Text))
            //{
            //    while(true)
            //    {
            //        var temp = reader.ReadLine();
            //        if(temp != null)
            //        {
            //            words.Add(new Word(temp));
            //        }
            //        else
            //        {
            //            break;
            //        }
            //    }
            //}

            //int index = 0;
            ////Lista stringow w kazdej liscie poszukaj zadanej frazy. I wez pierwszy element
            //foreach(var word in words)
            //{
            //    if(word.FindElement("F"))
            //    {
            //        break;
            //    }
            //    index++;
            //}
            ////index +=1;

            //for(int i = 0;i<index;i++)
            //{
            //    MainScroll.LineDown();
            //}

            FindAndReplace findAndReplace = new FindAndReplace();
            findAndReplace.Show();
            
            //int numberOfColumns = txtMainArea.LineCount;


            //for(int i = 0;i<=numberOfColumns;i++)
            //{
            //   //var c =  txtMainArea.GetCharacterIndexFromLineIndex(i);
            //}

            //MainScroll.Focus();
            //MainScroll.HorizontalOffset
            //MainScroll.LineDown();
            //MainScroll.ScrollToVerticalOffset();
            //txtMainArea.Text.First(a => a == 'f');
            //MainScroll.ScrollToVerticalOffset(7);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            foreach(Window win in App.Current.Windows)
            {
                win.Close();
            }
        }

        public void FindText(string text)
        {
            if (txtMainArea.Text != String.Empty)
            {
                txtMainArea.Select(txtMainArea.Text.IndexOf(text), text.Length);
            }
        }

        public void FindNext(string text)
        {
            if (txtMainArea.SelectedText != string.Empty && txtMainArea.SelectedText.Contains(text))
            {
                Regex rg = new Regex(@"[" + text + @"]",RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection matches = rg.Matches(txtMainArea.Text);
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    for (int i = 0; i < groups.Count; i++)
                    {
                        //All search occurnes of given index;
                        //groups[i].Value returns value
                        //groups[i].Index returns index where text starts
                        if(IndexOfSelection == -1)
                        {
                            IndexOfSelection = groups[i].Index;
                            txtMainArea.Select(IndexOfSelection, text.Length);
                        }
                        else
                        {
                            if(i >= IndexOfSelection)
                            {
                                IndexOfSelection = groups[i].Index;
                                txtMainArea.Select(IndexOfSelection, text.Length);
                            }
                        }
                    }
                }

            }
            else
            {
                IndexOfSelection = -1;
                FindText(text);
            }


            ////Get Text
            //string tempororary = txtMainArea.Text;
            ////Get Selectected Text
            //int selectedTextPosition = txtMainArea.Text.IndexOf(txtMainArea.SelectedText);

            //int c = txtMainArea.SelectionStart;
            
            //if(selectedTextPosition.ToLower() == text.ToLower())
            //{
            //    tempororary = tempororary.Remove(tempororary.IndexOf(text), text.Length);
            //    txtMainArea.Select(tempororary.IndexOf(text) + 1, text.Length);
            //}


            //if (txtMainArea.SelectedText == String.Empty)
            //{
            //    if (txtMainArea.Text != String.Empty)
            //    {
            //        txtMainArea.Select(txtMainArea.Text.IndexOf(text), text.Length);
            //    }
            //}
            //else
            //{
            //    //Now we know what some text is selected
            //    //Get the index of selected item
            //    int index = txtMainArea.Text.IndexOf(txtMainArea.SelectedText);

            //    //Remove the selected item from temprorary rext
            //    temporaryString.Remove(index, text.Length);

            //    index = temporaryString.IndexOf(text) + text.Length;

            //    txtMainArea.Select(index, text.Length);
            //}
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            window = this;
        }
    }
}
