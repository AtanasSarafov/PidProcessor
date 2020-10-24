using PidProcessor.Core.Domain;

namespace PidProcessor.Core.Interfaces
{
    public interface IPidDataExtractionService
    {
        Pid Segregate(long pid);

        Pid Segregate(string pid);
    }
}
