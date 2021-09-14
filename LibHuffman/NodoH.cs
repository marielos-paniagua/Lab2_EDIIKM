using System;
using System.Collections.Generic;
using System.Text;

namespace LibHuffman
{
    class HuffmanNode<T> where T : IDatos, new()
    {
        #region Variables
        public T Value;
        public string Code;
        public HuffmanNode<T> Padre;
        public HuffmanNode<T> hijod;
        public HuffmanNode<T> hijoi;
        #endregion

        public HuffmanNode(T value)
        {
            Value = value;
        }
    }
}
