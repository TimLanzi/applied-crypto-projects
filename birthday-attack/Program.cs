/*
Tim Lanzi
August 2020

CSE 539
Birthday attack hashing project
*/

using System;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace hash {
  class Program {
    // Takes a full hash and returns only the first 5 bytes
    static byte[] SplitHash(byte[] hash) {
      byte[] newhash = new byte[5];
      for (int i = 0; i < 5; i++) {
        newhash[i] = hash[i];
      }

      return newhash;
    }
    
    // Creates a random string
    static string RandomString() {
      Random random = new Random();
      const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
      // const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

      return new string(Enumerable.Repeat(chars, 10)
        .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    // Adds salt to a provided hash byte array
    static void AddSalt(byte salt, ref byte[] arr) {
      Array.Resize(ref arr, arr.Length + 1);
      arr[arr.GetUpperBound(0)] = salt;
    }

    static void Main(string[] args) {
      // Validation
      if (args.Length <= 0) {
        Console.WriteLine("Needs an argument");
        return;
      }

      // Get salt
      byte salt = Convert.ToByte(args[0], 16);
      
      MD5 md5 = MD5.Create();

      // Dict for storing hashes
      var dict = new Dictionary<string, string>();

      // Diagnostic vars
      // string foundVal = "";
      // string foundMsg = "";

      bool found = false;
      while(!found) {
        // Create message and add salt
        string message = RandomString();
        byte[] endcoded = Encoding.UTF8.GetBytes(message);
        AddSalt(salt, ref endcoded);

        // Hash and convert to string
        byte[] hash = md5.ComputeHash(endcoded);
        hash = SplitHash(hash);
        string hashAsStr = BitConverter.ToString(hash).Replace("-", "");

        // Check whether hash is in dictionary or not
        string value = "";
        if (dict.TryGetValue(hashAsStr, out value)) {

          // Make sure the messages aren't the same
          if (!message.Equals(value)) {
            Console.WriteLine("{0},{1}", message, value);
            found = true;
            // foundVal = value;
            // foundMsg = message;
          }
        } else {
          dict.Add(hashAsStr, message);
        }
      }

      // Diagnostic
      // byte[] en1 = Encoding.UTF8.GetBytes(foundMsg);
      // byte[] en2 = Encoding.UTF8.GetBytes(foundVal);
      // AddSalt(salt, ref en1);
      // AddSalt(salt, ref en2);
      // string one = BitConverter.ToString(md5.ComputeHash(en1)).Replace("-", " ");
      // string two = BitConverter.ToString(md5.ComputeHash(en2)).Replace("-", " ");
      // Console.WriteLine("\n");
      // Console.WriteLine("{0}", one);
      // Console.WriteLine("{0}", two);
    }
  }
}
