using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingCenter.Data;
using TrainingCenter.Entities;

namespace TrainingCenter.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BadEnrollmentsController : ControllerBase
{
    private readonly TrainingCenterDbContext _db;

    public BadEnrollmentsController(TrainingCenterDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var data = _db.Enrollments
            .Include(e => e.Student)
            .Include(e => e.TrainingTrack)
            .Include(e => e.Payments)
            .ToList();

        return Ok(data);
    }

    [HttpPost]
    public IActionResult Create(Enrollment enrollment)
    {
        enrollment.EnrollmentDate = DateTime.Now;
        enrollment.Status = "Active";

        _db.Enrollments.Add(enrollment);
        _db.SaveChanges();

        return Ok(enrollment);
    }

    [HttpPost("pay")]
    public IActionResult Pay(int enrollmentId, decimal amount)
    {
        var enrollment = _db.Enrollments
            .Include(x => x.Payments)
            .FirstOrDefault(x => x.EnrollmentId == enrollmentId);

        if (enrollment == null)
        {
            return Ok("not found");
        }

        var payment = new Payment
        {
            EnrollmentId = enrollmentId,
            Amount = amount,
            PaymentDate = DateTime.Now,
            PaymentStatus = "Done"
        };

        _db.Payments.Add(payment);
        _db.SaveChanges();

        return Ok(payment);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var item = _db.Enrollments.Find(id);

        if (item == null)
        {
            return Ok("missing");
        }

        _db.Enrollments.Remove(item);
        _db.SaveChanges();

        return Ok("deleted");
    }
}