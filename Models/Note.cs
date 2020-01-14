using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace notepad_react.Models
{
    public class Note
    {
        public int NoteID {set; get;}
        public DateTime NoteDate {set; get;}

        [Column(TypeName="NVarChar(64)")]
        public string Title {set; get;}

        [Column(TypeName="NVarChar(MAX)")]
        public string Description {set; get;}

        [Timestamp]
        public byte[] Timestamp {set; get;}

        public ICollection<NoteCategory> NoteCategories {set; get;}
    }
}