using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Machine.Specifications;
using Pop.Cs;
using Pop.Cs.Contracts;

namespace PopCs.Tests.Integrations
{
    public class ProcessTests
    {
        [Subject(typeof(ProcessFinder))]
        public class When_a_word_file_is_opened
        {
            private static string _filePath;
            private static PopProcess _process;
            private static IFindProcess _processDetective;

            Establish context = () =>
            {
                _processDetective = new ProcessFinder();
                
                _filePath = Path.Combine(Environment.CurrentDirectory, "Word.docx");
                Process.Start(_filePath);
                Thread.Sleep(5000);
            };

            Because of = () => _process = _processDetective.Find(_filePath);

            It should_have_the_id_of_the_process = () => _process.Id.ShouldBeGreaterThan(0);
            It should_have_the_name_of_the_process = () => _process.ProcessName.ShouldEqual("WINWORD");
        }
    }
}
