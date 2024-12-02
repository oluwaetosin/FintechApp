using System.Security.Cryptography;
public static class AESEncryptionService
{


    public static string Encrypt(string plainText, byte[] encryptionKey,  byte[] encryptionIV)
    {

        // Create a new AesManaged.
        using (Aes aesAlgorithm = Aes.Create())
        {
               
            var encryptor = aesAlgorithm.CreateEncryptor(encryptionKey, encryptionIV);
            
                
                // Create encryptor object
                

                byte[] encryptedData;

                //Encryption will be done in a memory stream through a CryptoStream object
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                        encryptedData = ms.ToArray();
                    }
                }

                return Convert.ToBase64String(encryptedData);
        }
        // Return encrypted data

    }


    public static  string DecryptDataWithAes(string cipherText, byte[] encryptionKey, byte[] encryptionIV)
    {
        using (Aes aesAlgorithm = Aes.Create())
        {
             
            

            // Create decryptor object
            ICryptoTransform decryptor = aesAlgorithm.CreateDecryptor(encryptionKey, encryptionIV);

            byte[] cipher = Convert.FromBase64String(cipherText);

            //Decryption will be done in a memory stream through a CryptoStream object
            using (MemoryStream ms = new MemoryStream(cipher))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }
    }

    public static string GenerateSecretKey()
{
    using (var aes = Aes.Create())
    {
        aes.KeySize = 256; // AES key size can be 128, 192, or 256 bits.
        aes.GenerateKey(); // Generates a random key
        aes.GenerateIV();
        Console.WriteLine(Convert.ToBase64String(aes.IV));
        return Convert.ToBase64String(aes.Key) + "and" + Convert.ToBase64String(aes.IV);
       
    }
}

}


