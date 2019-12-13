using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileTranslator
{
    //
    //Summary:
    //  Text File Translator
    //
    //Output:
    //  Create .txt file with translated senteces
    //
    class TxtTranslator
    {
        public static void TranslateTxtFile(string lang)
        {
            //TODO : Check if the file exists by instruction File.Exists(); //NOT NEEDE NOW
            using (StreamReader sr = new StreamReader(@"C:\Users\Sebastian\source\repos\FileTranslator\FileTranslator\txtFiles\ToTranslate.txt", Encoding.UTF8))
            {
                string line = null;

                //Begin and end of the sentence
                int fId;
                int sId;

                //If google server will block script then change request to false
                bool request = true;

                string beforeTranslation = null;
                string afterTranslation = null;

                //Temporary file where the not translated sentences are writed for a while.
                StreamWriter temporary = new StreamWriter(@"C:\Users\Sebastian\source\repos\FileTranslator\FileTranslator\txtFiles\temporary.txt", true, Encoding.UTF8);

                //File where all sentences are translated
                StreamWriter sw = new StreamWriter(@"C:\Users\Sebastian\source\repos\FileTranslator\FileTranslator\txtFiles\fromPLtoDE.txt", true, Encoding.UTF8);

                while ((line = sr.ReadLine()) != null)
                {
                    if (request == true)
                    {
                        try
                        {
                            //Enter
                            if (String.IsNullOrEmpty(line))
                            {
                                sw.Write(Environment.NewLine);
                                sw.Flush();
                                continue;
                            }
                            else
                            {
                                fId = line.IndexOf('>');
                                sId = line.IndexOf("</");
                                beforeTranslation = ">" + line.Substring(fId + 1, sId - fId - 1) + "</";
                                afterTranslation = ">" + GoogleTranslatorAPI.Translate(line.Substring(fId + 1, sId - fId - 1), lang) + "</";

                                //Write datas stored in variable line to file, but before that, replace old sentence to translated sentence
                                sw.Write(line.Replace(beforeTranslation, afterTranslation));
                                sw.Write(Environment.NewLine);
                                sw.Flush();
                            }
                        }
                        catch (Exception)
                        {
                            request = false;

                            temporary.Write(line);
                            temporary.Write(Environment.NewLine);
                            temporary.Flush();
                        }
                    }

                    //If google server throw exception then write all not translated datas to temporary file
                    else
                    {
                        if (String.IsNullOrEmpty(line))
                        {
                            temporary.Write(Environment.NewLine);
                            temporary.Flush();
                            continue;
                        }
                        else
                        {
                            temporary.Write(line);
                            temporary.Write(Environment.NewLine);
                            temporary.Flush();
                        }
                    }
                }
                temporary.Close();
                sw.Close();
            }

            //Delete main file and replace him by temporary file with datas which will be translated in the next time
            File.Delete(@"C:\Users\Sebastian\source\repos\FileTranslator\FileTranslator\txtFiles\ToTranslate.txt");
            File.Move(@"C:\Users\Sebastian\source\repos\FileTranslator\FileTranslator\txtFiles\temporary.txt", @"C:\Users\Sebastian\source\repos\FileTranslator\FileTranslator\txtFiles\ToTranslate.txt");
        }
    }
}
