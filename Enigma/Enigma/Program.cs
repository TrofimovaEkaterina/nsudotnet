using System;
using System.Security.Cryptography;

namespace Enigma
{
    class Program
    {
       
        private static SymmetricAlgorithm getAlgByName(String algName)  
        {
            SymmetricAlgorithm alg;
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
                default:
                    throw new Exception("Unsupported encryption algorithm");
            }
            return alg;
        }


        //Enigma.exe encrypt file.txt rc2 output.bin    
        //Enigma.exe decrypt output.bin rc2 file.key.txt file.txt
        static void Main(string[] args)
        {

            if (args.Length < 4)
            {
                Console.WriteLine("Not enough arguments");
            }
            else
            {
                try                                                 
                {
                    var alg = getAlgByName(args[2].ToLower());      
                    
                    CryptoMachine cryptoMachine = new CryptoMachine(alg);
                    
                    switch (args[0])                                        
                    {
                        case "encrypt":
                            cryptoMachine.Encode(args[1], args[3]);                                                                                                                        
                            break;
                        case "decrypt":
                            if (args.Length < 5)
                            {
                                Console.WriteLine("Not enough arguments");
                                break;
                            }
                            cryptoMachine.Decode(args[1], args[4], args[3]);   
                            break;
                        default:
                            Console.WriteLine("Unsupported command");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        
    }
}
