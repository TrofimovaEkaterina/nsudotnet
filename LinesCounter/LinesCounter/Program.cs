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
                String ext = Path.GetExtension(args[0]);
                if (ext.Equals(System.String.Empty) || ext == null)
                {
                    Console.WriteLine("Wrong argument format. Please, enter '.<file extension>' as an argument");
                    Environment.Exit(0);
                }
                try
                {
                    String dir = Directory.GetCurrentDirectory();       //Получение текущуей директорию
                    int linesCount = CheckDir(dir, ext);                //Запуск обхода
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