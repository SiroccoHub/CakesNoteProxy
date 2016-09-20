using System;
using System.Net;
using Microsoft.Extensions.Logging;

namespace CakesNoteProxy
{
    public static class NoteProxyConfigure
    {
        public static ILoggerFactory LoggerFactory;

        public static class NoteApi
        {
            public static string SiteFqdn { get; private set; }

            public static string UserId { get; private set; }

            public static bool IsIntro { get; private set; }

            static NoteApi()
            {
                SiteFqdn = "https://note.mu";
                UserId = "info";
                IsIntro = true;
            }

            public static void SetGlobal(string noteSiteFqdn)
            {
                if ( noteSiteFqdn!=null)
                    SiteFqdn = noteSiteFqdn;
            }

            public static void SetMyNote(string noteUserId, bool isIntro)
            {
                if (noteUserId != null)
                    UserId = noteUserId;
                IsIntro = isIntro;
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
