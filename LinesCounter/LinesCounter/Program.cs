using System;
using System.IO;
namespace LinesCounter
{
    class Program
    {
        private static int CheckDir(String path, String fileExt)        //Проверка директории
        {
            
            int linesCount = 0;
            String[] directories = Directory.GetDirectories(path);      //Получение имен всех поддиректорий и их обход рекурсивно в глубину
            foreach (var dir in directories)
            {
                linesCount += CheckDir(dir, fileExt);
            }
            String[] files = Directory.GetFiles(path);                  //Получение имен всех файлов в директории и их обход
            foreach (var file in files)
            {
                linesCount += CheckFile(file, fileExt);
            }
            return linesCount;
        }

        private static int CheckFile(String path, String fileExt)           //Проверка файла
        {
            int linesCount = 0;
            bool inMultiComm = false;

            if (path.EndsWith(fileExt))                                     //Eсли файл обладает указанным расширением...
            {
                using (StreamReader input = new StreamReader(path))
                {
                    String str;

                    while ((str = input.ReadLine()) != null)
                    {
                        str = str.Trim();

                        if (str.Equals(String.Empty) || str.StartsWith("//"))
                        {
                            continue;
                        }

                        if (inMultiComm)
                        {
                            if (str.Contains("*/"))
                            {
                                str = str.Substring(str.LastIndexOf("*/") + 2);
                                inMultiComm = false;
                            }
                            else {
                                continue;
                            }
                        }

                        while (str.Contains("/*"))
                        {
                            if (str.Contains("*/"))
                            {
                                str = str.Substring(str.LastIndexOf("*/") + 2);
                            }
                            else
                            {
                                inMultiComm = true;
                                str = str.Substring(0, str.IndexOf("/*"));
                                break;
                            }
                        }

                        str = str.Trim();

                        if (!str.Equals(String.Empty))
                        {
                            linesCount++;
                            Console.WriteLine(str);
                        }
                    }
                }
            }
            return linesCount;
        }


        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                String ext = Path.GetExtension(args[0]);
                if (ext.Equals(System.String.Empty) || ext == null)
                {
                    Console.WriteLine("Wrong argument format. Please, enter '.<file extension>' as an argument");
                    Environment.Exit(0);
                }
                try
                {
                    String dir = Directory.GetCurrentDirectory();
                    int linesCount = CheckDir(dir, ext);
                    Console.WriteLine(linesCount + " lines");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Please, enter '.<file extension>' as an argument");
            }
        }
    }
}