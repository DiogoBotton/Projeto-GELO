using Gelo.WebApi.Domains;
using Gelo.WebApi.Infraestructure.EntityTypeConfiguration;
using Gelo.WebApi.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gelo.WebApi.Infraestructure.Contexts
{
    public class GeloContext : DbContext, IUnitOfWork
    {
        public DbSet<TipoUsuario> TiposUsuarios { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }

        public GeloContext(DbContextOptions<GeloContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Classes mapeadoras
            modelBuilder.ApplyConfiguration(new UsuarioEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TipoUsuarioEntityTypeConfiguration());

            //Caso houver uma referencia de 2 Foreign Key's para uma mesma tabela, o loop altera o comportamento de Cascata para Restrito
            //[...] automaticamente ao fazer a migration
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.GetForeignKeys()
                    .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                    .ToList()
                    .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict);
            }

        }

        //Padrão de repositorios usando UnitOfWork, salva alterações na DB independentemente de quais tabelas foram alteradas.
        public async Task SaveDbChanges(CancellationToken cancellationToken = default)
        {
            await base.SaveChangesAsync();
        }
    }
}
