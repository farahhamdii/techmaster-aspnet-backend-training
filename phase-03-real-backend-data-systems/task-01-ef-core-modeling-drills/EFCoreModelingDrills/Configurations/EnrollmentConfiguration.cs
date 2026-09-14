using EFCoreModelingDrills.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreModelingDrills.Configurations;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Status)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(e => e.EnrollmentDate)
               .IsRequired();

        builder.Property(e => e.FinalGrade)
               .HasColumnType("decimal(5,2)");

        builder.HasOne(e => e.Student)
               .WithMany(s => s.Enrollments)
               .HasForeignKey(e => e.StudentId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Track)
               .WithMany(t => t.Enrollments)
               .HasForeignKey(e => e.TrackId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);
    }
}