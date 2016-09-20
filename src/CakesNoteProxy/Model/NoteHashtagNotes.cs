using System;

namespace CakesNoteProxy.Model
{
    public class NoteHashtagNotes
    {
        public long id { get; set; }
        public decimal trend_score { get; set; }
        public DateTime created_at { get; set; }
        public NoteHashtag hashtag { get; set; }
    }
}