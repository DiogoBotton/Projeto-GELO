using Gelo.WebApi.Domains;
using Gelo.WebApi.Infraestructure.Contexts;
using Gelo.WebApi.Interfaces;
using Gelo.WebApi.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gelo.WebApi.Infraestructure.Repositories
{
    public class TipoUsuarioRepository : ITipoUsuarioRepository
    {
        public GeloContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public TipoUsuarioRepository(GeloContext context)
        {
            _context = context;
        }

        public TipoUsuario Create(TipoUsuario objeto)
        {
            return _context.TiposUsuarios.Add(objeto).Entity;
        }

        public TipoUsuario FindByDescricao(string titulo)
        {
            return _context.TiposUsuarios.FirstOrDefault(x => x.Titulo == titulo);
        }
    }
}
