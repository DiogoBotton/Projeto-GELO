﻿using Gelo.WebApi.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gelo.WebApi.Domains
{
    public class Endereco : AbstractDomain
    {
        public string Descricao { get; set; }
        public long ClienteId { get; set; }

        public Endereco()
        {

        }

        public Endereco(string descricao, long clienteId)
        {
            Descricao = descricao;
            ClienteId = clienteId;
        }
    }
}
