﻿using System;
using System.IO;
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
