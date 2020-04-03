using Gelo.WebApi.Interfaces;
using Gelo.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gelo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        public readonly IClienteRepository _clienteRepository;

        public ClientesController(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        [HttpPost("importar-planilha/clientes")]
        public async Task<IActionResult> ImportarClientesCompletos([FromForm] ClientesImportacaoInputViewModel input)
        {
            try
            {
                var retorno = _clienteRepository.ImportarClientesExcel(input.Arquivo);

                await _clienteRepository.UnitOfWork.SaveDbChanges();

                return Ok(retorno);
            }
            catch (Exception ex)
            {

                return BadRequest("Houve um erro na importação da planilha.");
            }
        }
    }
}
