using System;
using System.IO;
using Machine.Specifications;
using Moq;
using Pop.Cs;
using Pop.Cs.Contracts;
using It = Machine.Specifications.It;

namespace PopCs.Tests.Units
{
    public class PopOpenerTests
    {
        [Subject(typeof(PopOpener))]
        public class When_the_document_is_alrady_in_the_foreground
        {
            private static PopOpener _opener;
            private static Mock<IPeekaboo> _peekaboo;
            private static Mock<IFindProcess> _finder;
            private static Mock<IStartProcess> _starter;
            private static string _filePath;
            private static IntPtr _handle;
            private static PopProcess _popProcess;

            Establish context = () =>
            {
                _filePath = Path.Combine(Environment.CurrentDirectory, "Word.docx");
                _starter = new Mock<IStartProcess>();

                _handle = (IntPtr)100;
                _popProcess = new PopProcess(1, "WINWORD", _handle);
                _finder = new Mock<IFindProcess>();
                _finder.Setup(f => f.Find(_filePath)).Returns(_popProcess);
                _finder.Setup(f => f.FindForegroundWindow()).Returns(_handle);
                
                _peekaboo = new Mock<IPeekaboo>();

                _opener = new PopOpener(_starter.Object, _finder.Object, _peekaboo.Object);
            };

            Because of = () => _opener.Open(_filePath);

            It should_not_minimise_the_opening_application = () => _peekaboo.Verify(p => p.Minimise(_handle), Times.Never);
            It should_still_retore_the_opening_application = () => _peekaboo.Verify(p => p.Restore(_handle));
        }
    }
}