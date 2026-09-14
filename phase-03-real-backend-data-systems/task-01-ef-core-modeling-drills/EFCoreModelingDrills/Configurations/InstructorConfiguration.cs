using EFCoreModelingDrills.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreModelingDrills.Configurations;

    public class InstructorConfiguration:IEntityTypeConfiguration<Instructor>
    {
    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany (i=>i.Tracks)
            .WithOne (t => t.Instructor)
            .HasForeignKey (t=>t.InstructorId)
             .IsRequired()
            .OnDelete (DeleteBehavior.Restrict);

        builder.Property(i => i.FullName)
          .IsRequired()
          .HasMaxLength(100);

    }

    }

