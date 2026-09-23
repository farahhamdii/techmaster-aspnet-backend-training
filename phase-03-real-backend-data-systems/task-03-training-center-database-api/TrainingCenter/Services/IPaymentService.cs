using TrainingCenter.DTOs;
using TrainingCenter.DTOs.Payment;
namespace TrainingCenter.Services;
public interface IPaymentService
{
    Task<List<PaymentResponse>> GetAllAsync(DateTime? from, DateTime? to, string? status);
    Task<PaymentResponse?> GetByIdAsync(int id);
    Task<PaymentResponse> CreateAsync(CreatePaymentRequest request);
    Task<PaymentResponse?> UpdateStatusAsync( int id,string status);
    Task<List<PaymentResponse>> GetEnrollmentPaymentsAsync(int enrollmentId);


 
}
