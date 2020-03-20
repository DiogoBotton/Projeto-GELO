using Gelo.WebApi.DefaultValues;
using Gelo.WebApi.Domains;
using Gelo.WebApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gelo.WebApi.Helpers.Implementations
{
    public class TipoUsuarioJobs : IJobs
    {
        public readonly ITipoUsuarioRepository _tipoUsuarioRepository;

        public TipoUsuarioJobs(ITipoUsuarioRepository tipoUsuarioRepository)
        {
            _tipoUsuarioRepository = tipoUsuarioRepository;
        }

        public async Task ExecuteAsync()
        {
            for (int i = 0; i < 2; i++)
            {
                var statusCasting = (TipoUsuarioDefaultValues)i;
                var status = TipoUsuarioDefaultValuesAcess.GetValue(statusCasting);

                var tipoUsuarioDb = _tipoUsuarioRepository.FindByDescricao(status);

                if (tipoUsuarioDb == null)
                {
                    tipoUsuarioDb = new TipoUsuario(status);

                    _tipoUsuarioRepository.Create(tipoUsuarioDb);
                    await _tipoUsuarioRepository.UnitOfWork.SaveDbChanges();
                }
            }
        }
    }
}
