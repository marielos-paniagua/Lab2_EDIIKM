using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibCompress;

namespace Lab2_EDII.Models
{
    public class HuffmanM : IDatos
    {
        int Frequency;
        double Probability;
        byte Value;

        public HuffmanM() { }

        public void AgregarFrecuencia(int num)
        {
            Frequency += num;
        }

        public void CalculoP(double totalBytes)
        {
            Probability = Convert.ToDouble(Frequency) / totalBytes;
        }

        public int Frecuencia()
        {
            return Frequency;
        }

        public double Probabilidad()
        {
            return Probability;
        }

        public byte Valor()
        {
            return Value;
        }

        public void Byte(byte value)
        {
            Value = value;
        }

        public void MandarP(double number)
        {
            Probability = number;
        }
    }
}
