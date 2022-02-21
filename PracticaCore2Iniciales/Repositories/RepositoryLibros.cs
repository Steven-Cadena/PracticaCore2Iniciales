using PracticaCore2Iniciales.Data;
using PracticaCore2Iniciales.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticaCore2Iniciales.Repositories
{
    public class RepositoryLibros
    {
        private LibrosContext context;
        public RepositoryLibros(LibrosContext context)
        {
            this.context = context;
        }

        //metodo para obtener todos los generos 
        public List<Genero> GetGeneros() 
        {
            var consulta = from datos in this.context.Generos
                           select datos;
            return consulta.ToList();
        }
        //metodo para obtener todos los libros 
        public List<Libro> GetAllLibros() 
        {
            var consulta = from datos in this.context.Libros
                           select datos;
            return consulta.ToList();
        }

        //metodo para buscar el libro 
        public Libro FindLibro(int id) 
        {
            var consulta = from datos in this.context.Libros
                           where datos.IdLibro == id
                           select datos;
            return consulta.FirstOrDefault();
        }

        //metodo para buscar los libros por genero
        public List<Libro> FindLibrosIdGenero(int id)
        {
            var consulta = from datos in this.context.Libros
                           where datos.IdGenero == id
                           select datos;
            return consulta.ToList();

        }

        //metodo para validar el login 
        public Usuario LoginUsuario(string email, string password)
        {
            //para buscar el usuario por su email
            Usuario usuario = this.context.Usuarios.SingleOrDefault(x => x.Email == email);
            if (usuario == null)
            {
                return null;
            }
            else
            {
                if (usuario.Email == email && usuario.Pass == password)
                {
                    return usuario;
                }
                else 
                {
                    return null;
                }
            }
        }

        //metodo para buscar le usuario 
        public Usuario FindUsuario(int idusuario) 
        {
            var consulta = from datos in this.context.Usuarios
                          where datos.IdUsuario == idusuario
                          select datos;
            return consulta.FirstOrDefault();
        }

        //metodo para la session
        public List<Libro> GetLibrosSession(List<int> idsLibros)
        {
            //CUANDO UTILIZAMOS BUSQUEDA EN COLECCIONES SE UTILIZA EL METODO Contains
            //contains muy importante, para comprobar si están en la bbdd esos id
            var consulta = from datos in this.context.Libros
                           where idsLibros.Contains(datos.IdLibro)
                           select datos;
            /*importante devolver null si no hay libros en la session*/
            if (consulta.Count() == 0)
            {
                return null;
            }
            return consulta.ToList();
        }



    }
}
