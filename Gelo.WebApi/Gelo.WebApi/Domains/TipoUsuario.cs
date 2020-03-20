using Gelo.WebApi.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gelo.WebApi.Domains
{
    public class TipoUsuario : AbstractDomain
    {
        public string Titulo { get; set; }

        public TipoUsuario()
        {

        }
        public TipoUsuario(string titulo)
        {
            Titulo = titulo;
        }
    }
}
