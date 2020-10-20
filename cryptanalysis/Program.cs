using System;
using System.IO;
using System.Security.Cryptography;

namespace cryptanalysis {
  class Program {
		private static string Encrypt(byte[] key, string secretString) {
			DESCryptoServiceProvider csp = new DESCryptoServiceProvider();
			MemoryStream ms = new MemoryStream();
			CryptoStream cs = new CryptoStream(ms,
				csp.CreateEncryptor(key, key),
				CryptoStreamMode.Write);

			StreamWriter sw = new StreamWriter(cs);
			sw.Write(secretString);
			sw.Flush();
			cs.FlushFinalBlock();
			sw.Flush();

			return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
		}

    static void Main(string[] args) {
			if (args.Length < 2) {
				Console.WriteLine("Not enough argumets");
				return;
			}

			string plainText = args[0];
			string secretString = args[1];

			DateTime start = new DateTime(2020, 7, 3, 11, 0, 0);
			DateTime end = new DateTime(2020, 7, 4, 11, 0, 0);
			
			// Console.WriteLine("{0} {1}", plainText, secretString);
			for (DateTime dt = start; dt < end; dt = dt.AddMinutes(1)) {
				TimeSpan ts = dt.Subtract(new DateTime(1970, 1, 1));

				int seed = (int)ts.TotalMinutes;
				Random rng = new Random(seed);
				byte[] key = BitConverter.GetBytes(rng.NextDouble());
				
				string result = Encrypt(key, plainText);
				// Console.WriteLine(result);
				if (result == secretString) {
					Console.WriteLine(seed);
					break;
				}
			}
    }
  }
}
