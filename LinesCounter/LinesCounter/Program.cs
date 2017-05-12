using System;
using System.IO;

namespace LinesCounter
{
    class Program
    {

        private static int checkDir(String path, String fileExt)        //Проверка директории
        {
            int linesCount = 0;

            String[] directories = Directory.GetDirectories(path);      //Получение имен всех поддиректорий и их обход рекурсивно в глубину
            foreach (var dir in directories)
            {
                linesCount += checkDir(dir, fileExt);
            }

            String[] files = Directory.GetFiles(path);                  //Получение имен всех файлов в директории и их обход
            foreach (var file in files)
            {
                linesCount += checkFile(file, fileExt);
            }
            return linesCount;
        }

        private static int checkFile(String path, String fileExt)           //Проверка файла
        {
            int linesCount = 0;
            if (path.EndsWith(fileExt))                                     //Eсли файл обладает указанным расширением...
            {
                StreamReader input = new StreamReader(path);                //Открытие потока на чтение
                String str;
                while ((str = input.ReadLine()) != null)                    
                {
                    str = str.Replace(Environment.NewLine, "").Trim();                  //Удаление переносов строк
                    if ((!str.StartsWith("//")) && (!str.Equals(String.Empty)))         //Строка не должна начинаться со слешей и не быть пустой
                    {
                        linesCount++;                                       
                    }
                }
            }
            return linesCount;
        }

        static void Main(string[] args)
        {
            if (args.Length == 1)                                           //Проверка числа аргументов
            {
                String ext;
                
                int idx = args[0].LastIndexOf('.');                         //Взятие "чистого" расширения из аргумента
                if (idx == -1) {
                    ext = "." + args[0];
                } else {
                    ext = args[0].Substring(idx);
                }

                Console.WriteLine(ext);
                try
                    {
                        String dir = Directory.GetCurrentDirectory();       //Получение текущуей директорию
                        int linesCount = checkDir(dir, ext);                //Запуск обхода

                        Console.WriteLine(linesCount + " lines");           
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);                  
                    }
                }
            
            else
            {
                Console.WriteLine("Please, enter <file extension> as an argument");
            }
            Console.ReadKey();
        }
    }
}
