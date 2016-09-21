using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CakesNoteProxy;
using Xunit;

namespace CakesNoteProxy.Test
{
    public class ForDebugRun
    {
        [Fact]
        public async Task GetNotesCurrentTimelineAsync()
        {
            using (var x = new NoteStore(ApplicationLogging.LoggerFactory))
            {
                var noteContent = (await x.GetNotesCurrentTimelineAsync()).ToList();
                Assert.True(noteContent.Any());
                Debug.WriteLine(noteContent.First());
            }
        }

        [Fact]
        public async Task GetCachedNotes()
        {
            var sw = new Stopwatch();

            using (var x = new NoteStore(ApplicationLogging.LoggerFactory))
            {
                sw.Restart();

                var noteContent = (await x.GetNotesTimelineAsync(5)).ToList();
                Assert.True(noteContent.Any());

                Debug.WriteLine("Elapsed: {0} ms", sw.ElapsedMilliseconds);
                Debug.WriteLine("noteContent.Count()={0}", noteContent.Count());
                foreach (var content in noteContent)
                {
                    Debug.WriteLine(content);
                }
                sw.Restart();

                noteContent = (await x.GetNotesTimelineAsync(5)).ToList();
                Assert.True(noteContent.Any());
                Assert.True(sw.ElapsedMilliseconds < 10);

                Debug.WriteLine("Elapsed: {0} ms", sw.ElapsedMilliseconds);
                Debug.WriteLine("noteContent.Count()={0}", noteContent.Count());
                foreach (var content in noteContent)
                {
                    Debug.WriteLine(content);
                }

                sw.Restart();

                noteContent = (await x.GetNotesTimelineAsync(5)).ToList();
                Assert.True(noteContent.Any());
                Assert.True(sw.ElapsedMilliseconds < 10);

                Debug.WriteLine("Elapsed: {0} ms", sw.ElapsedMilliseconds);
                Debug.WriteLine("noteContent.Count()={0}", noteContent.Count());
                foreach (var content in noteContent)
                {
                    Debug.WriteLine(content);
                }
                sw.Restart();

                noteContent = (await x.GetNotesTimelineAsync(10)).ToList();
                Assert.True(noteContent.Any());

                Debug.WriteLine("Elapsed: {0} ms", sw.ElapsedMilliseconds);
                Debug.WriteLine("noteContent.Count()={0}", noteContent.Count());
                foreach (var content in noteContent)
                {
                    Debug.WriteLine(content);
                }

                sw.Restart();

                noteContent = (await x.GetNotesTimelineAsync(10)).ToList();
                Assert.True(noteContent.Any());
                Assert.True(sw.ElapsedMilliseconds < 10);

                Debug.WriteLine("Elapsed: {0} ms", sw.ElapsedMilliseconds);
                Debug.WriteLine("noteContent.Count()={0}", noteContent.Count());
                foreach (var content in noteContent)
                {
                    Debug.WriteLine(content);
                }

                sw.Restart();

                noteContent = (await x.GetNotesTimelineAsync(10)).ToList();
                Assert.True(noteContent.Any());
                Assert.True(sw.ElapsedMilliseconds < 10);

                Debug.WriteLine("Elapsed: {0} ms", sw.ElapsedMilliseconds);
                Debug.WriteLine("noteContent.Count()={0}", noteContent.Count());
                foreach (var content in noteContent)
                {
                    Debug.WriteLine(content);
                }
            }

            using (var x = new NoteStore(ApplicationLogging.LoggerFactory))
            {
                sw.Restart();

                var noteContent = (await x.GetNotesTimelineAsync(5)).ToList();
                Assert.True(noteContent.Any());
                Assert.True(sw.ElapsedMilliseconds < 10);

                Debug.WriteLine("Elapsed: {0} ms", sw.ElapsedMilliseconds);
                Debug.WriteLine("noteContent.Count()={0}", noteContent.Count());
                foreach (var content in noteContent)
                {
                    Debug.WriteLine(content);
                }
                sw.Restart();

                noteContent = (await x.GetNotesTimelineAsync(5)).ToList();
                Assert.True(noteContent.Any());
                Assert.True(sw.ElapsedMilliseconds < 10);

                Debug.WriteLine("Elapsed: {0} ms", sw.ElapsedMilliseconds);
                Debug.WriteLine("noteContent.Count()={0}", noteContent.Count());
                foreach (var content in noteContent)
                {
                    Debug.WriteLine(content);
                }
            }
        }
    }
}
