using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticaCore2Iniciales.Extensions;
using PracticaCore2Iniciales.Models;
using PracticaCore2Iniciales.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticaCore2Iniciales.Controllers
{
    public class LibrosController : Controller
    {
        private RepositoryLibros repo;
        public LibrosController(RepositoryLibros repo)
        {
            this.repo = repo;
        }
        public IActionResult Index()
        {
            List<Libro> libros = this.repo.GetAllLibros();
            return View(libros);
        }
        public IActionResult LibrosGenero(int idlibro) 
        {
            List<Libro> libros = this.repo.FindLibrosIdGenero(idlibro);
            return View(libros);
        }

        public IActionResult Details(int idlibro) 
        {
            Libro libro = this.repo.FindLibro(idlibro);
            return View(libro);
        }
        [HttpPost]
        public IActionResult Details(string libro) 
        {
            if (HttpContext.Session.GetString("USUARIO") == null)
            {
                return RedirectToAction("AccesoDenegado", "Managed");
            }
            //NECESITAMOS ENVIAR INFORMACION DESDE
            //PRODUCTOS HASTA CARRITO
            TempData["LIBRO"] = libro;
            return View("");
        }
        /**************************************/
        public IActionResult SessionLibrosCorrecto(int? idlibro)
        {
            if (idlibro != null)
            {
                List<int> listIdLibros;
                if (HttpContext.Session.GetString("IDSLIBROS") == null)
                {
                    listIdLibros = new List<int>();
                }
                else
                {
                    //EXISTE Y RECUPERAMOS LA COLECCION DE SESSION
                    listIdLibros = HttpContext.Session.GetObject<List<int>>("IDSLIBROS");
                }
                //ALMACENAMOS EL ID DENTRO DE LA COLECCION
                listIdLibros.Add(idlibro.Value);
                //ALMACENAMOS LA COLLECCION DE NUEVO EN SESSION
                HttpContext.Session.SetObject("IDSLIBROS", listIdLibros);
                ViewData["MENSAJE"] = "Libros: " + listIdLibros.Count;
            }
            Libro libro = this.repo.FindLibro(idlibro.Value);
            return View();
        }
        /**************************************/
        public IActionResult LibrosAlmacenados(int? ideliminar)
        {
            List<int> listLibros =
                    HttpContext.Session.GetObject<List<int>>("IDSLIBROS");
            //preguntamos si esta creado la session con idsusuarios
            if (listLibros == null)
            {
                return View();
            }
            else
            {
                /*para el boton eliminar*/
                if (ideliminar != null)
                {
                    listLibros.Remove(ideliminar.Value);
                    if (listLibros.Count == 0)
                    {
                        HttpContext.Session.Remove("IDSLIBROS");
                    }
                    else
                    {
                        HttpContext.Session.SetObject("IDSLIBROS", listLibros);
                    }
                }
                List<Libro> libros =
                    this.repo.GetLibrosSession(listLibros);
                return View(libros);
            }
        }
        [HttpPost]
        public IActionResult LibrosAlmacenados(List<int> cantidades)
        {
            //List<int> cantidadEmpleados = cantidad;
            //HttpContext.Session.SetObject("CANTIDAD", cantidadEmpleados);
            TempData.Put("LIBROS", cantidades);
            return RedirectToAction("DetalleFactura");
        }

        public IActionResult DetalleFactura()
        {
            List<int> idlibros = HttpContext.Session.GetObject<List<int>>("IDSLIBROS");
            List<Libro> libros = new List<Libro>();
            foreach (int i in idlibros)
            {
                Libro libro = this.repo.FindLibro(i);
                libros.Add(libro);
            }

            ViewData["LIBROS"] = libros;
            var cants = TempData.Get<List<int>>("LIBROS");
            return View(cants);
        }
    }
}
