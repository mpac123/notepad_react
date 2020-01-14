using System;
using System.Collections.Generic;

namespace notepad_react.ViewModels
{
    public class NoteWithCategories
    {
        public int NoteID {set; get;}
        public DateTime NoteDate {set; get;}

        public string Title {set; get;}

        public string Description {set; get;}
        public bool IsMarkdown {set; get;}

        public byte[] Timestamp {set; get;}

        public ICollection<string> Categories {set; get;}
    }
}