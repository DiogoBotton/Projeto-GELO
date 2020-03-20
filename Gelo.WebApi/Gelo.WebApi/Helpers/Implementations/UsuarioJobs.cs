using Gelo.WebApi.DefaultValues;
using Gelo.WebApi.Domains;
using Gelo.WebApi.Helpers;
using Gelo.WebApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gelo.WebApi.Implementations
{
    public class UsuarioJobs : IJobs
    {
        public readonly IUsuarioRepository _usuarioRepository;
        public readonly ITipoUsuarioRepository _tipoUsuarioRepository;

        public UsuarioJobs(IUsuarioRepository usuarioRepository, ITipoUsuarioRepository tipoUsuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
            _tipoUsuarioRepository = tipoUsuarioRepository;
        }

        public async Task ExecuteAsync()
        {
            //Cria primeiro registro ADMIN na tabela de usuários
            var usuarioAdmDb = _usuarioRepository.FindByEmail("admin@email.com");

            if(usuarioAdmDb == null)
            {
                var tipoUsuarioString = TipoUsuarioDefaultValuesAcess.GetValue(TipoUsuarioDefaultValues.Administrador);
                var tipoUsuarioDb = _tipoUsuarioRepository.FindByDescricao(tipoUsuarioString);

                var nome = "Admin Padrão";
                var email = "admin@email.com";
                var senha = "admin123";

                Usuario novoUsuarioAdm = new Usuario(nome, email, senha, tipoUsuarioDb.Id);

                var usuarioRetorno = _usuarioRepository.Create(novoUsuarioAdm);
                await _usuarioRepository.UnitOfWork.SaveDbChanges();
            }

        }
    }
}
