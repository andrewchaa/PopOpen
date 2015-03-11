using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pop.Cs;

namespace Pop.Sample
{
    public class Program
    {
        static void Main(string[] args)
        {
            var opener = new PopOpener(new ProcessStarter(), new ProcessFinder(), new Peekaboo());
            opener.Open(Path.Combine(Environment.CurrentDirectory, "Word.docx"));
        }
    }
}
