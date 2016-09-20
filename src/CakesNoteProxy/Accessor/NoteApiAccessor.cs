using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
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

        public async Task<NoteView> GetNotesViewAsync(int page = 1, string utlname = null)
        {
            string result;
            try
            {
                var response = await _httpClient.GetAsync(
                    $"{NoteProxyConfigure.NoteApi.SiteFqdn}{NoteProxyConfigure.NoteApi.ApiRoot}/notes?note_intro_only={NoteProxyConfigure.NoteApi.IsIntro}&page={page}&urlname={utlname ?? NoteProxyConfigure.NoteApi.UserId}");

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
                _httpClient?.Dispose();
            }

            _disposed = true;
        }
    }
}
