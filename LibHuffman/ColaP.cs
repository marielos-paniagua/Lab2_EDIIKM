using System;
using System.Collections.Generic;
using System.Text;

namespace LibCompress
{
    public class ColaP<T>
    {
        NodoCP<T> Ruta;
        public int Dato;

        public ColaP()
        {
            Dato = 0;
        }

        private bool IsEmpty() { return Ruta == null; }

        public void Agregar(T valor, double prioridad)
        {
            if (IsEmpty())
            {
                Ruta = new NodoCP<T>(valor, prioridad);
                Dato = 1;
            }
            else
            {
                Dato++;
                Agrega(new NodoCP<T>(valor, prioridad), prioridad);
            }
        }

        private void Agrega(NodoCP<T> Nuevo, double priority)
        {
            var NuevoPadre = BuscaUltimo(Ruta, 1);
            if (NuevoPadre.HijoI != null)
            {
                NuevoPadre.HijoD = Nuevo;
                Nuevo.Padre = NuevoPadre;
                OrdenArriba(Nuevo);
            }
            else
            {
                NuevoPadre.HijoI = Nuevo;
                Nuevo.Padre = NuevoPadre;
                OrdenArriba(Nuevo);
            }
        }

        private NodoCP<T> BuscaUltimo(NodoCP<T> Actual, int Num)
        {
            try
            {
                int anterior = Dato;
                if (anterior == Num)
                {
                    return Actual;
                }
                else
                {
                    while (anterior / 2 != Num)
                    {
                        anterior = anterior / 2;
                    }
                    if (anterior % 2 == 0)
                    {
                        if (Actual.HijoI != null)
                        {
                            return BuscaUltimo(Actual.HijoI, anterior);
                        }
                        else
                        {
                            return Actual;
                        }
                    }
                    else
                    {
                        if (Actual.HijoD != null)
                        {
                            return BuscaUltimo(Actual.HijoD, anterior);
                        }
                        else
                        {
                            return Actual;
                        }
                    }
                }
            }
            catch
            {
                return Actual;
            }
        }
        public T ObtenerP()
        {
            if (Ruta == null)
            {
                return default;
            }
            NodoCP<T> Ultimo = new NodoCP<T>();
            Ultimo = BuscaUltimo(Ruta, 1);
            NodoCP<T> Primero = (NodoCP<T>)Ruta.Clone();
            var Copiado = (NodoCP<T>)Ultimo.Clone();
            Ruta.Valor = Copiado.Valor;
            Ruta.Prioridad = Copiado.Prioridad;
            if (Ultimo.Padre == null)
            {
                Ruta = null;
                Dato--;
                return Copiado.Valor;
            }
            else
            {
                if (Ultimo.Padre.HijoI == Ultimo)
                {
                    Ultimo.Padre.HijoI = null;
                }
                else
                {
                    Ultimo.Padre.HijoD = null;
                }
            }
            OrdenAbajo(Ruta);
            Dato--;
            return Primero.Valor;
        }
        private void OrdenArriba(NodoCP<T> Actual)
        {
            if (Actual.Padre != null)
            {
                if (Actual.Prioridad < Actual.Padre.Prioridad)
                {
                    Cambiar(Actual);
                }
                OrdenArriba(Actual.Padre);
            }
        }

        private void OrdenAbajo(NodoCP<T> Actual)
        {
            if (Actual.HijoI != null && Actual.HijoD != null)
            {
                if (Actual.HijoI.Prioridad > Actual.HijoD.Prioridad)
                {
                    if (Actual.Prioridad > Actual.HijoD.Prioridad)
                    {
                        Cambiar(Actual.HijoD);
                        OrdenAbajo(Actual.HijoD);
                    }
                }
                else
                {
                    if (Actual.Prioridad > Actual.HijoI.Prioridad)
                    {
                        Cambiar(Actual.HijoI);
                        OrdenAbajo(Actual.HijoI);
                    }
                }
            }
            else if (Actual.HijoI != null)
            {
                if (Actual.Prioridad > Actual.HijoI.Prioridad)
                {
                    Cambiar(Actual.HijoI);
                    OrdenAbajo(Actual.HijoI);
                }
            }
        }

        private void Cambiar(NodoCP<T> Actual)
        {
            var Prioridad1 = Actual.Prioridad;
            var Valor1 = Actual.Valor;
            Actual.Prioridad = Actual.Padre.Prioridad;
            Actual.Valor = Actual.Padre.Valor;
            Actual.Padre.Prioridad = Prioridad1;
            Actual.Padre.Valor = Valor1;
        }
    }   
    
}
