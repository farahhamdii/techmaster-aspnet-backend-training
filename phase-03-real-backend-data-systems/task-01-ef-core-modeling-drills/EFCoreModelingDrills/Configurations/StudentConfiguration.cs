using EFCoreModelingDrills.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreModelingDrills.Configurations;

    public class StudentConfiguration : IEntityTypeConfiguration<Student>

{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(s => s.Id);
        //one to one
        builder.HasOne(s => s.Profile)
            .WithOne(s => s.Student)
            .HasForeignKey<StudentProfile>(p => p.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

    }

    
}
