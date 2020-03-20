using Gelo.WebApi.Domains;
using Gelo.WebApi.Infraestructure.Contexts;
using Gelo.WebApi.Interfaces;
using Gelo.WebApi.SeedWork;
using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gelo.WebApi.Infraestructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        public GeloContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public UsuarioRepository(GeloContext context)
        {
            _context = context;
        }

        public Usuario Create(Usuario objeto)
        {
            return _context.Usuarios.Add(objeto).Entity;
        }

        public Usuario FindByEmail(string email)
        {
            return _context.Usuarios.FirstOrDefault(x => x.Email == email);
        }
    }
}
