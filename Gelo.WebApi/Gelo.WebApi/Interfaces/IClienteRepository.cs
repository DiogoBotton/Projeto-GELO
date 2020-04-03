using Gelo.WebApi.Domains;
using Gelo.WebApi.SeedWork;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gelo.WebApi.Interfaces
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        List<Cliente> ImportarClientesExcel(IFormFile file);
    }
}
