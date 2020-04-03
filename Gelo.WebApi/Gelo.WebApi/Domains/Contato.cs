using Gelo.WebApi.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gelo.WebApi.Domains
{
    public class Contato : AbstractDomain
    {
        public string Telefone { get; set; }
        public string NomesDonos { get; set; }
        public long ClienteId { get; set; }

        public Contato()
        {

        }

        public Contato(string telefone, long clienteId)
        {
            Telefone = telefone;
            ClienteId = clienteId;
        }

        public Contato(string telefone, string nomesDonos, long clienteId)
        {
            Telefone = telefone;
            NomesDonos = nomesDonos;
            ClienteId = clienteId;
        }
    }
}
