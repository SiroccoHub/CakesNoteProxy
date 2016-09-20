using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CakesNoteProxy.Model;

namespace CakesNoteProxy
{
    public class NoteCache : IDisposable
    {
        private readonly List<NoteContent> _contents;
        public DateTime ModifiedDateTimeUtc { get; private set; }

        public TimeSpan CacheLifecycleTimeSpan;
        public Predicate<DateTime> RefreshCachePredicate;

        public double CacheLifecycleSpeculativeExecutionRate;
        public Predicate<DateTime> RefreshCacheSpeculativeExecutionPredicate;
        private volatile bool _executeMonitoringThread = true;

        public Func<int, Task<List<NoteContent>>> RefreshCacheExecute;

        public TimeSpan MonitoringThreadInterval;

        public int CachedItemsCountHardLimit;

        public NoteCache()
        {
            _contents = new List<NoteContent>();

            CacheLifecycleTimeSpan = NoteProxyConfigure.NoteCache.CacheLifecycleTimeSpan;
            CacheLifecycleSpeculativeExecutionRate = NoteProxyConfigure.NoteCache.CacheLifecycleSpeculativeExecutionRate;

            MonitoringThreadInterval = NoteProxyConfigure.NoteCache.MonitoringThreadInterval;
            CachedItemsCountHardLimit = NoteProxyConfigure.NoteCache.CachedItemsCountHardLimit;

            RefreshCachePredicate = currentDateTime =>
            {
                var targetDt = ModifiedDateTimeUtc.Add(CacheLifecycleTimeSpan);

                Trace.TraceInformation("calling delegate RefreshCachePredicate(),{0},{1}", targetDt, currentDateTime);
                return (targetDt < currentDateTime);
            };

            RefreshCacheSpeculativeExecutionPredicate = currentDateTime =>
            {
                var targetDt = ModifiedDateTimeUtc.Add(
                    new TimeSpan((long)(CacheLifecycleTimeSpan.Ticks * CacheLifecycleSpeculativeExecutionRate)));

                Trace.TraceInformation("calling delegate RefreshCacheSpeculativeExecutionPredicate(),{0},{1}", targetDt, currentDateTime);
                return (targetDt < currentDateTime);
            };

            ModifiedDateTimeUtc = DateTime.UtcNow;

            // monitoring thread.
            Task.Factory.StartNew(() =>
            {
                while (_executeMonitoringThread)
                {
                    Trace.TraceInformation("calling ()monitoring. _executeMonitoringThread={0},interval={1}", _executeMonitoringThread, MonitoringThreadInterval);
                    if (RefreshCacheSpeculativeExecutionPredicate(DateTime.UtcNow))  RefreshCache(true);
                    Thread.Sleep(MonitoringThreadInterval);
                }
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Add to Cache
        /// </summary>
        /// <param name="addList"></param>
        public void AddRange(ICollection<NoteContent> addList)
        {
            Trace.TraceInformation("calling AddRange()");

            using (new RwLockScope(RwLockScopes.Upgradeable))
            {
                AddRangeInternal(addList);
            }
        }

        /// <summary>
        /// Add to Cache (internal use)
        /// </summary>
        /// <param name="addList"></param>
        private void AddRangeInternal(ICollection<NoteContent> addList)
        {
            Trace.TraceInformation("calling AddRangeInternal()");

            using (new RwLockScope(RwLockScopes.Write))
            {
                _contents.RemoveAll(addList.Contains);
                _contents.AddRange(addList);

                if (_contents.Count > CachedItemsCountHardLimit)
                {
                    Trace.TraceWarning("fire CachedItemsCountHardLimit,{0},{1},will delete", _contents.Count,CachedItemsCountHardLimit);
                    _contents.RemoveRange(CachedItemsCountHardLimit, _contents.Count - CachedItemsCountHardLimit);
                }

                ModifiedDateTimeUtc = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Refresh cache called from internal
        /// </summary>
        private async void RefreshCache(bool useNewThread = false)
        {
            Trace.TraceInformation("calling RefreshCache({0})", useNewThread);

            if (RefreshCacheExecute == null) return;

            if (!useNewThread)
            {
                AddRange(await RefreshCacheExecute(_contents.Count));
                Trace.TraceInformation("called RefreshCache(False)");
            }
            else
            {
                await Task.Factory.StartNew(async () =>
                 {
                     try
                     {
                         AddRange(await RefreshCacheExecute(_contents.Count));
                     }
                     catch (Exception ex)
                     {
                         Trace.TraceWarning("fire Exception at RefreshCache() another threads.");
                         Trace.TraceWarning("ex=\r\n{0}", ex);
                     }
                     finally
                     {
                         Trace.TraceInformation("called RefreshCache(True)");
                     }
                 }).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Get all Contents
        /// </summary>
        /// <returns></returns>
        public IList<NoteContent> GetAll()
        {
            Trace.TraceInformation("calling GetAll()");

            // if cache has expired, calling Refresh.
            if (RefreshCachePredicate(DateTime.UtcNow)) RefreshCache();

            using (new RwLockScope())
            {
                return _contents.ToList();
            }
        }




        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _executeMonitoringThread = false;
            }

            _disposed = true;
        }
    }
}
