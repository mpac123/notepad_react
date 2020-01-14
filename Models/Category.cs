using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace notepad_react.Models
{
    public class Category
    {
        public int CategoryID {set; get;}

        [Column(TypeName = "NVarChar(64)")]
        public string Title {set; get;}

        public ICollection<NoteCategory> NoteCategories {set; get;}
    }
}