using Gelo.WebApi.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gelo.WebApi.Domains
{
    public class NotaFiscalCondicao : AbstractDomain
    {
        public string Descricao { get; set; }
        public bool EmitirNF { get; set; }
        public long ClienteId { get; set; }

        public NotaFiscalCondicao()
        {

        }

        public NotaFiscalCondicao(string descricao, bool emitirNF, long clienteId)
        {
            Descricao = descricao;
            EmitirNF = emitirNF;
            ClienteId = clienteId;
        }

        public NotaFiscalCondicao(bool emitirNF, long clienteId)
        {
            EmitirNF = emitirNF;
            ClienteId = clienteId;
        }
    }
}
