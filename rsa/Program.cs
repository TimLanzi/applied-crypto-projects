using System;
using System.Numerics;

namespace rsa {
  class Program {
		static BigInteger CalcPrime(int e, int c) {
			return BigInteger.Pow(2, e) - c;
		}

		static BigInteger ExtendedEuclideanAlgorithm(BigInteger e, BigInteger phi) {
			BigInteger t = 0;
			BigInteger new_t = 1;
			BigInteger r = phi;
			BigInteger new_r = e;

			while (new_r != 0) {
				BigInteger quotient = r / new_r;
				
				BigInteger temp = new_t;
				new_t = t - (quotient * temp);
				t = temp;

				temp = new_r;
				new_r = r - (quotient * temp);
				r = temp;
			}

			if (r > 1) {
				Console.WriteLine("Cannot be inverted");
			}

			if (t < 0) {
				t = t + phi;
			}

			return t;
		}

		static BigInteger Encrypt(BigInteger e, BigInteger n, BigInteger message) {
			return BigInteger.ModPow(message, e, n);
		}

		static BigInteger Decrypt(BigInteger d, BigInteger n, BigInteger cipher) {
			return BigInteger.ModPow(cipher, d, n);
		}

		static bool VerifyPrivateKey(BigInteger e, BigInteger d, BigInteger phi) {
			return (e * d) % phi == 1;
		}

    static void Main(string[] args) {
			if (args.Length < 6) {
				Console.WriteLine("Not enough arguments");
			}

			int p_e = Int32.Parse(args[0]);
			int p_c = Int32.Parse(args[1]);
			int q_e = Int32.Parse(args[2]);
			int q_c = Int32.Parse(args[3]);
			BigInteger ciphertext = BigInteger.Parse(args[4]);
			BigInteger message = BigInteger.Parse(args[5]);

			BigInteger p = CalcPrime(p_e, p_c);
			BigInteger q = CalcPrime(q_e, q_c);
			// Console.WriteLine(p);
			// Console.WriteLine(q);

			int e = 65537;
			BigInteger n = p * q;
			BigInteger phi = (p - 1) * (q - 1);

			BigInteger d = ExtendedEuclideanAlgorithm(e, phi);
			// Console.WriteLine(d);

			// Verify d is correct
			// Console.WriteLine(VerifyPrivateKey(e, d, phi));

			BigInteger cipher = Encrypt(e, n, message);
			BigInteger plain = Decrypt(d, n, ciphertext);

			// Console.WriteLine("Encrypted: {0}", cipher);
			// Console.WriteLine("Decrypted: {0}", plain);

			Console.WriteLine("{0},{1}", plain, cipher);
    }
  }
}
