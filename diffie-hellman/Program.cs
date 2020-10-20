/*
Tim Lanzi
September 2020

Diffie-Hellman project
*/

using System;
using System.IO;
using System.Numerics;
using System.Linq;
using System.Security.Cryptography;

namespace diffie_hellman {
  class Program {
    // Helper function converts a string to a byte array
    public static byte[] StringToByteArray(string hex) {
      string cleanedHex = hex.Replace(" ", "");

      return Enumerable.Range(0, cleanedHex.Length / 2)
        .Select(x => Convert.ToByte(cleanedHex.Substring(x*2, 2), 16))
        .ToArray();

      // return Enumerable.Range(0, cleanedHex.Length)
      //   .Where(x => x % 2 == 0)
      //   .Select(x => Convert.ToByte(cleanedHex.Substring(x, 2), 16))
      //   .ToArray();
    }

    // Init AES algorithm according to project specifications
    public static Aes InitAES(byte[] iv, byte[] key) {
      Aes aes = Aes.Create();
      aes.KeySize = 256;
      aes.IV = iv;
      aes.Key = key;

      return aes;
    }

    // Decrypt ciphertext using AES
    public static string Decrypt(byte[] cipherText, Aes aes) {
      string plaintext = null;

      ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

      using (MemoryStream msDecrypt = new MemoryStream(cipherText)) {
        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)) {
          using (StreamReader srDecrypt = new StreamReader(csDecrypt)) {
            plaintext = srDecrypt.ReadToEnd();
          }
        }
      }

      return plaintext;
    }

    // Encrypt plaintext using AES
    public static byte[] Encrypt(string plaintext, Aes aes) {
      byte[] encrypted;

      ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

      using (MemoryStream msEncrypt = new MemoryStream()) {
        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write)) {
          using (StreamWriter swEncrypt = new StreamWriter(csEncrypt)) {
            swEncrypt.Write(plaintext);
          }
          encrypted = msEncrypt.ToArray();
        }
      }

      return encrypted;
    }

    static void Main(string[] args) {
      if (args.Length != 9) {
        Console.WriteLine("Not enough arguments.");
        return;
      }
      
      // Grab all command-line args
      string ivStr = args[0];
      int g_e = Int32.Parse(args[1]);
      int g_c = Int32.Parse(args[2]);
      int N_e = Int32.Parse(args[3]);
      int N_c = Int32.Parse(args[4]);
      BigInteger x = BigInteger.Parse(args[5]);
      BigInteger gy = BigInteger.Parse(args[6]);
      string cStr = args[7];
      string p = args[8];

      // Convert IV and C to byte arrays
      byte[] iv = StringToByteArray(ivStr);
      byte[] c = StringToByteArray(cStr);


      // Get g and N
      BigInteger g = new BigInteger(Math.Pow(2, g_e)) - g_c;
      BigInteger N = new BigInteger(Math.Pow(2, N_e)) - N_c;

      // Console.WriteLine("g = {0}", g);
      // Console.WriteLine("N = {0}", N);
      
      // BigInteger gx_mod_n = BigInteger.ModPow(g, x, N);
      // Console.WriteLine("gx mod N = {0}", gx_mod_n);


      // Compute the key
      BigInteger key = BigInteger.ModPow(gy, x, N);

      Aes aes = InitAES(iv, key.ToByteArray());

      string decrypted = Decrypt(c, aes);
      byte[] encrypted = Encrypt(p, aes);
      string encryptedStr = BitConverter.ToString(encrypted).Replace("-", " ");

      // Console.WriteLine("Decrypted: {0}", decrypted);
      // Console.WriteLine("Encrypted: {0}", encryptedStr);
      Console.WriteLine("{0},{1}", decrypted, encryptedStr);
    }
  }
}
