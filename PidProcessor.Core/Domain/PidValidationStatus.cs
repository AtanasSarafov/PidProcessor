namespace PidProcessor.Core.Domain
{
    public enum PidValidationStatus
    {
        Valid,
        InvalidDate,
        InvalidRegionSegment,
        InvalidOrderSegment,
        InvalidGenderSegment,
        InvalidChecksumSegment
    }
}
