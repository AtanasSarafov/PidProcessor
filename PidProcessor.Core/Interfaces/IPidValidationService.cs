using PidProcessor.Core.Domain;

namespace PidProcessor.Core.Interfaces
{
    public interface IPidValidationService
    {
        PidValidationResult Validate(long pid);

        PidValidationResult Validate(string pid);
    }
}
