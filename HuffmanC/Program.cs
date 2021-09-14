using System;
using LibHuffman;
using Lab2_EDII.Models;

namespace HuffmanC
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
                string comprimido = MHuffman.Comprimir(texto);
                Console.WriteLine(comprimido);
                Console.WriteLine("Para descomprimir, escriba 'ok'. De lo contrario, presione enter");
                if (Console.ReadLine() == "ok")
                {
                    Console.WriteLine("Descompresión:");
                    Console.WriteLine(MHuffman.DescomprimirT(comprimido));
                    Console.ReadLine();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
                Console.ReadLine();
            }
        }
    }
}
