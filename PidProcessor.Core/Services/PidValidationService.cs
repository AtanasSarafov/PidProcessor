using PidProcessor.Core.Configurations;
using PidProcessor.Core.Domain;
using PidProcessor.Core.Interfaces;
using System;
using System.Globalization;

namespace PidProcessor.Core.Services
{
    public class PidValidationService : IPidValidationService
    {
        private readonly IPidDataExtractionService _pidDataExtractor;

        public PidValidationService(IPidDataExtractionService pidDataExtractor)
        {
            _pidDataExtractor = pidDataExtractor;
        }

        public PidValidationResult Validate(long pid)
        {
            return Validate(pid.ToString());
        }

        public PidValidationResult Validate(string pid)
        {
            if (string.IsNullOrEmpty(pid))
            {
                throw new ArgumentNullException(nameof(pid));
            }

            if (!long.TryParse(pid, out long pidNumber))
            {
                throw new FormatException(nameof(pid));
            }

            if (!ValidateRange(pid, pidNumber))
            {
                throw new ArgumentOutOfRangeException(nameof(pid));
            }

            var pidObject = _pidDataExtractor.Segregate(pid);
            if (pidObject == null)
            {
                throw new ArgumentNullException(nameof(pidObject));
            }

            var result = new PidValidationResult() { Pid = pidObject };

            var dateValidationStatus = ValidateDate(pidObject);
            if (dateValidationStatus != PidValidationStatus.Valid)
            {
                result.ValidationStatus = dateValidationStatus;
                return result;
            }

            var checksumValidationStatus = ValidateChecksum(pid, pidObject.ChecksumSegment);
            if (checksumValidationStatus != PidValidationStatus.Valid)
            {
                result.ValidationStatus = checksumValidationStatus;
                return result;
            }

            return result;
        }

        private bool ValidateRange(string pid, long pidNumber)
        {
            return pid.Length == 10 && pidNumber >= 0001000000 && pidNumber <= 9999999999;
        }

        private PidValidationStatus ValidateDate(Pid pid)
        {
            var year = pid.YearSegment;
            var month = pid.MontSegment;

            // NOTE: For births before 1 January 1900, 20 is added to the month.
            if (month > 20)
            {
                month -= 20;
                year += 1800;
            }
            // NOTE: For births from 1 January 2000 to 31 December 2099, 40 is added to the month
            else if (month > 40)
            {
                month -= 40;
                year += 2000;
            }
            else
            {
                year += 1900;
            }

            if (!DateTime.TryParse($"{month}.{pid.DaySegment}.{year}", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
            {
                return PidValidationStatus.InvalidDate;
            }

            return PidValidationStatus.Valid;
        }

        private PidValidationStatus ValidateChecksum(string pid, int checksumSegment)
        {
            var checksum = 0;
            for (int i = 0; i < Config.Weights.Length; i++)
            {
                // NOTE: Each digit is multiplied by its weight (see Wikipedia table). 
                // The products obtained are added.
                int.TryParse(pid.Substring(i, 1), out var digit);
                checksum += digit * Config.Weights[i];
            }

            // NOTE: The sum is divided by 11 ( use sum % 11, not /, modulus, not division)
            double checksumRemainder = checksum % 11;
            var checksumDigit = checksumRemainder < 10 ? checksumRemainder : 0;

            if (checksumDigit != checksumSegment)
            {
                return PidValidationStatus.InvalidChecksumSegment;
            }

            return PidValidationStatus.Valid;
        }
    }
}
