using Gelo.WebApi.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gelo.WebApi.Domains
{
    public class BoletoCondicao : AbstractDomain
    {
        public string Descricao { get; set; }
        public long ClienteId { get; set; }

        public BoletoCondicao()
        {

        }

        public BoletoCondicao(string descricao, long clienteId)
        {
            Descricao = descricao;
            ClienteId = clienteId;
        }
    }
}
