using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Machine.Specifications;
using Pop.Cs;
using Pop.Cs.Contracts;

namespace PopCs.Tests.Integrations
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
            protected static string _file1Path;
            protected static IFindProcess _finder;
        }

        [Subject(typeof(PopOpener))]
        public class When_it_opens_a_word_document : Context
        {
            Establish context = () =>
            {
                _file1Path = Path.Combine(Environment.CurrentDirectory, "Word.docx");
                _popOpener = new PopOpener(new ProcessStarter(), new ProcessFinder(), new Peekaboo());
            };

            Because of = () =>
            {
                _openingProcess = _popOpener.Open(_file1Path);
                _activeWindowHandle = GetForegroundWindow();
                Console.WriteLine("Active window handle: {0}", _activeWindowHandle);
            };

            It should_find_the_currently_foreground_window_to_compare = () => _activeWindowHandle.ShouldNotEqual(IntPtr.Zero);
            It should_have_the_opening_process_in_the_foreground = () => _activeWindowHandle.ShouldEqual(_openingProcess.MainWindowHandle);
        }

        [Subject(typeof(PopOpener))]
        public class When_it_opens_an_excel_document : Context
        {
            private static string _file2Path;

            Establish context = () =>
            {
                _file1Path = Path.Combine(Environment.CurrentDirectory, "Excel 1.xlsx");
//                _file2Path = Path.Combine(Environment.CurrentDirectory, "Excel 2.xlsx");
                _popOpener = new PopOpener(new ProcessStarter(), new ProcessFinder(), new Peekaboo());
            };

            Because of = () =>
            {
                _openingProcess = _popOpener.Open(_file1Path);
//                _openingProcess = _popOpener.Open(_file2Path);
                _activeWindowHandle = GetForegroundWindow();
            };

            It should_find_the_currently_foreground_window_to_compare = () => _activeWindowHandle.ShouldNotEqual(IntPtr.Zero);
            It should_have_the_opening_process_in_the_foreground = () => _activeWindowHandle.ShouldEqual(_openingProcess.MainWindowHandle);
        }
    }

}