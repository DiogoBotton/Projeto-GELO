using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gelo.WebApi.Helpers;
using Gelo.WebApi.Infraestructure.Contexts;
using Gelo.WebApi.Infraestructure.Repositories;
using Gelo.WebApi.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gelo.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //DependencyInjecions(DI) Vinculando a dependencia da classe repository com a InterfaceRepository
            //Em outras palavras, você pode usar a implementação dos métodos do repository, apenas instanciando... A interface! loucura neh? Foi o que eu pensei também!
            services.AddScoped<ITipoUsuarioRepository, TipoUsuarioRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();

            /* 
             **Configuração BD**
            */
            var connection = Configuration["ConexaoMySql:MySqlConnectionString"];
            services.AddDbContext<GeloContext>(options =>
                options.UseMySql(connection)
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime, IServiceScopeFactory services)
        {
            //Método que aplica jobs sempre que a API é inicializada. (registra o serviço na inicialização)
            applicationLifetime.ApplicationStarted.Register(() =>
            {
                app.ConfigureJobsAsync();
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //Colocar sempre no final (UseMvc)
            app.UseMvc();
        }
    }
}
