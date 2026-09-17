using Microsoft.EntityFrameworkCore;
using TrainingCenter.Data;
using TrainingCenter.DTOs;
using TrainingCenter.DTOs.Payment;
using TrainingCenter.Entities;

namespace TrainingCenter.Services
{
    public class EnrollmentService :IEnrollmentService
    {
        private readonly TrainingCenterDbContext _context;
        public EnrollmentService(TrainingCenterDbContext context)
        {
            _context = context;
            
        }
        public async Task<List<EnrollmentDetailsResponse>>GetAllAsync(
            string? status ,int? trackId, int? studentId,string? paymentStatus)
        {
            var query =_context.Enrollments
                .Include(e=>e.Student)
                .Include(e=>e.TrainingTrack)
                .Include(e=>e.Payments)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
            {
                query=query.Where(e=>e.Status == status);
            }
            if (trackId.HasValue)
            {
                query = query.Where(e => e.TrainingTrackId == trackId.Value);
            }
            if (studentId.HasValue)
            {
                query = query.Where(e =>e.StudentId == studentId.Value);
            }

            if (!string.IsNullOrWhiteSpace(paymentStatus))
            {
                query = query.Where(e => e.Payments.Any(p =>p.PaymentStatus == paymentStatus));
            }

            return await query .Select(e => new EnrollmentDetailsResponse
            { 
    
        EnrollmentId = e.EnrollmentId,
        StudentId = e.StudentId,
        StudentName = e.Student.FullName,
        TrainingTrackId = e.TrainingTrackId,
        TrackTitle = e.TrainingTrack.Title,
        EnrollmentDate = e.EnrollmentDate,
        Status = e.Status,
        ProgressPercentage = e.ProgressPercentage,
        FinalResult = e.FinalResult,
        TotalPaid = e.Payments.Where(p => p.PaymentStatus == "Paid").Sum(p => p.Amount),
        //one enroll=> momken many payment
        Payments = e.Payments
            .Select(p => new PaymentResponse
            {
                PaymentId = p.PaymentId,
                EnrollmentId = p.EnrollmentId,
                Amount = p.Amount,
                PaymentMethod = p.PaymentMethod,
                PaymentDate = p.PaymentDate,
                PaymentStatus = p.PaymentStatus,
                ReferenceNumber = p.ReferenceNumber,
                Notes = p.Notes
            })
            .ToList()
    })
    .ToListAsync();

        }
        public async Task<EnrollmentDetailsResponse?> GetByIdAsync(int id)
        {
            return await _context.Enrollments
                .Where(e => e.EnrollmentId == id)
                .Select(e => new EnrollmentDetailsResponse
                {
                    EnrollmentId = e.EnrollmentId,
                    StudentId = e.StudentId,
                    StudentName = e.Student.FullName,
                    TrainingTrackId = e.TrainingTrackId,
                    TrackTitle = e.TrainingTrack.Title,
                    EnrollmentDate = e.EnrollmentDate,
                    Status = e.Status,
                    ProgressPercentage = e.ProgressPercentage,
                    FinalResult = e.FinalResult,
                    TotalPaid = e.Payments.Where(p => p.PaymentStatus == "Paid").Sum(p => p.Amount),
                    Payments = e.Payments
                        .Select(p => new PaymentResponse
                        {
                            PaymentId = p.PaymentId,
                            EnrollmentId = p.EnrollmentId,
                            Amount = p.Amount,
                            PaymentMethod = p.PaymentMethod,
                            PaymentDate = p.PaymentDate,
                            PaymentStatus = p.PaymentStatus,
                            ReferenceNumber = p.ReferenceNumber,
                            Notes = p.Notes
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<EnrollmentDetailsResponse> CreateAsync(CreateEnrollmentRequest request)
        {
            var studentExists=await _context.Students.AnyAsync(s=>s.StudentId==request.StudentId && s.IsActive&&!s.IsDeleted);
            if (!studentExists)
            {
                throw new InvalidOperationException("Student not found or inactive.");
            }

            var track = await _context.TrainingTracks .FirstOrDefaultAsync(t =>t.TrainingTrackId == request.TrainingTrackId &&!t.IsDeleted);
            if (track == null)
            {
                throw new InvalidOperationException("Training track not found.");
            }
            if (track.Status != "Active")
            {
                throw new InvalidOperationException("Training track is not active.");
            }
            var duplicateEnrollment = await _context.Enrollments.AnyAsync(e =>e.StudentId == request.StudentId &&e.TrainingTrackId == request.TrainingTrackId
                && e.Status != "Cancelled");
            if (duplicateEnrollment) 
            {
                throw new InvalidOperationException("Student already has an active or pending enrollment in this track.");
            }
            //capacity el track elwahed
            var activeEnrollments = await _context.Enrollments.CountAsync(e => e.TrainingTrackId == request.TrainingTrackId && e.Status == "Active");
            if (activeEnrollments >= track.Capacity)
            {
                throw new InvalidOperationException("Training track is full");
            }

            var enrollment = new Enrollment
            {
                StudentId = request.StudentId,
                TrainingTrackId = request.TrainingTrackId,
                EnrollmentDate = DateTime.UtcNow,
                Status = "Pending",
                ProgressPercentage = 0,
                CreatedAt = DateTime.UtcNow
            };
            _context.Enrollments.Add(enrollment);

            await _context.SaveChangesAsync();

            return (await GetByIdAsync(enrollment.EnrollmentId))!;
        }

        public async Task<EnrollmentDetailsResponse?> UpdateStatusAsync(int id, string status)
        {
            var enrollment = await _context.Enrollments .FirstOrDefaultAsync(e =>e.EnrollmentId == id);
            if (enrollment == null)
            {
                return null;
            }

            var validStatuses = new[]
            {
            "Pending",
            "Active",
            "Completed",
            "Cancelled" };

            if (!validStatuses.Contains(status))
            {
                throw new InvalidOperationException("Invalid enrollment status.");
            }

            if (enrollment.Status == "Completed" &&status == "Active")
            {
                throw new InvalidOperationException( "Completed enrollment cannot become active again.");
            }

            if (enrollment.Status == "Cancelled" &&status == "Active")
            {
                throw new InvalidOperationException("Cancelled enrollment cannot become active again.");
            }
            if (enrollment.Status == "Completed" &&status == "Cancelled")
            {
                throw new InvalidOperationException(
                    "Completed enrollment cannot be cancelled.");
            }
            enrollment.Status = status;
            enrollment.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return await GetByIdAsync(id);
        }
        public async Task<List<EnrollmentDetailsResponse>>GetStudentEnrollmentsAsync(int studentId)
        {
            return await GetAllAsync( null,null, studentId, null);
        }
        public async Task<List<EnrollmentDetailsResponse>>GetTrackStudentsAsync(int trackId)
        {
            return await GetAllAsync(null, trackId, null,null);
        }
    }
}
