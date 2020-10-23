using System;

namespace PidProcessor.Core.Domain
{
    public class Pid
    {
        public DateTime DateSegment { get; set; }

        public int RegionSegment { get; set; }

        public int OrderSegment { get; set; }

        public int GenderSegment { get; set; }

        public int ChecksumSegment { get; set; }
    }
}
