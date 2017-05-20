using System;
using System.IO;
using System.Security.Cryptography;

namespace Enigma
{
    class CryptoMachine:IDisposable
    {
        private bool disposed = false;
        private SymmetricAlgorithm alg;
        
        public CryptoMachine(SymmetricAlgorithm alg)
        {
            this.alg = alg;
        }

        public void Encode(String fileToEncrypt, String fileToWrite)
        {
            alg.GenerateKey();
            alg.GenerateIV();
            File.WriteAllLines(String.Format("{0}.key.txt", Path.GetFileNameWithoutExtension(fileToEncrypt)),
                new string[] { Convert.ToBase64String(alg.Key), Convert.ToBase64String(alg.IV) });

            using (FileStream fsIn = new FileStream(fileToEncrypt, FileMode.Open))
            {
                using (FileStream fsCrypt = new FileStream(fileToWrite, FileMode.Create))
                {
                    using (ICryptoTransform encryptor = alg.CreateEncryptor())
                    {
                        using (CryptoStream cs = new CryptoStream(fsIn, encryptor, CryptoStreamMode.Read))
                        {
                            cs.CopyTo(fsCrypt);
                        }
                    }
                }
            }
        }

        public void Decode(String fileToDecrypt, String fileToWrite, String keyFile)
        {

            String keyStr, ivStr;

            using (StreamReader fsIn = new StreamReader(keyFile))
            {
                keyStr = fsIn.ReadLine();
                ivStr = fsIn.ReadLine();
            }

            if (keyStr == null || ivStr == null)
            {
                throw new Exception("Key file corrupted");
            }

            alg.Key = Convert.FromBase64String(keyStr);
            alg.IV = Convert.FromBase64String(ivStr);

            using (FileStream fsCrypted = new FileStream(fileToDecrypt, FileMode.Open))
            {
                using (FileStream fsOut = new FileStream(fileToWrite, FileMode.Create))
                {
                    using (ICryptoTransform decryptor = alg.CreateDecryptor())
                    {
                        using (CryptoStream cs = new CryptoStream(fsCrypted, decryptor, CryptoStreamMode.Read))
                        {
                            cs.CopyTo(fsOut);
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                if(alg!=null)
                    alg.Dispose();
            }
            
            disposed = true;
        }

    }
}
