using System;

namespace CakesNoteProxy
{
    public static class NoteProxyConfigure
    {
        public static class NoteApi
        {
            public static string SiteFqdn { get; private set; }

            public static string DefaultUserId { get; private set; }
            static NoteApi()
            {
                SiteFqdn = "https://note.mu";
                DefaultUserId = "info";
            }

            public static void SetGlobal(string noteSiteFqdn)
            {
                if ( noteSiteFqdn!=null)
                    SiteFqdn = noteSiteFqdn;
            }

            public static void SetMyNote(string noteDefautUserId)
            {
                if (noteDefautUserId != null)
                    DefaultUserId = noteDefautUserId;
            }

            public static readonly string ApiRoot = "/api/v1";
        }

        public static class NoteCache
        {
            public static int CachedItemsCountHardLimit = 50;

            public static TimeSpan MonitoringThreadInterval = TimeSpan.FromSeconds(12);

            public static TimeSpan CacheLifecycleTimeSpan = TimeSpan.FromSeconds(720);

            public static double CacheLifecycleSpeculativeExecutionRate = 0.8;
        }
    }
}
