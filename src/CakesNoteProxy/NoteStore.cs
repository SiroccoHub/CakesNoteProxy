using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CakesNoteProxy.Accessor;
using CakesNoteProxy.Model;
using Microsoft.Extensions.Logging;

namespace CakesNoteProxy
{
    public class NoteStore : IDisposable
    {
        private readonly ILogger _logger;
        private static ILoggerFactory _loggerFactory;

        private static readonly object Sync = new object();

        private static volatile NoteCache _cachedInstance;

        private const int DefaultContentCount = 10;

        public NoteStore()
        {
            _loggerFactory = NoteProxyConfigure.LoggerFactory;

            if (NoteProxyConfigure.LoggerFactory == null)
                throw new ArgumentNullException(nameof(NoteProxyConfigure.LoggerFactory));

            _logger = NoteProxyConfigure.LoggerFactory.CreateLogger<NoteStore>();

            var cachedInstance = CachedInstance;
        }

        public NoteStore(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory ?? NoteProxyConfigure.LoggerFactory;

            if (_loggerFactory == null)
                throw new ArgumentNullException(nameof(loggerFactory));

            _logger = loggerFactory.CreateLogger<NoteStore>();
            var cachedInstance = CachedInstance;
        }

        public NoteCache CachedInstance
        {
            get
            {
                if (_cachedInstance != null) return _cachedInstance;

                lock (Sync)
                {
                    // instanced
                    if (_cachedInstance == null)
                        _cachedInstance = new NoteCache(_loggerFactory);

                    // deligation refleshing cache
                    _cachedInstance.RefreshCacheExecute = async (currentContentCount) =>
                    {
                        var refreshContentCount = currentContentCount == 0
                            ? DefaultContentCount
                            : currentContentCount;

                        var results = await CallApiAndAddAsync(refreshContentCount);
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
        public async Task<IEnumerable<NoteContent>> GetNotesTimelineAsync(int willGetNotesCount = DefaultContentCount)
        {
            return (await CachedInstance.GetAllAsync()).OrderByDescending(p => p.publish_at).Take(willGetNotesCount);
        }

        /// <summary>
        /// Get Current Timeline from online
        /// </summary>
        /// <param name="willGetNotesCount"></param>
        /// <returns></returns>
        public async Task<IEnumerable<NoteContent>> GetNotesCurrentTimelineAsync(int willGetNotesCount = DefaultContentCount)
        {
            CachedInstance.AddRange(await CallApiAndAddAsync(willGetNotesCount));
            return (await CachedInstance.GetAllAsync()).OrderByDescending(p => p.publish_at);
        }

        /// <summary>
        /// Get and Put Data from Calling Api
        /// </summary>
        /// <param name="willGetNotesCount"></param>
        private async Task<List<NoteContent>> CallApiAndAddAsync(int willGetNotesCount)
        {
            var results = new List<NoteContent>();
            using (var accessor = new NoteApiAccessor(_loggerFactory))
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

                    _logger.LogInformation($"calling CallApiAndAdd,willGetNotesCount={willGetNotesCount},gotNotesCount={gotNotesCount},currentPage={currentPage},data.next_page.HasValue={data.next_page.HasValue},{data.next_page.HasValue},data.last_page={data.last_page}");

                    if (data.next_page.HasValue == false || data.last_page) break;
                }
            }
            return results;
        }

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージ状態を破棄します (マネージ オブジェクト)。
                }

                // TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。

                disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージ リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~NoteStore() {
        //   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
        //   Dispose(false);
        // }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        void IDisposable.Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
