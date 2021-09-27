using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibCompress;
using Lab2_EDII.Models;

namespace Lab2_EDII.Utils
{
    public class Storage
    {
        private static Storage _instance = null;

        public static Storage Instance
        {
            get
            {
                if (_instance == null) _instance = new Storage();
                return _instance;
            }
        }
        public Huffman<HuffmanM> Arbol;
        public LZW<HuffmanM> Tabla;
        public List<Datos> Datos = new List<Datos>();
    }
}
