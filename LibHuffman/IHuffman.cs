using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace LibHuffman
{
    interface IHuffman
    {        
        string Comprimir(string text);
        string DescomprimirT(string text);
        Task ComprimirF(string ruta, IFormFile file, string nombre);
        Task DescomprimirF(IFormFile file, string nombre);
    }
}
