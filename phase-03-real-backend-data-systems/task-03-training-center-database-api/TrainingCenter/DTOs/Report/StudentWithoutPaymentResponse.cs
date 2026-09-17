namespace TrainingCenter.DTOs;

public class StudentWithoutPaymentResponse
{
    public int StudentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}