namespace PidProcessor.Core.Domain
{
    public class PidValidationResult
    {
        public Pid Pid { get; set; }

        public PidValidationStatus ValidationStatus { get; set; }
    }
}
