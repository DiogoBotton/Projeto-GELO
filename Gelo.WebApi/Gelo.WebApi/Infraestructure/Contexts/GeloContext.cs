using Gelo.WebApi.Domains;
using Gelo.WebApi.Infraestructure.EntityTypeConfigurations;
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
        public DbSet<NotaFiscalCondicao> NotasFiscaisCondicoes { get; set; }
        public DbSet<BoletoCondicao> BoletosCondicoes { get; set; }
        public DbSet<Contato> Contatos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<ProdutoPorCliente> ProdutosPorClientes { get; set; }

        public GeloContext(DbContextOptions<GeloContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Classes mapeadoras
            modelBuilder.ApplyConfiguration(new UsuarioEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TipoUsuarioEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BoletoCondicaoEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new NotaFiscalCondicaoEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ContatoEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EnderecoEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProdutoEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProdutoPorClienteEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ClienteEntityTypeConfiguration());

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
