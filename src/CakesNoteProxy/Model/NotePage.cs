using System.Collections.Generic;

namespace CakesNoteProxy.Model
{
    public class NotePage
    {
        public bool first_page { get; set; }
        public int? next_page { get; set; }
        public bool last_page { get; set; }
        public List<NoteContent> notes { get; set; }
        public List<string> announce { get; set; }
    }
}