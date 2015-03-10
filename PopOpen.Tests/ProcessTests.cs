using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Machine.Specifications;

namespace PopOpen.Tests
{
    public class ProcessTests
    {
        [Subject("File open")]
        public class When_a_word_file_is_opened
        {
            private static string _filePath;
            private static Process _process;
            private static IFindProcess _processDetective;

            Establish context = () =>
            {
                _processDetective = new ProcessDetective();
                
                _filePath = Path.Combine(Environment.CurrentDirectory, "Word.docx");
                Process.Start(_filePath);
                Thread.Sleep(5000);
            };

            Because of = () => _process = _processDetective.Find(_filePath);

            It should_have_the_id_of_the_process = () => _process.Id.ShouldBeGreaterThan(0);
            It should_have_the_name_of_the_process = () => _process.ProcessName.ShouldEqual("WINWORD");

            Cleanup a = () => _process.Kill();
        }
    }
}
