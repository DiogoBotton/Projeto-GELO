using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gelo.WebApi.ViewModels
{
    public class ClientesImportacaoInputViewModel
    {
        [Required]
        public IFormFile Arquivo { get; set; }

        public ClientesImportacaoInputViewModel()
        {

        }
    }
}
