using System;
using System.Collections.Generic;
using System.Text;

namespace LibHuffman
{
    class HuffmanNode<T> where T : IDatos, new()
    {
        #region Variables
        public T Valor;
        public string rut;
        public HuffmanNode<T> Padre;
        public HuffmanNode<T> hijod;
        public HuffmanNode<T> hijoi;
        #endregion

        public HuffmanNode(T value)
        {
            Valor = value;
        }
    }
}
