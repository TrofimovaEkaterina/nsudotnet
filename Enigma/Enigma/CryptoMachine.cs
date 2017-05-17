using System;
using System.IO;
using System.Security.Cryptography;

namespace Enigma
{
    class CryptoMachine
    {
        SymmetricAlgorithm Alg;
        public CryptoMachine(SymmetricAlgorithm alg)
        {
            Alg = alg;
        }

        public void Encode(String fileToEncrypt, String fileToWrite)
        {
            Alg.GenerateKey();
            Alg.GenerateIV();
            File.WriteAllLines(String.Format("{0}.key.txt", Path.GetFileNameWithoutExtension(fileToEncrypt)),
                new string[] { Convert.ToBase64String(Alg.Key), Convert.ToBase64String(Alg.IV) });

            using (FileStream fsIn = new FileStream(fileToEncrypt, FileMode.Open))
            {
                using (FileStream fsCrypt = new FileStream(fileToWrite, FileMode.Create))
                {
                    using (CryptoStream cs = new CryptoStream(fsIn, Alg.CreateEncryptor(), CryptoStreamMode.Read))
                    {
                        cs.CopyTo(fsCrypt);
                    }
                }
            }
        }

        public void Decode(String fileToDecrypt, String fileToWrite, String keyFile)
        {

            String keyStr, ivStr;
            StreamReader inStream = new StreamReader(keyFile);

            keyStr = inStream.ReadLine();
            ivStr = inStream.ReadLine();

            if (keyStr == null || ivStr == null)
            {
                throw new Exception("Key file corrupted"); 
            }
            
            Alg.Key = Convert.FromBase64String(keyStr);
            Alg.IV = Convert.FromBase64String(ivStr);

            using (FileStream fsCrypted = new FileStream(fileToDecrypt, FileMode.Open))
            {
                using (FileStream fsOut = new FileStream(fileToWrite, FileMode.Create))
                {
                    using (CryptoStream cs = new CryptoStream(fsCrypted, Alg.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        cs.CopyTo(fsOut);
                    }
                }
            }
        }

    }

}
