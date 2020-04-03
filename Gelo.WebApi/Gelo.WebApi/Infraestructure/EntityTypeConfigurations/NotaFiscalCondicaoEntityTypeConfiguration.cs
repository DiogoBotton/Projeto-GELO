using Gelo.WebApi.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gelo.WebApi.Infraestructure.EntityTypeConfigurations
{
    public class NotaFiscalCondicaoEntityTypeConfiguration : IEntityTypeConfiguration<NotaFiscalCondicao>
    {
        public void Configure(EntityTypeBuilder<NotaFiscalCondicao> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();

            builder.Property(x => x.Descricao).IsRequired();

            builder.HasOne<Cliente>()
                .WithMany()
                .HasForeignKey("ClienteId")
                .IsRequired();
        }
    }
}
