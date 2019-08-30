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
using System.Xml;

namespace Notepad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Window window = new Window();
        public string temporaryString { get; set; } = "";
        public int CurrentSelectedStartIndex { get; set; }
        public bool FirstElementSelected { get; set; }
        public SelectedElement SelectedElement { get; set; }
        public SelectedText SelectedText { get; set; }
        public MenuItem BackgroundColorsMenuItem { get; set; }
        public MenuItem ForeGroundColorsMenuItem { get; set; }
       
        private List<string> _colors;
        private bool _txtMainAreaWordMapped;
        private List<Key> keys;
        private bool listenForShortcut;
        public bool canOpenFindAndReplace;

        // Prevent example from ending if CTL+C is pressed.


        public MainWindow()
        {
            InitializeComponent();
            keys = new List<Key>();
            _colors = new List<string>();
            canOpenFindAndReplace = true;
            _colors.Add("White"); _colors.Add("Blue"); _colors.Add("Black"); _colors.Add("Brown"); _colors.Add("Red"); _colors.Add("Gold");
            _txtMainAreaWordMapped = false;
            CurrentSelectedStartIndex = -1;
            txtBasicInfo.Text = "Line 0 Char 0 Letters 0 Vowels a e i o u";
            SelectedElement = new SelectedElement();
            BackgroundColorsMenuItem = Window1.FindName("BackgroundColors") as MenuItem;
            ForeGroundColorsMenuItem = Window1.FindName("ForegroundColors") as MenuItem;
            
            foreach (string color in _colors)
            {
                MenuItem menuItemBackground = new MenuItem() { Header = color};
                menuItemBackground.Click += changeBackground;
                BackgroundColorsMenuItem.Items.Add(menuItemBackground);

                MenuItem menuItemForeground = new MenuItem() { Header = color };
                menuItemForeground.Click += changeForeground;
                ForeGroundColorsMenuItem.Items.Add(menuItemForeground);
            }

            
            BackgroundColors.Items.Add(new MenuItem() { Header = "Color1" });
            initialStyleSettings();
        }

        private void changeBackground(object sender,EventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            BrushConverter c = new BrushConverter();
            SolidColorBrush b =  c.ConvertFromString(item.Header.ToString()) as SolidColorBrush;
            txtMainArea.Background = b;
            //txtMainArea.Foreground = Brushes.Red;
        }

        public void changeForeground(object sender, EventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            BrushConverter c = new BrushConverter(); 
            SolidColorBrush b = c.ConvertFromString(item.Header.ToString()) as SolidColorBrush;
            txtMainArea.Foreground = b;
        }


        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        public void ChangeFontStyle(string fontType,int size)
        {
            FontFamilyConverter converter = new FontFamilyConverter();
            txtMainArea.FontFamily = converter.ConvertFromString(fontType) as FontFamily;
            txtMainArea.FontSize = size;
        }

        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(FindAndReplace))
                {
                    (window as FindAndReplace).Counter = 0;
                }
            }

            CursorProp.row = txtMainArea.GetLineIndexFromCharacterIndex(txtMainArea.CaretIndex);
            CursorProp.col = txtMainArea.CaretIndex - txtMainArea.GetCharacterIndexFromLineIndex(CursorProp.row);
            ChangeStatusInfo();
            CurrentSelectedStartIndex = txtMainArea.SelectionStart;

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
                    saveFileDialog.Filter = "Text documents (.txt)|*.txt" +
                                            "|All Files|*.*"; // Filter files by extension
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
                saveFileDialog.Filter = "Text documents (.txt)|*.txt" +
                                        "|All Files|*.*"; // Filter files by extension

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
            saveFileDialog.FileName = this.Title.Replace("*",string.Empty);
            saveFileDialog.DefaultExt = ".txt";
            saveFileDialog.Filter = "Text documents (.txt)|*.txt" +
                                    "|All Files|*.*"; // Filter files by extension

            var Saveresult = saveFileDialog.ShowDialog();
            if (Saveresult == true)
            {
                string path = saveFileDialog.FileName;
                
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        if(sender == null)
                            sw.WriteLine(txtMainArea.Text + null); // if not null will be added it will not add last letter to file
                        else
                            sw.WriteLine(txtMainArea.Text);
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
                    saveFileDialog.Filter = "Text documents (.txt)|*.txt" +
                                            "|All Files|*.*"; // Filter files by extension
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

            if(canOpenFindAndReplace)
            {
                FindAndReplace findAndReplace = new FindAndReplace();
                findAndReplace.Show();
                canOpenFindAndReplace = false;
            }
            
            
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
                if(win != this.Window1)
                    win.Close();
            }

          
        }

        public void FindFirst(string text)
        {
            if (txtMainArea.Text != String.Empty)
            {
                string pattern = @"\b(\w*work\w*)\b";
                string input = "This this work a nice day. What about this? This tastes good. I work a a dog.";
                int index = 0;
                foreach (Match match in Regex.Matches(input, pattern, RegexOptions.IgnoreCase))
                {
                    if (index == 0) txtMainArea.Select(match.Index, text.Length);
                    //txtMainArea.Select(txtMainArea.Text.IndexOf(text), text.Length);
                    //match.Index;
                }


                //FirstElementSelected = true;
            }
        }
        
        public void FindNextText(string text)
        {
            if (txtMainArea.Text != String.Empty)
            {
                string textFromCursor = "";
                string beforeCursor = "";
                int indexToSelect = 0;

                //Get substring from position from cursor to end!
                //string textFromCursor = txtMainArea.Text.Substring()

                //Biore text od curosra do końca i przed kursorem
                if (txtMainArea.SelectedText.Contains(text))
                    CurrentSelectedStartIndex += text.Length;
                 textFromCursor = txtMainArea.Text.Substring(CurrentSelectedStartIndex);
                 beforeCursor = txtMainArea.Text.Substring(0, CurrentSelectedStartIndex);


                //w tekscie po kursorze znajduję pierwszy index textu i zamianiam go na tyldę
                indexToSelect = textFromCursor.IndexOf(text);
                if(indexToSelect < 0)
                {
                    CurrentSelectedStartIndex = 0;
                    textFromCursor = txtMainArea.Text.Substring(CurrentSelectedStartIndex);
                    beforeCursor = txtMainArea.Text.Substring(0, CurrentSelectedStartIndex);
                    indexToSelect = textFromCursor.IndexOf(text);

                    if(indexToSelect < 0)
                    {
                        MessageBox.Show("Brak wystąpienia danego elementu");
                        return;
                    }
                }
                textFromCursor = textFromCursor.Remove(textFromCursor.IndexOf(text),text.Length);
                textFromCursor = textFromCursor.Insert(indexToSelect, '~'.ToString());

                //textFromCursor = textFromCursor.Replace(text, '~'.ToString());
                //(int i = 0; i < textFromCursor.Length; i++)
                //{
                //    if(textFromCursor[i] == '~')
                //    {
                //        textFromCursor.Substring(i)
                //    }
                //}

                //Teraz dołączam text
                string newString = String.Empty;

                if (beforeCursor == String.Empty)
                    newString = textFromCursor;
                else
                    newString = beforeCursor +  textFromCursor;

                //Zamianiam tylde na zadane słowo i je zaznaczam
                int indexOfTilde = newString.IndexOf('~');
                newString = newString.Remove(indexOfTilde, 1);
                newString = newString.Insert(indexOfTilde, text);

                txtMainArea.Text = newString;
                txtMainArea.Select(indexOfTilde, text.Length);
                
                //foreach (Window window in Application.Current.Windows)
                //{
                //    if (window.GetType() == typeof(FindAndReplace))
                //    {
                //        index = (window as FindAndReplace).Counter;
                //    }
                //}



                //foreach (Match match in Regex.Matches(textFromCursor, pattern, RegexOptions.IgnoreCase))
                //{
                //    if (index == count)
                //    {
                //        txtMainArea.Select(match.Index, text.Length);
                //    }
                //    //Check if some text is selected if
                //    //1)It is search word and its index is 0 start from 1
                //    //2)if it serach word and its index is not 0 start from 0
                //    //3)if selectec text is serach text start from 0
                //    //We know here that point 3 is happening or that nothing is selected
                //    //txtMainArea.Select(txtMainArea.Text.IndexOf(text), text.Length);
                //    //match.Index;
                //    count++;
                //}


                ////FirstElementSelected = true;
            }
        }

        public void ReplaceText(string textToFind,string textToReplace)
        {
            if (txtMainArea.Text != String.Empty)
            {
                int index = txtMainArea.Text.IndexOf(textToFind);
                if (index == -1) return;
                txtMainArea.Text = txtMainArea.Text.Remove(index, textToFind.Length);
                txtMainArea.Text = txtMainArea.Text.Insert(index, textToReplace);
                txtMainArea.Select(index, textToReplace.Length);
            }
        }

        public void ReplaceAll(string textToFind, string textToReplace)
        {
            if (txtMainArea.Text != String.Empty)
            {
                txtMainArea.Text = txtMainArea.Text.Replace(textToFind, textToReplace);
            }
        }

        public void FindText(string text)
        {
            if (txtMainArea.Text != String.Empty)
            {
                txtMainArea.Select(txtMainArea.Text.IndexOf(text), text.Length);
                FirstElementSelected = true;
            }
        }

        public void FindNext(string text)
        {
            //Zabezpieczys sie poprzez sprawdzenie czy zaznaczony text to ten ktory jest wpisany w polu
            //Nie zapomniec o tym by sprawdzic czy uzytkownik nie zaznaczyl juz samym jakiegos stringa takiego samoego jak
            //Wpisanego w polu!!!

            //Najpierw sprawdzamy czy coś jest zaznaczone jesli nie to wywolujemy po prostu funkcje FindText
            if(txtMainArea.Text != string.Empty && txtMainArea.SelectedText != string.Empty 
                && (txtMainArea.SelectedText.Length == text.Length || txtMainArea.SelectedText == text))
            {
                if(FirstElementSelected)
                {
                    //Wiemy ze cos jest zaznaczone i ze jest to pierwszy element
                    //SelectedElement = new SelectedElement(text,txtMainArea.Text.IndexOf(text));
                    FirstElementSelected = false;
                    SelectedText = new SelectedText(txtMainArea.Text, text, txtMainArea.Text.IndexOf(text));
                    //Usun ten pierwszy Element z obiektu zaktualizuj jego wewnetrzna klase i tym samym zaktualzuj selected text
                    SelectedText.DeleteFromTextNextIndex();
                    txtMainArea.Select(SelectedText.CurrentIndex, text.Length);
                }
                else
                {
                    //Sprawdz czy ten element nie jest Ostatnim
                    if(SelectedText.IsLastIndex())
                    {
                        FindText(text);
                        return;
                    }
                    //Wiemy ze cos jest zaznaczone i ze jest to juz kolejny Element
                    SelectedText.DeleteFromTextNextIndex();
                    txtMainArea.Select(SelectedText.CurrentIndex, text.Length);
                }
            }
            else if(txtMainArea.Text != string.Empty && txtMainArea.SelectedText == string.Empty)
            {
                FindText(text);
            }
            
            //Na poczatku bedzie 0 wiemy ze bedzie 0 nastepnie inne indexy czyli poprzedni i poprzednie indexy nas nie obchodzą.
            //Pozycja nastepna od wartosci w zmiennej CurrentSelectedStartIndex!

            //if (txtMainArea.SelectedText != string.Empty && txtMainArea.SelectedText.Contains(text))
            //{
            //    Regex rg = new Regex(@"[" + text + @"]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //    MatchCollection matches = rg.Matches(txtMainArea.Text);
            //    foreach (Match match in matches)
            //    {
            //        if (match.Index >= CurrentSelectedStartIndex)
            //        {


            //            GroupCollection groups = match.Groups;
            //            for (int i = 0; i < groups.Count; i++)
            //            {
            //                var c = groups[i].Index;
            //                //All search occurnes of given index;
            //                //groups[i].Value returns value
            //                //groups[i].Index returns index where text starts
            //                if (IndexOfSelection == -1)
            //                {
            //                    IndexOfSelection = groups[i].Index;
            //                    txtMainArea.Select(IndexOfSelection, text.Length);
            //                }
            //                else
            //                {
            //                    if (i >= IndexOfSelection)
            //                    {
            //                        IndexOfSelection = groups[i].Index;
            //                        txtMainArea.Select(IndexOfSelection, text.Length);
            //                    }
            //                }
            //            }
            //        }
            //    }

            //}
            //else
            //{
            //    IndexOfSelection = -1;
            //    FindText(text);
            //}


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

        private void MenuWordMap_Click(object sender, RoutedEventArgs e)
        {

            Console.WriteLine("Yos");

            _txtMainAreaWordMapped = !_txtMainAreaWordMapped;
            //ScrollViewer MainScroller = Window1.FindName("MainScroll") as ScrollViewer; 
            

            if (_txtMainAreaWordMapped)
            {
                WordMapIcon.Visibility  = Visibility.Visible;
                MainScroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
                txtMainArea.TextWrapping = TextWrapping.Wrap;
            }
            else
            {
                WordMapIcon.Visibility = Visibility.Hidden;
                MainScroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                txtMainArea.TextWrapping = TextWrapping.NoWrap;
            }
        }

        private void MenuItemFontStyke_Click(object sender, RoutedEventArgs e)
        {
            FontInfo fontInfo = new FontInfo();
            fontInfo.Show();
        }

        private void Window1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                listenForShortcut = true;
            }
            
            if(e.Key == Key.Tab)
            {
                int index = txtMainArea.SelectionStart;
                txtMainArea.Text = txtMainArea.Text.Insert(index, "    ");
                txtMainArea.Select(index + 4, 0);

                e.Handled = true;
            }

            if(listenForShortcut)
            {
                if (e.Key == Key.F)
                {
                    listenForShortcut = false;
                    MenuFind_Click(null,null);
                }

                if(e.Key == Key.S)
                {
                    listenForShortcut = false;
                    MenuSave_Click(null, null);
                    e.Handled = true;
                }

                if(e.Key == Key.D)
                {
                    if(txtMainArea.SelectedText == string.Empty)
                    {
                        int row = txtMainArea.GetLineIndexFromCharacterIndex(txtMainArea.CaretIndex);
                        string[] lines = txtMainArea.Text.Split(new string[] { "\r\n" },StringSplitOptions.None);

                        string copyText = "";
                        int index = 0;

                        for (int i = 0; i < lines.Length; i++)
                        {
                            copyText = copyText + lines[i] + "\r\n" ;
                            if (i == row)
                            {
                                copyText = copyText + lines[i] + "\r\n";
                                index += copyText.Length - lines[i].Length - 2;
                            }
                        }
                        copyText = copyText.Remove(copyText.LastIndexOf("\r\n"), 2);

                        txtMainArea.Text = copyText;
                        txtMainArea.Select(index, 0);
                        
                    }
                    else
                    {
                        string text = txtMainArea.SelectedText;
                        int ind = txtMainArea.SelectionStart + text.Length;
                        txtMainArea.Text = txtMainArea.Text.Insert(ind, text);
                        txtMainArea.Select(ind, text.Length);
                    }
                }
            }

        }

        private void Window1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                listenForShortcut = false;
            }

        }

        private void Window1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var c = txtMainArea.Text != string.Empty;
            bool g = this.Title.ElementAt(this.Title.Length - 1) == '*';

            if (c || g)
            {
                string message = "Czy zapisac ten plik";
                var result = MessageBox.Show(message, "Save file", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result.ToString() == "Yes")
                {
                    Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                    saveFileDialog.FileName = this.Title;
                    saveFileDialog.DefaultExt = ".txt";
                    saveFileDialog.Filter = "Text documents (.txt)|*.txt" +
                                            "|All Files|*.*"; // Filter files by extension
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
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Aplikacja stworzona przez Jakuba Bergmann na tę chwilę studenta informatykii i programiste z rocznym doświadczeniem");
        }

        //In order for it to work you must have BasicConfiguration.xml in the same place where .exe is
        private void SaveCurrentStyle_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            try
            {
                doc.Load("BasicConfiguration.xml");
            }catch(System.IO.FileNotFoundException)
            {
                //If file will not exists create it!
                doc.LoadXml("<style> \n" +
                    "  <fontFamily>Linux Libertine Display G</fontFamily> \n" +
                    "  <fontSize>12</fontSize> \n" +
                    "  <background>FFFFFFFF</background> \n" +
                    "   <foreground>FF000000</foreground> \n" +
                    "</style>");
            }
            XmlNode root = doc.FirstChild;
            if(root.HasChildNodes)
            {
                for (int i = 0; i < root.ChildNodes.Count; i++)
                {
                    var gg = root.ChildNodes[i];
                    XmlElement elementXml = root.ChildNodes[i] as XmlElement;

                    //It means that given element is \r\n 
                    if (elementXml != null)
                    {
                        BrushConverter c = new BrushConverter();

                        switch (elementXml.LocalName)
                        {
                            case "fontFamily":
                                elementXml.InnerText = txtMainArea.FontFamily.Source;
                                break;
                            case "fontSize":
                                elementXml.InnerText = txtMainArea.FontSize.ToString();
                                break;
                            case "background":
                                elementXml.InnerText = c.ConvertToString(txtMainArea.Background);
                                break;
                            case "foreground":
                                elementXml.InnerText = c.ConvertToString(txtMainArea.Foreground);
                                break;
                        }
                    }
                }
            }
            doc.Save("BasicConfiguration.xml");
            MessageBox.Show("Zmiany zostały zapisane");
        }


        private void initialStyleSettings()
        {
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            try
            {
                doc.Load("BasicConfiguration.xml");
            }
            catch (System.IO.FileNotFoundException)
            {
                //If file will not exists create it!
                doc.LoadXml("<style> \n" +
                    "  <fontFamily>Linux Libertine Display G</fontFamily> \n" +
                    "  <fontSize>12</fontSize> \n" +
                    "  <background>#FFFFFFFF</background> \n" +
                    "   <foreground>#FF000000</foreground> \n" +
                    "</style>");
            }
            XmlNode root = doc.FirstChild;
            if (root.HasChildNodes)
            {
                for (int i = 0; i < root.ChildNodes.Count; i++)
                {
                    XmlElement elementXml = root.ChildNodes[i] as XmlElement;
                    //It means that given element is \r\n 
                    if (elementXml != null)
                    {
                        BrushConverter c = new BrushConverter();
                        FontFamilyConverter converter = new FontFamilyConverter();
                        SolidColorBrush b;
                        switch (elementXml.LocalName)
                        {
                            case "fontFamily":
                                txtMainArea.FontFamily = converter.ConvertFromString(elementXml.InnerText) as FontFamily;
                                break;
                            case "fontSize":
                                txtMainArea.FontSize = Convert.ToInt32(elementXml.InnerText);
                                break;
                            case "background":
                                b = c.ConvertFromString(elementXml.InnerText) as SolidColorBrush;
                                txtMainArea.Background = b;
                                break;
                            case "foreground":
                                 b = c.ConvertFromString(elementXml.InnerText) as SolidColorBrush;
                                txtMainArea.Foreground = b;
                                break;
                        }
                    }
                }
            }
        }
    }
}
