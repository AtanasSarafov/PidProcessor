using PidProcessor.Core.Configurations;
using PidProcessor.Core.Domain;
using PidProcessor.Core.Interfaces;
using System;
using System.Linq;

namespace PidProcessor.Core.Services
{
    public class PidDataExtractionService : IPidDataExtractionService
    {
        public Pid Segregate(long pid)
        {
            return Segregate(pid.ToString());
        }

        public Pid Segregate(string pid)
        {
            if (string.IsNullOrEmpty(pid))
            {
                throw new ArgumentNullException(nameof(pid));
            }

            if (pid.Length != 10)
            {
                throw new ArgumentOutOfRangeException(nameof(pid));
            }

            if (!long.TryParse(pid, out long pidNumber))
            {
                throw new FormatException(nameof(pid));
            }

            int.TryParse(pid.Substring(0, 2), out var year);
            int.TryParse(pid.Substring(2, 2), out var month);
            int.TryParse(pid.Substring(4, 2), out var day);
            int.TryParse(pid.Substring(6, 3), out var region);
            int.TryParse(pid.Substring(8, 1), out var gender);
            int.TryParse(pid.Substring(9, 1), out var checksum);

            return new Pid()
            {
                YearSegment = year,
                MontSegment = month,
                DaySegment = day,
                RegionSegment = region,
                OrderSegment = ExtraxtOrder(region),
                GenderSegment = gender,
                ChecksumSegment = checksum
            };
        }

        private int ExtraxtOrder(int region)
        {
            return Config.Regions.Where(i => i.Range.Contains(region))
                                 .Select(i => Array.IndexOf(i.Range, region) + 1) // NOTE: Zero based index.
                                 .FirstOrDefault();
        }
    }
}
