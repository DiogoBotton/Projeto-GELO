using Gelo.WebApi.Helpers.Implementations;
using Gelo.WebApi.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gelo.WebApi.Helpers
{
    public static class ConfigureJobs
    {
        public static async void ConfigureJobsAsync(this IApplicationBuilder app)
        {
            List<Type> jobs = new List<Type>()
            {
                //typeof's com classes JOBS dentro

                //Padrões para o sistema funcionar
                typeof(TipoUsuarioJobs),
                typeof(UsuarioJobs),
                //TODO: AreaSaudeEspecialidade
            };

            using(var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                foreach (var item in jobs)
                {
                    var job = (IJobs)ActivatorUtilities.CreateInstance(scope.ServiceProvider, item);

                    await job.ExecuteAsync();
                }
            }
        }
    }
}
