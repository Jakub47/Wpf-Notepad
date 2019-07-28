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
    /// Interaction logic for FindAndReplace.xaml
    /// </summary>
    public partial class FindAndReplace : Window
    {
        public int Counter { get; set; }

        public FindAndReplace()
        {
            InitializeComponent();
            Counter = -1;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            
        }

        //Find
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Counter = 0;
            string text = textToFind.Text;
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).Activate();
                    (window as MainWindow).FindText(text);
                }
            }
        }

        //Find next
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string text = textToFind.Text;
            Counter++;
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).Activate();
                    (window as MainWindow).FindNextText(text);
                }
            }
        }

        private void Replace_Click(object sender, RoutedEventArgs e)
        {
            Counter = -1;
        }

        private void ReplaceAll_Click(object sender, RoutedEventArgs e)
        {
            Counter = -1;
        }

        private void TextToFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            Counter = -1;
        }


    }
}
