using Gelo.WebApi.Domains;
using Gelo.WebApi.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gelo.WebApi.Interfaces
{
    public interface ITipoUsuarioRepository : IRepository<TipoUsuario>
    {
        TipoUsuario FindByDescricao(string titulo);
    }
}
