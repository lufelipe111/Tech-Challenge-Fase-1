using ContactRegister.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactRegister.Infrastructure.Persistence.Mappers;

public class ContactMapper : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("tb_contact");
        builder.HasKey(x => x.Id).HasName("id");
        
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(x => x.FirstName).HasColumnName("first_name");
        builder.Property(x => x.LastName).HasColumnName("last_name");
        builder.Property(x => x.Email).HasColumnName("email");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        builder.Property(x => x.DddId).HasColumnName("ddd_id");
        
        #region Address
        
        builder
            .ComplexProperty(x => x.Address)
            .Property(a => a.AddressLine1).HasColumnName("address_line1");
        builder
            .ComplexProperty(x => x.Address)
            .Property(a => a.AddressLine2).HasColumnName("address_line2").IsRequired(false);
        builder
            .ComplexProperty(x => x.Address)
            .Property(a => a.City).HasColumnName("city");
        builder
            .ComplexProperty(x => x.Address)
            .Property(a => a.State).HasColumnName("state");
        builder
            .ComplexProperty(x => x.Address)
            .Property(a => a.PostalCode).HasColumnName("postal_code");
        
        #endregion
        
        #region HomeNumber
        
        builder
            .ComplexProperty(x => x.HomeNumber)
            .Property(h => h.Number).HasColumnName("tx_home_number")
            .IsRequired(false);
        
        #endregion
        
        #region MobileNumber
        
        builder
            .ComplexProperty(x => x.MobileNumber)
            .Property(h => h.Number).HasColumnName("tx_mobile_number")
            .IsRequired();
        
        #endregion
        
        #region Navigations
        
        builder
            .HasOne(c => c.DddCode)
            .WithMany()
            .HasForeignKey(x => x.DddId)
            .IsRequired();
        
        #endregion
    }
}