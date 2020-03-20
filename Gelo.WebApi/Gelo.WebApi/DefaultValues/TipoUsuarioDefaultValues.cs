using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gelo.WebApi.DefaultValues
{
    public class TipoUsuarioDefaultValuesAcess
    {
        public static string GetValue(TipoUsuarioDefaultValues tipoUsuarioDefaultValues)
        {
            string retorno = null;
            switch (tipoUsuarioDefaultValues)
            {
                case TipoUsuarioDefaultValues.Administrador:
                    retorno = "Administrador";
                    break;
                case TipoUsuarioDefaultValues.Comum:
                    retorno = "Comum";
                    break;
            }

            return retorno;
        }
    }
    public enum TipoUsuarioDefaultValues
    {
        Administrador,
        Comum,
    }
}
