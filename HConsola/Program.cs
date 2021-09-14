using System;
using LibHuffman;
using Lab2_EDII.Models;

namespace HConsola
{
    class Program
    {
        static void Main(string[] args)
        {
            Huffman<HuffmanM> MHuffman = new Huffman<HuffmanM>();
            try
            {
                Console.WriteLine("Texto a comprimir");
                string texto = Console.ReadLine();
                Console.WriteLine("Compresión:");
                Console.WriteLine(MHuffman.ComprimirT(texto));
                Console.WriteLine("Para descomprimir, escriba 'ok'. De lo contrario, presione enter");
            }
            catch (Exception)
            {
                Console.WriteLine("Ingrese texto válido");
                Console.ReadLine();
            }
        }
    }
}
