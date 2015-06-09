using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CakesNoteProxy.UnitTest
{
    [TestClass]
    public class ForDebugRun
    {
        [TestMethod]
        public void NoteStore_GetCurrentTimeline()
        {
            var noteContent = NoteStore.GetNotesCurrentTimeline().First();

            Debug.WriteLine(noteContent);
        }

        [TestMethod]
        public void NoteStore_GetCachedNotes()
        {
            var sw = new Stopwatch();
            sw.Restart();

            var noteContent = NoteStore.GetNotesTimeline(5);

            Debug.WriteLine("Elapsed: {0} ms", sw.ElapsedMilliseconds);
            Debug.WriteLine("noteContent.Count()={0}", noteContent.Count());
            foreach (var content in noteContent)
            {
                Debug.WriteLine(content);
            }
            sw.Restart();

            noteContent = NoteStore.GetNotesTimeline(5);
            
            Debug.WriteLine("Elapsed: {0} ms", sw.ElapsedMilliseconds);
            Debug.WriteLine("noteContent.Count()={0}", noteContent.Count());
            foreach (var content in noteContent)
            {
                Debug.WriteLine(content);
            }

            sw.Restart();

            noteContent = NoteStore.GetNotesTimeline(5);

            Debug.WriteLine("Elapsed: {0} ms", sw.ElapsedMilliseconds);
            Debug.WriteLine("noteContent.Count()={0}", noteContent.Count());
            foreach (var content in noteContent)
            {
                Debug.WriteLine(content);
            }
            sw.Restart();

            noteContent = NoteStore.GetNotesTimeline(10);

            Debug.WriteLine("Elapsed: {0} ms", sw.ElapsedMilliseconds);
            Debug.WriteLine("noteContent.Count()={0}", noteContent.Count());
            foreach (var content in noteContent)
            {
                Debug.WriteLine(content);
            }
            Debug.WriteLine("wait...");
            Thread.Sleep(10000);
            sw.Restart();

            noteContent = NoteStore.GetNotesCurrentTimeline(20);
            Debug.WriteLine("Elapsed: {0} ms", sw.ElapsedMilliseconds);

            Debug.WriteLine("noteContent.Count()={0}", noteContent.Count());
            foreach (var content in noteContent)
            {
                Debug.WriteLine(content);
            }

            Debug.WriteLine("wait...");
            Thread.Sleep(10000);

            //Debug.WriteLine("Elapsed: {0} ms", sw.ElapsedMilliseconds);
            //sw.Restart();

            //Debug.WriteLine("mode change. Always Cache Expired.");
            //var cacheInstance = NoteStore.CachedInstance;
            //cacheInstance.RefreshCachePredicate = time => true;

            //Debug.WriteLine("Elapsed: {0} ms", sw.ElapsedMilliseconds);
            //sw.Restart();

            //Debug.WriteLine("wait...");
            //Thread.Sleep(5000);

            sw.Restart();

            noteContent = NoteStore.GetNotesTimeline(25);

            Debug.WriteLine("Elapsed: {0} ms", sw.ElapsedMilliseconds);
            Debug.WriteLine("noteContent.Count()={0}", noteContent.Count());
            foreach (var content in noteContent)
            {
                Debug.WriteLine(content);
            }
            Debug.WriteLine("wait...");
            Thread.Sleep(10000);

            sw.Restart();

            noteContent = NoteStore.GetNotesTimeline(10);

            Debug.WriteLine("Elapsed: {0} ms", sw.ElapsedMilliseconds);
            Debug.WriteLine("noteContent.Count()={0}", noteContent.Count());
            foreach (var content in noteContent)
            {
                Debug.WriteLine(content);
            }
        }
    }
}
