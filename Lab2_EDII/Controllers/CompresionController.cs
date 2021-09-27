using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibCompress;
using Lab2_EDII.Models;
using Lab2_EDII.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Lab2_EDII.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CompresionController : ControllerBase
    {
        private IWebHostEnvironment Environment;
        public CompresionController(IWebHostEnvironment env)
        {
            Environment = env;
        }

        [HttpGet]
        public string Get()
        {
            return "Lab2, Huffman";
        }

        [HttpPost]
        [Route("huffman/compress/{name}")]
        public async Task<IActionResult> Post([FromForm] IFormFile file, string name)
        {
            Storage.Instance.Arbol = new Huffman<HuffmanM>($"{Environment.ContentRootPath}");
            string nombre = name + ".huff";
            await Storage.Instance.Arbol.ComprimirF(Environment.ContentRootPath, file, nombre);
            var DatosF = new Datos();
            DatosF.AgregarDatos(Environment.ContentRootPath, file.FileName, nombre);
            Storage.Instance.Datos.Add(DatosF);

            return PhysicalFile($"{Environment.ContentRootPath}/{name}", MediaTypeNames.Text.Plain, $"{nombre}");                      
            
        }

        [HttpPost]
        [Route("lzw/compress/{name}")]
        public async Task<IActionResult> Postl([FromForm] IFormFile file, string name)
        {
            Storage.Instance.Tabla = new LZW<HuffmanM>($"{Environment.ContentRootPath}");
            string nombre = name + ".lzw";
            await Storage.Instance.Tabla.ComprimirF(Environment.ContentRootPath, file, nombre);
            var DatosF = new Datos();
            DatosF.AgregarDatos(Environment.ContentRootPath, file.FileName, nombre);
            Storage.Instance.Datos.Add(DatosF);

            return PhysicalFile($"{Environment.ContentRootPath}/{name}", MediaTypeNames.Text.Plain, $"{nombre}");
        }

        [HttpPost]
        [Route("huffman/decompress")]
        public async Task<IActionResult> PostD([FromForm] IFormFile file)
        {
            Storage.Instance.Arbol = new Huffman<HuffmanM>($"{Environment.ContentRootPath}");
            Datos.Dato(Environment.ContentRootPath);
            var nombre = "";
            foreach (var item in Storage.Instance.Datos)
            {
                if (item.Ncomprimido == file.FileName)
                {
                    nombre = item.NOriginal;
                }
            }
            await Storage.Instance.Arbol.DescomprimirF(file, nombre);
            return PhysicalFile($"{Environment.ContentRootPath}/{nombre}", MediaTypeNames.Text.Plain, ".txt");
        }

        [HttpPost]
        [Route("lzw/decompress")]
        public async Task<IActionResult> PostDl([FromForm] IFormFile file)
        {
            Storage.Instance.Tabla = new LZW<HuffmanM>($"{Environment.ContentRootPath}");
            Datos.Dato(Environment.ContentRootPath);
            var nombre = "";
            foreach (var item in Storage.Instance.Datos)
            {
                if (item.Ncomprimido == file.FileName)
                {
                    nombre = item.NOriginal;
                }
            }
            await Storage.Instance.Tabla.DescomprimirF(file, nombre);
            return PhysicalFile($"{Environment.ContentRootPath}/{nombre}", MediaTypeNames.Text.Plain, ".txt");
        }

        [HttpGet]
        [Route("compressions")]
        public List<Datos> GetListCompress()
        {
            Datos.Dato(Environment.ContentRootPath);
            return Storage.Instance.Datos;
        }
    }
}
