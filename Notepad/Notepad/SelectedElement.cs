using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad
{
    public class SelectedElement
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
