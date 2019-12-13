using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Threading;
using System.Globalization;

namespace FileTranslator
{
    class Program
    {
        static void Main(string[] args)
        {
            CsvTranslator.TranslateCsvFile("DE");
        }

    }
}
