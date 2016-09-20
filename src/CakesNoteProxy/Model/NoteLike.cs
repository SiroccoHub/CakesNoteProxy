using System;

namespace CakesNoteProxy.Model
{
    public class NoteLike
    {
        public long id { get; set; }
        public bool user_likes { get; set; }
        public DateTime created_at { get; set; }
        public NoteUser user { get; set; }
    }
}