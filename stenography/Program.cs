/*
Tim Lanzi
August 2020

Steganography program for project 1
*/

using System;

namespace steganography {
  class Program {
    static void Main(string[] args) {
      if (args.Length == 0) {
        Console.WriteLine("Need a message");
        return;
      }

      // Create mask to grab the 2 most significant bits (11000000)
      byte mask = 192;

      // Initialize offset to the bmp file header length
      int offset = 26;

      // Grab message string
      string[] messageChars = args[0].Split(" ");

      byte[] bmpBytes = new byte[] {
        0x42, 0x4D, 0x4C, 0X00, 0X00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x1A, 0x00, 0x00, 0x00, 0x0C, 0x00,
        0x00, 0x00, 0x04, 0x00, 0x04, 0x00, 0x01, 0x00,
        0x18, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF,
        0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
        0xFF, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0x00,
        0x00, 0x00, 0xFF, 0x00, 0x00, 0xFF, 0xFF, 0xFF,
        0xFF, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
        0xFF, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0x00,
        0x00, 0x00,
      };

      for (int i = 0; i < messageChars.Length; i++) {
        byte msgByte = Convert.ToByte(messageChars[i], 16);

        // Handle 4 bytes of image per character of message
        for (int j = 0; j < 4; j++) {
          // Grab msb and shift to the lsb position
          byte msb = (byte)(msgByte & mask);
          msb >>= 6;

          // Left shift byte to keep grabbing msb
          msgByte <<= 2;
          
          // XOR image byte with msb
          bmpBytes[offset] ^= msb;
          offset++;
        }
      }

      Console.WriteLine(BitConverter.ToString(bmpBytes).Replace("-", " "));
    }
  }
}
