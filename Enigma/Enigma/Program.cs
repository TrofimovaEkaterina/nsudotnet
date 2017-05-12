using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace Enigma
{
    class Program
    {


        private static void error(String msg)   //Функция вывода семантической ошибки и завершения работы программы
        {
            Console.WriteLine(msg);
            Environment.Exit(0);
        }

        private static SymmetricAlgorithm getAlgByName(String algName)  //Получение алгоритма по имени
        {
            SymmetricAlgorithm alg = null;
            switch (algName)
            {
                case "aes":
                    alg = new AesManaged();    
                    break;
                case "des":
                    alg = new DESCryptoServiceProvider();
                    break;
                case "rc2":
                    alg = new RC2CryptoServiceProvider();
                    break;
                case "rijndael":
                    alg = new RijndaelManaged();
                    break;
            }
            return alg;
        }


        //Enigma.exe encrypt file.txt rc2 output.bin    
        //Enigma.exe decrypt output.bin rc2 file.key.txt file.txt
        static void Main(string[] args)
        {
            if ((args.Length == 4) || (args.Length == 5))           //Проверка числа аргументов
            {
                
                try                                                 
                {
                    String input = args[1];                                         //Имя файла на чтение
                    String output = (args.Length == 4) ? (args[3]) : (args[4]);     //Имя файла на запись

                    var alg = getAlgByName(args[2].ToLower());      //Получение алгоритма по аргументу
                    if ((alg) == null)                              //Если имя алгоритма не входит в список поддерживаемых программой -- ошибка
                    {
                        error("Unsupported encryption's type");
                    }

                    CryptoMachine cryptoMachine = new CryptoMachine(alg);    //Создание кодера-декодера

                    switch (args[0])                                         //Кодирование/декодирование
                    {
                        case "encrypt":
                            cryptoMachine.Encode(input, output);                                                                                                                        
                            break;
                        case "decrypt":
                            if (args.Length != 5)                   //Не хватает данных для декодирования
                            {
                                error("Not enough arguments");
                            }
                            
                            cryptoMachine.Decode(input, output, args[3]);   
                            break;
                        default:
                            error("Unsupported command");           //Неподдерживаемая команда
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                error("Not enough arguments");
            }
        }
        
    }
}
