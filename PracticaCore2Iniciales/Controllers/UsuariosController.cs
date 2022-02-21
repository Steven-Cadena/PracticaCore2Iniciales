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
    public class UsuariosController : Controller
    {
        private RepositoryLibros repo;
        public UsuariosController(RepositoryLibros repo)
        {
            this.repo = repo;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
