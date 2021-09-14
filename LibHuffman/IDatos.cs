using System;
using System.Collections.Generic;
using System.Text;

namespace LibHuffman
{
    public interface IDatos
    {
        double Probabilidad();
        int Frecuencia();
        byte Valor();
        void AgregarFrecuencia(int num);
        void Byte(byte valor);
        void CalculoP(double totalBytes);
        void MandarP(double num);
    }
}
