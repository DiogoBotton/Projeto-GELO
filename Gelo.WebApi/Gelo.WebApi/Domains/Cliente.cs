using Gelo.WebApi.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gelo.WebApi.Domains
{
    public class Cliente : AbstractDomain
    {
        public string NomeRazaoSocial { get; set; }
        public uint CapacidadeFreezerEmPacotes { get; set; }

        public Cliente(string nomeRazaoSocial, uint capacidadeFreezerEmPacotes)
        {
            NomeRazaoSocial = nomeRazaoSocial;
            CapacidadeFreezerEmPacotes = capacidadeFreezerEmPacotes;
        }

        public Cliente()
        {

        }
    }
}
