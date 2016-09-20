using System;

namespace CakesNoteProxy.Model
{
    public class NoteUser
    {
        public long id { get; set; }
        public long g_id { get; set; }
        public string urlname { get; set; }
        public string nickname { get; set; }
        public long note_count { get; set; }
        public long following_count { get; set; }
        public long follower_count { get; set; }
        public string user_profile_image_path { get; set; }
        public DateTime created_at { get; set; }
        public bool is_following { get; set; }
        public bool is_me { get; set; }
    }
}
