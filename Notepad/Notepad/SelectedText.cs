using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad
{
    public class SelectedText
    {
        public string SelectedTextInTextField { get; set; }
        public int CurrentIndex { get; private set; }

        private SelectedElement selected;
        private int _deleteLostLength;

        public SelectedText(string selectedText)
        {
            SelectedTextInTextField = selectedText;
        }

        public SelectedText(string selectedText, string textInFindTextField, int indexOfText)
        {
            SelectedTextInTextField = selectedText;

            selected = new SelectedElement(textInFindTextField, indexOfText);
        }

        public void DeleteFromTextNextIndex()
        {
            SelectedTextInTextField = SelectedTextInTextField.Remove(selected.IndexOfText, selected.Text.Length);
            selected.IndexOfText = SelectedTextInTextField.IndexOf(selected.Text);
            CurrentIndex = selected.IndexOfText + _deleteLostLength + selected.Text.Length;
            _deleteLostLength += selected.Text.Length;
        }

        public bool IsLastIndex()
        {
            return SelectedTextInTextField.IndexOf(selected.Text) == -1 ? true : false;
        }

        private class SelectedElement
        {
            public string Text { get; set; }
            public int IndexOfText { get; set; }

            public SelectedElement()
            {

            }

            public SelectedElement(string text, int indexOfText)
            {
                this.Text = text;
                this.IndexOfText = indexOfText;
            }
        }
    }
}
