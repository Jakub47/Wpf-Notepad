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

namespace Notepad.Views
{
    /// <summary>
    /// Interaction logic for FontInfo.xaml
    /// </summary>
    public partial class FontInfo : Window
    {
        private string fontStyle;
        private int fontSize;
        
        public FontInfo()
        {
            InitializeComponent();
            InitializeFontFamillyList();
            InitializeFontSizeList();
        }

        private void InitializeFontSizeList()
        {
            List<FontFamily> fontFamiliy = Fonts.SystemFontFamilies.ToList();
            foreach (FontFamily currentFontFamily in fontFamiliy)
            {
                ComboBoxItem item = new ComboBoxItem() { Content = currentFontFamily.Source.ToString() };
                ComboBoxFonts.Items.Add(item);
            }
        }

        private void InitializeFontFamillyList()
        {
            for(int i = 0;i<=72;i++)
            {
                if(i == 12)
                    ComboBoxFontSize.Items.Add(new ListBoxItem() { Content = (i + 1).ToString(),IsSelected=true});
                else
                    ComboBoxFontSize.Items.Add(new ListBoxItem() { Content = (i + 1).ToString() });
            }
        }

        private void ComboBoxFonts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FontFamilyConverter converter = new FontFamilyConverter();
            ComboBoxItem selectedItem = e.AddedItems[0] as ComboBoxItem;
            string fontSelected = selectedItem.Content.ToString();
            fontStyle = fontSelected;
            TextBoxPresentText.FontFamily = converter.ConvertFromString(fontSelected) as FontFamily;
        }

        private void ComboBoxFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem selectedItem = e.AddedItems[0] as ListBoxItem;
            int fontSizeInComboBox = Convert.ToInt32(selectedItem.Content.ToString());
            TextBoxPresentText.FontSize = fontSizeInComboBox;

            fontSize = fontSizeInComboBox;
        }

        private void ButtonAcceptChanges_Click(object sender, RoutedEventArgs e)
        {

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                   // (window as MainWindow).Activate();
                    (window as MainWindow).ChangeFontStyle(fontStyle, fontSize);
                    (window as MainWindow).Show();
                    Hide();
                }
            }
        }

        private void ButtonCancelChanges_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).Show();
                    Hide();
                }
            }
        }
    }
}
