namespace PidProcessor.Core.Domain
{
    public class PidValidationResult
    {
        public Pid Pid { get; set; }

        public string Message { get; set; }

        public PidValidationStatus Status { get; set; }
    }
}
