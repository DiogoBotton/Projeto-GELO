using Gelo.WebApi.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gelo.WebApi.Infraestructure.EntityTypeConfiguration
{
    public class TipoUsuarioEntityTypeConfiguration : IEntityTypeConfiguration<TipoUsuario>
    {
        public void Configure(EntityTypeBuilder<TipoUsuario> builder)
        {
            builder.ToTable("TiposUsuarios");

            builder.Property(x => x.Id).IsRequired();
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Titulo).IsRequired();
        }
    }
}
