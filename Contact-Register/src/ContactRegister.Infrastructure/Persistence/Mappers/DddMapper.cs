using ContactRegister.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactRegister.Infrastructure.Persistence.Mappers;

public class DddMapper : IEntityTypeConfiguration<Ddd>
{
    public void Configure(EntityTypeBuilder<Ddd> builder)
    {
        builder.ToTable("tb_ddd");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(x => x.Code).HasColumnName("code");
    }
}