using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace FileTranslator
{
    class CsvTranslator
    {
        public static void TranslateCsvFile(string lang)
        {
            //Open and translate txt from file until the file is not empty
            for (; ; )
            {
                using (StreamReader sr = new StreamReader(@"C:\Users\Sebastian\source\repos\FileTranslator\FileTranslator\csvFiles\ToTranslate.csv", Encoding.UTF8))
                {
                    string line;

                    string language = lang.ToUpper();

                    string script = null;

                    bool request = true;

                    string index = null;
                    string name = null;

                    StreamWriter sw = new StreamWriter(@"C:\Users\Sebastian\source\repos\FileTranslator\FileTranslator\csvFiles\Translated.txt", true, Encoding.UTF8);
                    StreamWriter temporary = new StreamWriter(@"C:\Users\Sebastian\source\repos\FileTranslator\FileTranslator\csvFiles\temporary.csv", false, Encoding.UTF8);

                    while ((line = sr.ReadLine()) != null)
                    {

                        if (line == ";")
                        {
                            continue;
                        }

                        //While google didn't block us
                        if (request)
                        {
                            try
                            {
                                index = line.Substring(0, line.IndexOf(';'));//Get all before ';'

                                name = GoogleTranslatorAPI.Translate((line.Substring(line.IndexOf(';') + 1)), language);//Get all after ';' and 
                                name = name.Replace(name.Substring(0, 1), name.Substring(0, 1).ToUpper());//Change first letter to big ex. 123;box -> Box


                                //Search and replace special chars which aren't translated
                                if (name.IndexOf(@"\u0026") != -1) name = name.Replace(@"\u0026", "&");
                                if (name.IndexOf(@"\u003e") != -1) name = name.Replace(@"\u003e", ">");
                                if (name.IndexOf(@"\u003c") != -1) name = name.Replace(@"\u003c", "<");
                                if (name.IndexOf(@"\u200b") != -1) name = name.Replace(@"", "?");


                                script = $"execute procedure DP_DODAJ_POPRAW_JEZYKI '{index}', '{language}', '{name}';";

                                sw.Write(script);
                                sw.Write(Environment.NewLine);
                                sw.Flush();

                            }

                            //Add one not translated line where the system get crash
                            catch (Exception)
                            {
                                index = line.Substring(0, line.IndexOf(';'));
                                name = line.Substring(line.IndexOf(';') + 1);

                                temporary.Write(index);
                                temporary.Write(@";");
                                temporary.Write(name);
                                temporary.Write(Environment.NewLine);

                                request = false;
                            }
                        }
                        else
                        {
                            index = line.Substring(0, line.IndexOf(';'));
                            name = line.Substring(line.IndexOf(';') + 1);

                            temporary.Write(index);
                            temporary.Write(@";");
                            temporary.Write(name);
                            temporary.Write(Environment.NewLine);
                        }
                    }
                    sw.Close();
                    temporary.Close();
                }
                File.Delete(@"C:\Users\Sebastian\source\repos\FileTranslator\FileTranslator\csvFiles\ToTranslate.csv");
                File.Move(@"C:\Users\Sebastian\source\repos\FileTranslator\FileTranslator\csvFiles\temporary.csv", @"C:\Users\Sebastian\source\repos\FileTranslator\FileTranslator\csvFiles\ToTranslate.csv");

                for (int j = 0; j < 3600; j++)
                {
                    Console.WriteLine(j + "/3600");
                    Thread.Sleep(1000);
                    Console.Clear();
                }
            }
        }
    }
}
