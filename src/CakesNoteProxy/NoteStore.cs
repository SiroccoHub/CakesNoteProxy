using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CakesNoteProxy.Accessor;
using CakesNoteProxy.Model;

namespace CakesNoteProxy
{
    public class NoteStore
    {
        private static readonly object Sync = new object();

        private volatile static NoteCache _cachedInstance;

        static NoteStore()
        {
            var cachedInstance = NoteStore.CachedInstance;
        }

        public NoteStore()
        {
            var cachedInstance = NoteStore.CachedInstance;
        }

        public static NoteCache CachedInstance
        {
            get
            {
                if (_cachedInstance != null) return _cachedInstance;

                lock (Sync)
                {
                    // instanced
                    if (_cachedInstance == null)
                        _cachedInstance = new NoteCache();

                    // deligation refleshing cache
                    _cachedInstance.RefreshCacheExecute = async (contentCount) =>
                    {
                        var results = await CallApiAndAddAsync(contentCount);
                        return results.OrderByDescending(p => p.publish_at).ToList();
                    };
                }
                return _cachedInstance;
            }
        }

        /// <summary>
        /// Get Timeline from cache and online
        /// </summary>
        /// <param name="willGetNotesCount"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<NoteContent>> GetNotesTimeline(int willGetNotesCount = 10)
        {
            if (_cachedInstance.GetAll().Count < willGetNotesCount)
                await GetNotesCurrentTimeline(willGetNotesCount);

            return _cachedInstance.GetAll().OrderByDescending(p => p.publish_at).Take(willGetNotesCount).ToList();
        }

        /// <summary>
        /// Get Current Timeline from online
        /// </summary>
        /// <param name="willGetNotesCount"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<NoteContent>> GetNotesCurrentTimeline(int willGetNotesCount = 10)
        {
            _cachedInstance.AddRange(await CallApiAndAddAsync(willGetNotesCount));
            return _cachedInstance.GetAll().OrderByDescending(p => p.publish_at).ToList();
        }

        /// <summary>
        /// Get and Put Data from Calling Api
        /// </summary>
        /// <param name="willGetNotesCount"></param>
        private static async Task<List<NoteContent>> CallApiAndAddAsync(int willGetNotesCount)
        {
            var results = new List<NoteContent>();
            using (var accessor = new NoteApiAccessor())
            {
                var gotNotesCount = 0;
                var currentPage = 1;

                while (gotNotesCount < willGetNotesCount)
                {
                    var view = await accessor.GetNotesViewAsync(currentPage++);
                    if (view == null) break;

                    var data = view.data;
                    gotNotesCount += data.notes.Count;
                    results.AddRange(data.notes);

                    Trace.TraceInformation("calling CallApiAndAdd,{0},{1},{2},{3},{4}", willGetNotesCount, gotNotesCount, currentPage - 1, data.next_page.HasValue, data.last_page);
                    
                    if (data.next_page.HasValue == false || data.last_page) break;
                }
            }
            return results;
        }
    }
}
