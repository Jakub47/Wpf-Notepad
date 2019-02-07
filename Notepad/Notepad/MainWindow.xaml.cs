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
    }
}
