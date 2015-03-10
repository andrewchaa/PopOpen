using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Machine.Specifications;

namespace PopOpen.Tests
{
    public class PopOpenerTests
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [Subject(typeof(PopOpener))]
        public class When_finds_the_opening_process
        {
            private static string _filePath;
            private static Process _process;
            private static IFindProcess _finder;
            private static PopOpener _popOpener;
            private static IntPtr _activeWindowHandle;
            private static Process _openingProcess;

            Establish context = () =>
            {
                _filePath = Path.Combine(Environment.CurrentDirectory, "Word.docx");
                _popOpener = new PopOpener(new ProcessStarter(), new ProcessFinder());
            };

            Because of = () =>
            {
                _openingProcess = _popOpener.Open(_filePath);
                _activeWindowHandle = GetForegroundWindow();
                Console.WriteLine("Active window handle: {0}", _activeWindowHandle);
            };

            It should_have_foreground_window = () => _activeWindowHandle.ShouldNotEqual(IntPtr.Zero);
            It should_have_the_opening_process_in_the_foreground = () => _activeWindowHandle.ShouldEqual(_openingProcess.MainWindowHandle);
        } 
    }
}