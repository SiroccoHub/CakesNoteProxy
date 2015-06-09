using System;
using System.Diagnostics;
using System.Net.Http;
using CakesNoteProxy.Model;
using Newtonsoft.Json;

namespace CakesNoteProxy.Accessor
{
    internal class NoteApiAccessor : IDisposable
    {
        private readonly HttpClient _httpClient;

        public NoteApiAccessor()
        {
            _httpClient = new HttpClient();
        }

        public NoteApiAccessor(HttpMessageHandler httpMessageHandler)
        {
            _httpClient = new HttpClient(httpMessageHandler);
        }

        public NoteView GetNotesView(int page = 1, string utlname = null)
        {
            string result;
            try
            {
                var response = _httpClient.GetAsync(
                    string.Format("{0}{1}/notes?note_intro_only={2}&page={3}&urlname={4}",
                        NoteProxyConfigure.NoteApi.SiteFqdn,
                        NoteProxyConfigure.NoteApi.ApiRoot,
                        NoteProxyConfigure.NoteApi.IsIntro,
                        page,
                        utlname ?? NoteProxyConfigure.NoteApi.UserId)).Result;

                response.EnsureSuccessStatusCode();
                result = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Trace.TraceWarning("fire Exception at GetNotesView()");
                Trace.TraceWarning("ex=\r\n{0}", ex);
                result = null;
            }
            return (result != null) ? JsonConvert.DeserializeObject<NoteView>(result) : null;
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
                if (_httpClient!=null) _httpClient.Dispose();
            }

            _disposed = true;
        }
    }
}
