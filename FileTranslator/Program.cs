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
            string fromLanguage;
            string toLanguage;
            string fileType;
            string fileName;

            Console.Write("File name?: ");
            fileName = Console.ReadLine();

            Console.Write("File type?(csv, txt, etc.): ");
            fileType = Console.ReadLine();

            Console.Write("From which language would you like to do translation?: ");
            fromLanguage = Console.ReadLine();

            Console.Write("To which language would you like to do translation?: ");
            toLanguage = Console.ReadLine();

            Translator.TranslateCsvFile(fromLanguage, toLanguage, fileName, fileType);

        }

    }
}
