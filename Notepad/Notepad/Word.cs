using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad
{
    public class Word
    {
        public string content { get; set; }
        public int length { get; }


        public Word()
        {
            content = String.Empty; 
        }

        public Word(string content)
        {
            this.content = content;
            length = content.Length;
        }

        public bool FindElement(string element)
        {
            return content.Contains(element);
        }
    }
}
