using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PracticaCore2Iniciales.Filters;
using PracticaCore2Iniciales.Models;
using PracticaCore2Iniciales.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PracticaCore2Iniciales.Controllers
{
    public class ManageController : Controller
    {
        private RepositoryLibros repo;

        public ManageController(RepositoryLibros repo) 
        {
            this.repo = repo;
        }
        public IActionResult Index()
        {
            return View();
        }

        /*vistas de LOGIN*/
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            Usuario usuario = this.repo.LoginUsuario(email, password);
            if (usuario != null)
            {
                ClaimsIdentity identity =
                   new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme,
                   ClaimTypes.Name, ClaimTypes.Role);
                Claim claimName = new Claim(ClaimTypes.Name, usuario.Nombre);
                identity.AddClaim(claimName);
                /*guardamos el id del usuario*/
                Claim claimId = new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString());
                /*agregamos los claim al usuario*/
                identity.AddClaim(claimId);
                Claim claimImg = new Claim("IMAGEN",usuario.Foto);
                identity.AddClaim(claimImg);
                Claim claimEmail = new Claim("EMAIL", usuario.Email);
                identity.AddClaim(claimEmail);
                Claim claimApellidos = new Claim("APELLIDO", usuario.Apellidos);
                identity.AddClaim(claimApellidos);
                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);
                //string controller = TempData["controller"].ToString();
                //string action = TempData["action"].ToString();
                //return RedirectToAction(action, controller);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["MENSAJE"] = "Usuario/Password incorrectos";
            }
            return View();
        }
        public IActionResult ErrorAcceso()
        {
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [AuthorizeUsuarios]
        public IActionResult PerfilUsuario(int idusuario) 
        {
            Usuario usuario = this.repo.FindUsuario(idusuario);
            return View(usuario);
        }

        public IActionResult AccesoDenegado() 
        {
            return View();
        }
    }
}
