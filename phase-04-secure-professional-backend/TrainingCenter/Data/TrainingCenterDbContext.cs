using Microsoft.EntityFrameworkCore;
using TrainingCenter.Entities;
namespace TrainingCenter.Data
{
    public class TrainingCenterDbContext :DbContext
    {
        public TrainingCenterDbContext(DbContextOptions<TrainingCenterDbContext>options):base(options)
        {

            
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<TrainingTrack> TrainingTracks { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Payment> Payments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Student
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(s => s.StudentId);

                entity.Property(s => s.FullName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(s => s.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasIndex(s => s.Email)
                    .IsUnique();
            });

            // Instructor
            modelBuilder.Entity<Instructor>(entity =>
            {
                entity.HasKey(i => i.InstructorId);

                entity.Property(i => i.FullName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(i => i.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(i => i.Specialization)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasIndex(i => i.Email)
                    .IsUnique();
            });

            // Training Track
            modelBuilder.Entity<TrainingTrack>(entity =>
            {
                entity.HasKey(t => t.TrainingTrackId);

                entity.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(t => t.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(t => t.Code)
                    .IsUnique();

                entity.HasOne(t => t.Instructor)
                    .WithMany(i => i.TrainingTracks)
                    .HasForeignKey(t => t.InstructorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Enrollment
            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => e.EnrollmentId);

                entity.Property(e => e.ProgressPercentage)
                    .HasPrecision(5, 2);

                entity.HasOne(e => e.Student)
                    .WithMany(s => s.Enrollments)
                    .HasForeignKey(e => e.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.TrainingTrack)
                    .WithMany(t => t.Enrollments)
                    .HasForeignKey(e => e.TrainingTrackId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => new
                {
                    e.StudentId,
                    e.TrainingTrackId
                })
                .IsUnique();
            });

            // Payment
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(p => p.PaymentId);

                entity.Property(p => p.Amount)
                    .HasPrecision(18, 2);

                entity.HasOne(p => p.Enrollment)
                    .WithMany(e => e.Payments)
                    .HasForeignKey(p => p.EnrollmentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    
    }
}
