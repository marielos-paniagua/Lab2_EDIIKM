using System;
using System.Collections.Generic;
using System.Text;

namespace LibHuffman
{
    internal class NodoCP<T> : ICloneable
    {
        public NodoCP<T> Padre;
        public NodoCP<T> HijoD;
        public NodoCP<T> HijoI;
        public T Valor;
        public double Prioridad;

        public NodoCP() { }

        public NodoCP(T valor, double prioridad)
        {
            Valor = valor;
            Prioridad = prioridad;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
