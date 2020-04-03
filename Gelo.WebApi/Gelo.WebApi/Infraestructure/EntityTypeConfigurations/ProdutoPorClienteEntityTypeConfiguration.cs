using Gelo.WebApi.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gelo.WebApi.Infraestructure.EntityTypeConfigurations
{
    public class ProdutoPorClienteEntityTypeConfiguration : IEntityTypeConfiguration<ProdutoPorCliente>
    {
        public void Configure(EntityTypeBuilder<ProdutoPorCliente> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();

            builder.HasOne<Produto>()
                .WithMany()
                .HasForeignKey("ProdutoId")
                .IsRequired();

            builder.HasOne<Cliente>()
                .WithMany()
                .HasForeignKey("ClienteId")
                .IsRequired();
        }
    }
}
