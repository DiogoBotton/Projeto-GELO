using Gelo.WebApi.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gelo.WebApi.Domains
{
    public class ProdutoPorCliente : AbstractDomain
    {
        public decimal ValorNegociado { get; set; }
        public bool Atual { get; set; }
        public long ProdutoId { get; set; }
        public long ClienteId { get; set; }

        public ProdutoPorCliente()
        {

        }

        public ProdutoPorCliente(decimal valorNegociado, long produtoId, long clienteId)
        {
            ValorNegociado = valorNegociado;
            Atual = true;
            ProdutoId = produtoId;
            ClienteId = clienteId;
        }

        public void AlterarParaAntigo()
        {
            this.Atual = false;
        }

        public void AlterarValores(decimal valorNegociado, long produtoId, long clienteId)
        {
            ValorNegociado = valorNegociado;
            ProdutoId = produtoId;
            ClienteId = clienteId;
        }
    }
}
