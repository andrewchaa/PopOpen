using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Machine.Specifications;
using Pop.Contracts;

namespace Pop.Tests.Integrations
{
    public class PopOpenerTests
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        public class Context
        {
            protected static Process _process;
            protected static PopOpener _popOpener;
            protected static IntPtr _activeWindowHandle;
            protected static PopProcess _openingProcess;
            protected static string _filePath;
            protected static IFindProcess _finder;
        }

        [Subject(typeof(PopOpener))]
        public class When_it_opens_a_word_document : Context
        {
            Establish context = () =>
            {
                _filePath = Path.Combine(Environment.CurrentDirectory, "Word.docx");
                _popOpener = new PopOpener(new ProcessStarter(), new ProcessFinder(), new Peekaboo());
            };

            Because of = () =>
            {
                _openingProcess = _popOpener.Open(_filePath);
                _activeWindowHandle = GetForegroundWindow();
                Console.WriteLine("Active window handle: {0}", _activeWindowHandle);
            };

            It should_find_the_currently_foreground_window_to_compare = () => _activeWindowHandle.ShouldNotEqual(IntPtr.Zero);
            It should_have_the_opening_process_in_the_foreground = () => _activeWindowHandle.ShouldEqual(_openingProcess.MainWindowHandle);
        }
    }

}