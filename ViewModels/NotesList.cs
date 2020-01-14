using System;
using System.Collections.Generic;
using notepad_react.Models;

namespace notepad_react.ViewModels
{
    public class NotesList
    {
        public List<NoteWithCategories> Notes { set; get; }
        public int CurrentPage {set; get;}
        public int AllPages {set; get;}
    }
}