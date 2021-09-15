using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using Lab2_EDII.Utils;

namespace Lab2_EDII.Models
{
    public class Datos
    {
        public string NOriginal { get; set; }
        public string Ncomprimido { get; set; }
        public string Ruta { get; set; }
        public double RazonC { get; set; }
        public double FactorC { get; set; }
        public double Procentaje { get; set; }

        public Datos() { }

        public void AgregarDatos(string ruta, string original, string nuevo)
        {
            var ContO = System.IO.File.ReadAllBytes($"{ruta}/{original}");
            var FileP = $"{ruta}/{nuevo}";
            double BinarioO = ContO.Count();
            var ContN = System.IO.File.ReadAllBytes($"{ruta}/{nuevo}");
            double BinarioN = ContN.Count();

            NOriginal = original;
            Ncomprimido = nuevo;
            Ruta = FileP;
            Razon(BinarioN, BinarioO);
            Factor(BinarioN, BinarioO);
            Porcent();
            Dato(ruta);
            Storage.Instance.Datos.Add(this);
            var file = new FileStream($"{ruta}/DatosC", FileMode.OpenOrCreate);
            var B = JsonSerializer.Serialize(Storage.Instance.Datos, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            using var escribir = new StreamWriter(file);
            escribir.WriteLine(B);
            escribir.Close();
            file.Close();
        }

        public static void Dato(string path)
        {
            var file = new FileStream($"{path}/DatosC", FileMode.OpenOrCreate);
            if (file.Length != 0)
            {
                Storage.Instance.Datos.Clear();
                using var reader = new StreamReader(file);
                var enviar = reader.ReadToEnd();
                var aenviar = JsonSerializer.Deserialize<List<Datos>>(enviar, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                foreach (var item in aenviar)
                {
                    Storage.Instance.Datos.Add(item);
                }
            }
            file.Close();
        }

        public void Razon(double Comprimido, double Original)
        {
            RazonC = Math.Round(Comprimido / Original, 4);
        }

        public void Factor(double Comprimido, double Original)
        {
            FactorC = Math.Round(Original / Comprimido, 3);
        }

        public void Porcent()
        {
            Procentaje = Math.Round(RazonC * 100, 5);
        }
    }
}
