using Gelo.WebApi.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gelo.WebApi.Domains
{
    public class Produto : AbstractDomain
    {
        public string NomeProduto { get; set; }

        public Produto()
        {

        }

        public Produto(string nomeProduto)
        {
            NomeProduto = nomeProduto;
        }
    }
}
