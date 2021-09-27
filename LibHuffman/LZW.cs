using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace LibCompress
{
    public class LZW<T> : ICompress where T : IDatos, new()
    {
        Dictionary<string, int> diccionario = new Dictionary<string, int>();
        Dictionary<int, List<byte>> Ddiccionario = new Dictionary<int, List<byte>>(); 
        int cont = 1;
        string FilePath;

        public LZW() { }
        public LZW(string filePath)
        {
            FilePath = filePath;
        }

        public string Comprimir(string texto)
        {
            var almacenar = ConvertirByte.Convertir(texto);
            List<byte> letras = new List<byte>();
            Inicio(almacenar, letras);
            List<byte> almacen = almacenar.ToList();
            List<int> tabla = new List<int>();
            while (almacen.Count != 0)
            {
                string aux = almacen[0].ToString();
                while (aux.Length != 0)
                {
                    if (almacen.Count > 1)
                    {
                        string cadena = aux + almacen[1].ToString();
                        if (!diccionario.ContainsKey(cadena))
                        {
                            tabla.Add(diccionario[aux]);
                            Agregar(cadena);
                            aux = "";
                            almacen.RemoveAt(0);
                        }
                        else
                        {
                            aux += almacen[1].ToString();
                            almacen.RemoveAt(0);
                        }
                    }
                    else
                    {
                        tabla.Add(diccionario[aux]);
                        Agregar(aux);
                        almacen.RemoveAt(0);
                        aux = "";
                    }
                }
            }

            int bytes = Convert.ToString(tabla.Max(), 2).Length;
            List<byte> comprimido = new List<byte>();
            comprimido.Add(Convert.ToByte(bytes));
            comprimido.Add(Convert.ToByte(letras.Count()));            
            foreach (var item in letras)
            {
                comprimido.Add(item);
            }

            string ceroUno = "";
            foreach (var item in tabla)
            {
                string aux = Convert.ToString(item, 2);
                while (aux.Length != bytes)
                {
                    aux = "0" + aux;
                }
                ceroUno += aux;
            }
            while (ceroUno.Length % 8 != 0)
            {
                ceroUno += "0";
            }
            while (ceroUno.Length != 0)
            {
                comprimido.Add(Convert.ToByte(ceroUno.Substring(0, 8), 2));
                ceroUno = ceroUno.Remove(0, 8);
            }

            return ConvertirByte.ConvertirP(comprimido.ToArray());
        }

        public void Inicio(byte[] texto, List<byte> letras)
        {
            foreach (var item in texto)
            {
                if (!diccionario.ContainsKey(item.ToString()))
                {
                    diccionario.Add(item.ToString(), cont);
                    cont++;
                    letras.Add(item);
                }
            }
        }

        public void Agregar(string cadena)
        {
            if (!diccionario.ContainsKey(cadena))
            {
                diccionario.Add(cadena, cont);
                cont++;
            }
        }

        public string DescomprimirT(string texto)
        {
            var almacenar = ConvertirByte.Convertir(texto);
            int bytes = almacenar[0];
            cont = 1;
            int iniciales = almacenar[1];
            for (int i = 0; i < iniciales; i++)
            {
                Ddiccionario.Add(cont, new List<byte> { almacenar[i+2]});
                cont++;
            }
            var comprimido = new byte[almacenar.Length - (2 + iniciales)];
            for (int i = 0; i < comprimido.Length; i++)
            {
                comprimido[i] = almacenar[i + 2 + iniciales];
            }

            List<int> convert = new List<int>();
            List<byte> previo = new List<byte>();
            List<byte> actual = new List<byte>();
            List<byte> nuevo = new List<byte>();
            string ceroUno = "";
            foreach (var item in comprimido)
            {
                string aux = Convert.ToString(item, 2);
                while (aux.Length < 8)
                {
                    aux = "0" + aux;
                }
                ceroUno += aux;
            }
            while (ceroUno.Length >= bytes)
            {
                var aux = Convert.ToInt32(ceroUno.Substring(0, bytes), 2);
                ceroUno = ceroUno.Remove(0, bytes);
                if (aux != 0)
                {
                    convert.Add(aux);
                    previo.Clear();
                    foreach (var item in actual)
                    {
                        previo.Add(item);
                    }
                    if (Ddiccionario.ContainsKey(aux))
                    {
                        actual.Clear();
                        foreach (var item in Ddiccionario[aux])
                        {
                            actual.Add(item);
                        }
                        nuevo.Clear();
                        foreach (var item in previo)
                        {
                            nuevo.Add(item);
                        }
                        nuevo.Add(actual[0]);
                    }
                    else
                    {
                        actual.Clear();
                        foreach (var item in previo)
                        {
                            actual.Add(item);
                        }
                        nuevo.Clear();
                        foreach (var item in previo)
                        {
                            nuevo.Add(item);
                        }
                        nuevo.Add(actual[0]);
                        foreach (var item in nuevo)
                        {
                            actual.Add(item);
                        }
                    }
                    if (!Encontrar(nuevo))
                    {
                        Ddiccionario.Add(cont, new List<byte>(nuevo));
                        cont++;
                    }
                }
            }

            var descomprimir = new List<byte>();
            foreach (var item in convert)
            {
                foreach (var item1 in Ddiccionario[item])
                {
                    descomprimir.Add(item1);
                }
            }

            return ConvertirByte.ConvertirP(descomprimir.ToArray());
        }

        public bool Encontrar(List<byte> encontrar)
        {
            foreach (var item in Ddiccionario.Values)
            {
                if (encontrar.Count == item.Count)
                {
                    if (comparar(encontrar, item))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool comparar(List<byte> item1, List<byte> item2)
        {
            for (int i = 0; i < item1.Count; i++)
            {
                if (item1[i] != item2[i])
                {
                    return false;
                }
            }
            return true;
        }

        public async Task ComprimirF(string ruta, IFormFile file, string nombre)
        {
            using var text = new FileStream($"{ruta}/{file.FileName}", FileMode.OpenOrCreate);
            await file.CopyToAsync(text);
            using var reader = new BinaryReader(text);
            var almacenar = new byte[5000];
            text.Position = text.Seek(0, SeekOrigin.Begin);

            List<byte> letras = new List<byte>();
            while (text.Position != text.Length)
            {
                almacenar = reader.ReadBytes(5000);
                Inicio(almacenar, letras);
            }

            int bytes;
            List<int> tabla = new List<int>();
            text.Position = text.Seek(0, SeekOrigin.Begin);
            while (text.Position != text.Length)
            {
                almacenar = reader.ReadBytes(5000);
                List<byte> almacen = almacenar.ToList();
                while (almacen.Count != 0)
                {
                    string aux = almacen[0].ToString();
                    while (aux.Length != 0)
                    {
                        if (almacen.Count > 1)
                        {
                            string cadena = aux + almacen[1].ToString();
                            if (!diccionario.ContainsKey(cadena))
                            {
                                tabla.Add(diccionario[aux]);
                                Agregar(cadena);
                                aux = "";
                                almacen.RemoveAt(0);
                            }
                            else
                            {
                                aux += almacen[1].ToString();
                                almacen.RemoveAt(0);
                            }
                        }
                        else
                        {
                            if (text.Position != text.Length)
                            {
                                almacenar = reader.ReadBytes(5000);
                                List<byte> aux1 = almacenar.ToList();
                                while (almacen.Count > 0)
                                {
                                    aux1.Insert(0, almacen[0]);
                                    almacen.RemoveAt(0);
                                }
                                almacen = aux1;
                                bytes = 0;
                            }
                            else
                            {
                                tabla.Add(diccionario[aux]);
                                Agregar(aux);
                                almacen.RemoveAt(0);
                                aux = "";
                            }                            
                        }
                    }
                }
            }
            reader.Close();
            text.Close();

            bytes = Convert.ToString(tabla.Max(), 2).Length;
            var comprimido = new FileStream($"{ruta}/{nombre}", FileMode.OpenOrCreate);
            var escribir = new StreamWriter(comprimido);
            string texto = "";
            escribir.Write(ConvertirByte.ConvertirP(new byte[] { Convert.ToByte(bytes) }));
            escribir.Write(ConvertirByte.ConvertirP(new byte[] { Convert.ToByte(letras.Count())}));
            foreach (var item in letras)
            {
                escribir.Write(ConvertirByte.ConvertirP(new byte[] { Convert.ToByte(item) }));
            }

            string ceroUno = "";
            foreach (var item in tabla)
            {
                texto = Convert.ToString(item, 2);
                while (texto.Length != bytes)
                {
                    texto = "0" + texto;
                }
                ceroUno += texto;
            }
            while (ceroUno.Length % 8 != 0)
            {
                ceroUno += "0";
            }
            while (ceroUno.Length != 0)
            {
                escribir.Write(ConvertirByte.ConvertirP(new byte[] { Convert.ToByte(ceroUno.Substring(0, 8), 2) }));
                ceroUno = ceroUno.Remove(0, 8);
            }

            escribir.Close();
            comprimido.Close();
        }
        public async Task DescomprimirF(IFormFile file, string nombre)
        {
            using var text = new FileStream($"{FilePath}/{file.FileName}", FileMode.OpenOrCreate);
            await file.CopyToAsync(text);
            //using var reader = new BinaryReader(text);
            using var reader = new StreamReader(text);
            var almacenar = new byte[5000];
            text.Position = text.Seek(0, SeekOrigin.Begin);
            var lector = reader.ReadToEnd();

            //almacenar = reader.ReadBytes(5000);
            almacenar = ConvertirByte.Convertir(lector);
            int bytes = almacenar[0];
            text.Position = 2 + almacenar[1];

            int iniciales = almacenar[1];
            for (int i = 0; i < iniciales; i++)
            {
                Ddiccionario.Add(cont, new List<byte> { almacenar[i + 2] });
                cont++;
            }
            var comprimido = new byte[almacenar.Length - (2 + iniciales)];
            for (int i = 0; i < comprimido.Length; i++)
            {
                comprimido[i] = almacenar[i + 2 + iniciales];
            }

            List<int> convert = new List<int>();
            List<byte> previo = new List<byte>();
            List<byte> actual = new List<byte>();
            List<byte> nuevo = new List<byte>();
            string ceroUno = "";

                //almacenar = reader.ReadBytes(5000);
                foreach (var item in comprimido)
                {
                    string aux = Convert.ToString(item, 2);
                    while (aux.Length < 8)
                    {
                        aux = "0" + aux;
                    }
                    ceroUno += aux;
                }
                while (ceroUno.Length >= bytes)
                {
                    var aux = Convert.ToInt32(ceroUno.Substring(0, bytes), 2);
                    ceroUno = ceroUno.Remove(0, bytes);
                    if (aux != 0)
                    {
                        convert.Add(aux);
                        previo.Clear();
                        foreach (var item in actual)
                        {
                            previo.Add(item);
                        }
                        if (Ddiccionario.ContainsKey(aux))
                        {
                            actual.Clear();
                            foreach (var item in Ddiccionario[aux])
                            {
                                actual.Add(item);
                            }
                            nuevo.Clear();
                            foreach (var item in previo)
                            {
                                nuevo.Add(item);
                            }
                            nuevo.Add(actual[0]);
                        }
                        else
                        {
                            actual.Clear();
                            foreach (var item in previo)
                            {
                                actual.Add(item);
                            }
                            nuevo.Clear();
                            foreach (var item in previo)
                            {
                                nuevo.Add(item);
                            }
                            nuevo.Add(actual[0]);
                            foreach (var item in nuevo)
                            {
                                actual.Add(item);
                            }
                        }
                        if (!Encontrar(nuevo))
                        {
                            Ddiccionario.Add(cont, new List<byte>(nuevo));
                            cont++;
                        }
                    }
                }
            
            reader.Close();
            text.Close();

            using var descomprimir = new FileStream($"{FilePath}/{nombre}", FileMode.OpenOrCreate);
            using var escribir = new BinaryWriter(descomprimir);
            foreach (var item in convert)
            {
                foreach (var item1 in Ddiccionario[item])
                {
                    escribir.Write(item1);
                }
            }
            escribir.Close();
            descomprimir.Close();
        }

    }
}
