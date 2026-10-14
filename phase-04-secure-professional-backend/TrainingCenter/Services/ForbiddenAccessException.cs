
namespace TrainingCenter.Services
{
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException(string message)
            : base(message)
        {
        }
    }
}
