using Microsoft.AspNetCore.Mvc;
using PracticaCore2Iniciales.Models;
using PracticaCore2Iniciales.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticaCore2Iniciales.ViewComponents
{
    public class MenuLibroViewComponent:ViewComponent
    {
        private RepositoryLibros repo;
        public MenuLibroViewComponent(RepositoryLibros repo)
        {
            this.repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Genero> libros = this.repo.GetGeneros();
            return View(libros);
        }
    }
}
