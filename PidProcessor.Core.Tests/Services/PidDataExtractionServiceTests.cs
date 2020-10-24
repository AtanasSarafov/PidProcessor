using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PidProcessor.Core.Domain;
using PidProcessor.Core.Services;
using System;

namespace PidProcessor.Core.Tests
{
    [TestClass]
    public class PidDataExtractionServiceTests
    {
        private PidDataExtractionService _pidDataExtractionService;

        [TestInitialize]
        public void Initialize()
        {
            _pidDataExtractionService = new PidDataExtractionService();
        }

        [TestMethod]
        public void ShouldThrowExceptionIfPidIsNull()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(() => _pidDataExtractionService.Segregate(null));
            Assert.AreEqual("pid", exception.ParamName);
        }

        [TestMethod]
        public void ShouldThrowExceptionIfPidIsEmpty()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(() => _pidDataExtractionService.Segregate(""));
            Assert.AreEqual("pid", exception.ParamName);
        }

        [TestMethod]
        public void ShouldThrowExceptionIfPidIsOutOfRangeException()
        {
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _pidDataExtractionService.Segregate(0));
            Assert.AreEqual("pid", exception.ParamName);
        }

        [TestMethod]
        public void ShouldThrowExceptionIfPidIsOutOfRangeExceptionTop()
        {
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _pidDataExtractionService.Segregate(10000000000));
            Assert.AreEqual("pid", exception.ParamName);
        }

        [TestMethod]
        public void ShouldThrowExceptionIfPidIsInWrongFormat()
        {
            var exception = Assert.ThrowsException<FormatException>(() => _pidDataExtractionService.Segregate("NotaNumber"));
            Assert.AreEqual("pid", exception.Message);
        }

        [TestMethod]
        public void ShouldReturnValidPidObject()
        {
            var expected = new Pid()
            {
                YearSegment = 92,
                MontSegment = 7,
                DaySegment = 11,
                RegionSegment = 346,
                OrderSegment = 5,
                GenderSegment = 6,
                ChecksumSegment = 6
            };

            var actual = _pidDataExtractionService.Segregate(9207113466);

            Assert.IsNotNull(actual);
            expected.Should().BeEquivalentTo(actual);
        }
    }
}
