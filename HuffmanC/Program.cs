using System;
using LibCompress;
using Lab2_EDII.Models;

namespace HuffmanC
{
    class Program
    {
        static void Main(string[] args)
        {
            Huffman<HuffmanM> MHuffman = new Huffman<HuffmanM>();
            LZW<HuffmanM> MLZW= new LZW<HuffmanM>();
            try
            {
            Metodo:
                Console.WriteLine("Método a utilizar");
                string metodo = Console.ReadLine();
                string comprimido = "";
                Console.WriteLine("Texto a comprimir");
                string texto = Console.ReadLine();
                Console.WriteLine("Compresión:");
                switch (metodo)
                {
                    case "huffman":                     
                        comprimido = MHuffman.Comprimir(texto);
                        break;
                    case "lzw":
                        comprimido = MLZW.Comprimir(texto);
                        break;
                    default:
                        Console.WriteLine("Ingrese un método válido");
                        goto Metodo;
                }
                Console.WriteLine(comprimido);
                Console.WriteLine("Para descomprimir, escriba 'ok'. De lo contrario, presione enter");
                if (Console.ReadLine() == "ok")
                {
                    Console.WriteLine("Descompresión:");
                    switch (metodo)
                    {
                        case "huffman":
                            Console.WriteLine(MHuffman.DescomprimirT(comprimido));
                            break;
                        case "lzw":
                            Console.WriteLine(MLZW.DescomprimirT(comprimido));
                            break;
                        default:
                            break;
                    }
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
