using System;
using System.IO;
using System.Security.Cryptography;

namespace Enigma
{
    class CryptoMachine
    {
        SymmetricAlgorithm algorithm;
        public CryptoMachine(SymmetricAlgorithm alg)      //На вход получает алгоритм шифрования
        {
            algorithm = alg;
        }

        private String getFileName(string path)
        {
            int idx = path.LastIndexOf('.');
            if (idx < 0)
            {
                return path;
            }
            return path.Substring(0, idx);
        }


        public void Encode(String input, String output)            {
            algorithm.GenerateKey();
            algorithm.GenerateIV();
            File.WriteAllLines(String.Format("{0}.key.txt", getFileName(input)),
                new string[] { Convert.ToBase64String(algorithm.Key), Convert.ToBase64String(algorithm.IV) });

            new CryptoStream(new FileStream(input, FileMode.Open),
                algorithm.CreateEncryptor(),
                CryptoStreamMode.Read)
                .CopyTo(new FileStream(output, FileMode.Create));

        }

        public void Decode(String input, String output, String keyFile)  
        {

            String keyStr, ivStr;
            StreamReader inStream = new StreamReader(keyFile);      //Открытие потока на чтение

            keyStr = inStream.ReadLine();
            ivStr = inStream.ReadLine();

            if (keyStr == null || ivStr == null)                    //Простенькая проверка на целостность
            {
                Console.WriteLine("key file corrupted");
                Environment.Exit(0);
            }

            keyStr.Replace(Environment.NewLine, "");
            ivStr.Replace(Environment.NewLine, "");
            algorithm.Key = Convert.FromBase64String(keyStr);
            algorithm.IV = Convert.FromBase64String(ivStr);

            new CryptoStream(new FileStream(input, FileMode.Open),
                algorithm.CreateDecryptor(),
                CryptoStreamMode.Read)
                .CopyTo(new FileStream(output, FileMode.Create));
        }


    }
}
