using System;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace LibCompress
{
    public class Huffman<T> : ICompress where T : IDatos, new()
    {
        HuffmanNode<T> Ruta;
        Dictionary<byte, HuffmanNode<T>> Diccionario;
        ColaP<HuffmanNode<T>> ColaP;
        double cont;
        string FilePath;

        public Huffman() { }
        public Huffman(string filePath)
        {
            FilePath = filePath;
        }
        public string Comprimir(string text)
        {
            var almacenar = new byte[5000];
            Diccionario = new Dictionary<byte, HuffmanNode<T>>();
            almacenar = ConvertirByte.Convertir(text);
            T valor = new T();
            HuffmanNode<T> Nodo;
            cont = 0.00;

            foreach (var Dato in almacenar)
            {
                if (!Diccionario.ContainsKey(Dato))
                {
                    valor.Byte(Dato);
                    Nodo = new HuffmanNode<T>(valor);
                    Diccionario.Add(Dato, Nodo);
                }
                Diccionario[Dato].Valor.AgregarFrecuencia(1);
                cont++;
                valor = new T();
            }
            Arbol();
            CeroUno(Ruta, "");

            string cantidad = "";
            cantidad += ConvertirByte.ConvertirP(ConvertirI(Diccionario.Values.Count));
            foreach (var byteObject in Diccionario.Values)
            {
                cantidad += ConvertirByte.ConvertirP(new byte[] { byteObject.Valor.Valor() }) + ConvertirByte.ConvertirP(ConvertirI(byteObject.Valor.Frecuencia()));
            }


            string texto = "";
            foreach (var DatoB in almacenar)
            {
                texto += Diccionario[DatoB].rut;
            }
            while (texto.Length % 8 != 0)
            {
                texto += "0";
            }

            string aux = texto;
            List<string> stringB = new List<string>();
            while (aux.Length != 0)
            {
                string temp = aux.Substring(0, 8);
                stringB.Add(temp);
                aux = aux.Remove(0, 8);
            }

            texto = "";
            byte[] bytes = new byte[1];
            foreach (var DatoB in stringB)
            {
                bytes[0] = Convert.ToByte(DatoB, 2);
                texto += ConvertirByte.ConvertirP(bytes);
            }
            string Comprimido = cantidad + texto;
            return Comprimido;
        }

        private void Arbol()
        {
            ColaP = new ColaP<HuffmanNode<T>>();
            foreach (var Node in Diccionario.Values)
            {
                Node.Valor.CalculoP(cont);
                ColaP.Agregar(Node, Node.Valor.Probabilidad());
            }

            T NewNodeValue = new T();
            while (ColaP.Dato != 1)
            {
                var nodo = ColaP.ObtenerP();
                var nodo1 = ColaP.ObtenerP();
                NewNodeValue.MandarP(nodo.Valor.Probabilidad() + nodo1.Valor.Probabilidad());
                var NewNode = new HuffmanNode<T>(NewNodeValue);
                nodo.Padre = NewNode;
                nodo1.Padre = NewNode;
                if (nodo.Valor.Probabilidad() < nodo1.Valor.Probabilidad())
                {
                    NewNode.hijoi = nodo1;
                    NewNode.hijod = nodo;
                }
                else
                {
                    NewNode.hijod = nodo1;
                    NewNode.hijoi = nodo;
                }
                ColaP.Agregar(NewNode, NewNode.Valor.Probabilidad());
                Ruta = NewNode;
                NewNodeValue = new T();
            }
        }

        private void CeroUno(HuffmanNode<T> nodo, string ruta)
        {
            if (nodo.Padre != null)
            {
                if (nodo.Padre.hijoi == nodo)
                {
                    nodo.rut = $"{ruta}0";
                }
                else
                {
                    nodo.rut = $"{ruta}1";
                }
                if (Diccionario.ContainsKey(nodo.Valor.Valor()))
                {
                    Diccionario[nodo.Valor.Valor()].rut = nodo.rut;
                }
            }
            else
            {
                nodo.rut = "";
            }

            if (nodo.hijoi != null)
            {
                CeroUno(nodo.hijoi, nodo.rut);
            }
            if (nodo.hijod != null)
            {
                CeroUno(nodo.hijod, nodo.rut);
            }
        }

        private byte[] ConvertirI(int num)
        {
            string texto = ByteCompleto(num);
            byte[] enviar = new byte[1];
            enviar[0] = Convert.ToByte(texto, 2);
            return enviar;
        }

        private string ByteCompleto(int num)
        {
            string bits = Convert.ToString(num, 2);
            while (bits.Length % 8 != 0 || bits.Length / 8 != 1)
            {
                bits = "0" + bits;
            }
            return bits;
        }

        public string DescomprimirT(string texto)
        {
            var almacenar = new byte[5000];
            Diccionario = new Dictionary<byte, HuffmanNode<T>>();
            almacenar = ConvertirByte.Convertir(texto);
            T valor = new T();
            HuffmanNode<T> Nodo;
            cont = 0.00;
            int cantidad = almacenar[0];

            int contar = 0;
            while (contar < cantidad)
            {
                almacenar = ConvertirByte.Convertir(texto.Substring(1 + (contar * 2), 2));
                valor.Byte(almacenar[0]);
                valor.AgregarFrecuencia(ConvertirB(almacenar));
                cont += valor.Frecuencia();
                Nodo = new HuffmanNode<T>(valor);
                Diccionario.Add(almacenar[0], Nodo);
                valor = new T();
                contar++;
            }
            Arbol();
            CeroUno(Ruta, "");

            var text = "";
            almacenar = ConvertirByte.Convertir(texto.Substring(1 + (cantidad * 2)));
            text += ConvertirBinario(almacenar);

            var UnoCero = "";
            var Descomprimido = "";
            int Contador = 0;
            byte[] bytes = new byte[1];
            bool flag = true;
            while (Descomprimido.Length != cont)
            {
                while (flag)
                {
                    foreach (var elchar in Diccionario)
                    {
                        if (elchar.Value.rut == UnoCero)
                        {
                            bytes[0] = elchar.Key;
                            Descomprimido += ConvertirByte.ConvertirP(bytes);
                            flag = false;
                        }
                    }
                    if (flag)
                    {
                        Contador++;
                        UnoCero = text.Substring(0, Contador);
                    }
                }
                flag = true;
                UnoCero = "";
                text = text.Remove(0, Contador);
                Contador = 0;
            }
            return Descomprimido;
        }

        private int ConvertirB(byte[] texto)
        {
            var enviar = "";
            for (int i = 1; i < texto.Length; i++)
            {
                enviar += Convert.ToString(texto[i], 2);
            }
            return Convert.ToInt32(enviar, 2);
        }

        private string ConvertirBinario(byte[] texto)
        {
            var enviar = "";
            foreach (var dato in texto)
            {
                enviar += ByteCompleto(dato);
            }
            return enviar;
        }

        public async Task ComprimirF(string ruta, IFormFile file, string nombre)
        {           
            using var text = new FileStream($"{FilePath}/{file.FileName}", FileMode.OpenOrCreate);
            await file.CopyToAsync(text);
            using var reader = new BinaryReader(text);
            var almacenar = new byte[5000];
            Diccionario = new Dictionary<byte, HuffmanNode<T>>();
            text.Position = text.Seek(0, SeekOrigin.Begin);
            cont = 0.00;

            while (text.Position != text.Length)
            {
                almacenar = reader.ReadBytes(5000);
                T valor = new T();
                HuffmanNode<T> Nodo;
                foreach (var Dato in almacenar)
                {
                    if (!Diccionario.ContainsKey(Dato))
                    {
                        valor.Byte(Dato);
                        Nodo = new HuffmanNode<T>(valor);
                        Diccionario.Add(Dato, Nodo);
                    }
                    Diccionario[Dato].Valor.AgregarFrecuencia(1);
                    cont++;
                    valor = new T();
                }
            }
            Arbol();
            CeroUno(Ruta, "");

            var Comprimido = new FileStream($"{FilePath}/{nombre}", FileMode.OpenOrCreate);
            var escribir = new StreamWriter(Comprimido);
            string cantidad = "";
            cantidad += ConvertirByte.ConvertirP(ConvertirI(Diccionario.Values.Count));
            escribir.Write(cantidad);
            foreach (var byteObject in Diccionario.Values)
            {
                cantidad = ConvertirByte.ConvertirP(new byte[] { byteObject.Valor.Valor() }) + ConvertirByte.ConvertirP(ConvertirI(byteObject.Valor.Frecuencia()));
                escribir.Write(cantidad);
            }
            text.Position = text.Seek(0, SeekOrigin.Begin);
            string texto = "";

            while (text.Position != text.Length)
            {
                almacenar = reader.ReadBytes(5000);
                foreach (var DatoB in almacenar)
                {
                    texto += Diccionario[DatoB].rut;
                    if (texto.Length >= 8)
                    {
                        escribir.Write((char)Convert.ToByte(texto.Substring(0, 8), 2));
                        texto = texto.Remove(0, 8);
                    }
                }                
            }
            if (texto.Length != 0)
            {
                while (texto.Length % 8 != 0)
                {
                    texto += "0";
                }
                escribir.Write((char)Convert.ToByte(texto, 2));
            }
            text.Close();
            escribir.Close();
            Comprimido.Close();
        }

        public async Task DescomprimirF(IFormFile file, string nombre)
        {
            using var almacen = new FileStream($"{FilePath}/{file.FileName}", FileMode.OpenOrCreate);
            await file.CopyToAsync(almacen);
            using var reader = new StreamReader(almacen);
            almacen.Position = almacen.Seek(0, SeekOrigin.Begin);
            var lector = reader.ReadToEnd();

            var almacenar = new byte[5000];
            Diccionario = new Dictionary<byte, HuffmanNode<T>>();
            almacenar = ConvertirByte.Convertir(lector);
            T valor = new T();
            HuffmanNode<T> Nodo;
            cont = 0.00;
            int cantidad = almacenar[0];

            int contar = 0;
            while (contar < cantidad)
            {
                almacenar = ConvertirByte.Convertir(lector.Substring(1 + (contar * 2), 2));
                valor.Byte(almacenar[0]);
                valor.AgregarFrecuencia(ConvertirB(almacenar));
                cont += valor.Frecuencia();
                Nodo = new HuffmanNode<T>(valor);
                Diccionario.Add(almacenar[0], Nodo);
                valor = new T();
                contar++;
            }
            almacen.Close();
            Arbol();
            CeroUno(Ruta, "");

            var text = "";
            almacenar = ConvertirByte.Convertir(lector.Substring(1 + (cantidad * 2)));
            text += ConvertirBinario(almacenar);

            var UnoCero = "";
            var Descomprimido = "";
            int Contador = 0;
            byte[] bytes = new byte[1];
            bool flag = true;
            var nuevo = new FileStream($"{FilePath}/{nombre}", FileMode.OpenOrCreate);
            var escribir = new StreamWriter(nuevo);

            while (Descomprimido.Length != cont)
            {
                while (flag)
                {
                    foreach (var elchar in Diccionario)
                    {
                        if (elchar.Value.rut == UnoCero)
                        {
                            bytes[0] = elchar.Key;
                            Descomprimido += ConvertirByte.ConvertirP(bytes);
                            flag = false;
                        }
                    }
                    if (flag)
                    {
                        Contador++;
                        UnoCero = text.Substring(0, Contador);
                    }
                }
                flag = true;
                UnoCero = "";
                text = text.Remove(0, Contador);
                Contador = 0;
            }
            escribir.Write(Descomprimido);
            escribir.Close();
            nuevo.Close();
        }

    }
}
