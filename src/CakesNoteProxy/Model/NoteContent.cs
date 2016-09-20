using System;
using System.Collections.Generic;

namespace CakesNoteProxy.Model
{
    public class NoteContent : IEquatable<NoteContent>
    {
        public int id { get; set; }
        public string key { get; set; }
        public string body { get; set; }
        public string external_url { get; set; }
        public string type { get; set; }
        public int user_id { get; set; }
        public int serialization_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string tweet_text { get; set; }
        public List<NotePicture> pictures { get; set; }
        public List<object> embedded_contents { get; set; }
        public object audio { get; set; }
        public string status { get; set; }
        public DateTime publish_at { get; set; }
        public int price { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int like_count { get; set; }
        public int comment_count { get; set; }
        public bool is_limited { get; set; }
        public bool is_my_note { get; set; }
        public bool can_read { get; set; }
        public bool? is_liked { get; set; }
        public bool is_purchased { get; set; }
        public bool is_magazine_purchased { get; set; }
        public bool is_available { get; set; }
        public string eyecatch { get; set; }
        public int eyecatch_width { get; set; }
        public int eyecatch_height { get; set; }
        public bool has_draft { get; set; }
        public bool? is_draft { get; set; }
        public bool is_reserved { get; set; }
        public DateTime? reserved_publish_at { get; set; }
        public List<NoteLike> likes { get; set; }
        public NoteUser user { get; set; }
        public List<object> comments { get; set; }
        public List<NoteHashtagNotes> hashtag_notes { get; set; }
        public bool has_coupon { get; set; }


        public override string ToString()
        {
            return string.Format("NoteContent,id:{0},key:{1}", id, key);
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var objAsNoteContent = obj as NoteContent;
            return objAsNoteContent != null && Equals(objAsNoteContent);
        }
        public override int GetHashCode()
        {
            return id;
        }
        public bool Equals(NoteContent other)
        {
            return other != null && (id.Equals(other.id));
        }
    }
}