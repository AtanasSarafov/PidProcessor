using System;

namespace PidProcessor.Core.Domain
{
    public class Pid
    {
        public int YearSegment { get; set; }

        public int MontSegment { get; set; }

        public int DaySegment { get; set; }

        public int RegionSegment { get; set; }

        public int OrderSegment { get; set; }

        public int GenderSegment { get; set; }

        public int ChecksumSegment { get; set; }
    }
}
