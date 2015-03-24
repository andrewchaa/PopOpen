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
            var file = Path.Combine(Environment.CurrentDirectory, "Word.docx");
            PopOpen.Open(file);
            
        }
    }
}
