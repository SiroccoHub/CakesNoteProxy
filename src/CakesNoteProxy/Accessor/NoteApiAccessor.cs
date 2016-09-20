using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using CakesNoteProxy.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CakesNoteProxy.Accessor
{
    internal class NoteApiAccessor : IDisposable
    {
        private readonly ILogger _logger;

        private readonly HttpClient _httpClient;

        public NoteApiAccessor(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<NoteApiAccessor>();
            _httpClient = new HttpClient();
        }

        public NoteApiAccessor(ILoggerFactory loggerFactory, HttpMessageHandler httpMessageHandler)
        {
            _logger = loggerFactory.CreateLogger<NoteApiAccessor>();
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
                _logger.LogWarning($"fire Exception at GetNotesView()");
                _logger.LogWarning($"ex=\r\n{ex}");
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
