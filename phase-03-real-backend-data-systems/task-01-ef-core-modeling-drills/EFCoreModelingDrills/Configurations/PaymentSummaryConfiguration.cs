using EFCoreModelingDrills.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreModelingDrills.Configurations;

public class PaymentSummaryConfiguration : IEntityTypeConfiguration<PaymentSummary>
{
    public void Configure(EntityTypeBuilder<PaymentSummary> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.TotalRequired)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(p => p.TotalPaid)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(p => p.RemainingAmount)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(p => p.PaymentStatus)
               .IsRequired()
               .HasMaxLength(30);

        builder.HasOne(p => p.Enrollment)
               .WithOne(e => e.PaymentSummary)
               .HasForeignKey<PaymentSummary>(p => p.EnrollmentId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => p.EnrollmentId)
               .IsUnique();
    }
}