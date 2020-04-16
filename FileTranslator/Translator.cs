using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace FileTranslator
{
    class Translator
    {
        public static void TranslateCsvFile(string fromLanguage, string toLanguage,string fileName, string fileType)
        {
            string filePath = @"..\..\..\Files\" + fileType + @"\" + fileName + "." + fileType;
            string translatedFilePath = @"..\..\..\Files\" + fileType + @"\"+fileName+"Translated.txt";
            string temporaryFilePath = @"..\..\..\Files\" + fileType + @"\"+fileName+"Temporary." + fileType;

            string tLanguage = toLanguage;
            string fLanguage = fromLanguage;

            //Open and translate file until is not empty
            for (; ; )
            {
                using (StreamReader fileToTranslate = new StreamReader(filePath, Encoding.UTF8))
                {
                    string line;
                    string name = null;
                    bool request = true;

                    StreamWriter translated = new StreamWriter(translatedFilePath, true, Encoding.UTF8);
                    StreamWriter temporary = new StreamWriter(temporaryFilePath, false, Encoding.UTF8);

                    while ((line = fileToTranslate.ReadLine()) != null)
                    {
                        //While google didn't block us
                        if (request)
                        {
                            try
                            {
                                name = GoogleTranslatorAPI.Translate(line, fLanguage,tLanguage);
                                name = name.Replace(name.Substring(0, 1), name.Substring(0, 1).ToUpper());//Change first letter to big ex. 123;box -> Box

                                //Search and replace special chars which aren't translated
                                if (name.IndexOf(@"\u0026") != -1) name = name.Replace(@"\u0026", "&");
                                if (name.IndexOf(@"\u003e") != -1) name = name.Replace(@"\u003e", ">");
                                if (name.IndexOf(@"\u003c") != -1) name = name.Replace(@"\u003c", "<");
                                if (name.IndexOf(@"\u200b") != -1) name = name.Replace(@"", "?");

                                translated.Write(name);
                                translated.Write(Environment.NewLine);
                                translated.Flush();
                            }

                            //Add one not translated line where the system get crash
                            catch (Exception)
                            {
                                name = line;
                                temporary.Write(name);
                                temporary.Write(Environment.NewLine);
                                request = false;
                            }
                        }
                        else
                        {
                            name = line;
                            temporary.Write(name);
                            temporary.Write(Environment.NewLine);
                        }
                    }
                    translated.Close();
                    temporary.Close();
                }
                File.Delete(filePath);
                File.Move(temporaryFilePath, filePath);

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
