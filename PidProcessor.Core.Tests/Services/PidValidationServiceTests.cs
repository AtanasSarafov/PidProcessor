using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PidProcessor.Core.Domain;
using PidProcessor.Core.Interfaces;
using PidProcessor.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PidProcessor.Core.Tests
{
    [TestClass]
    public class PidValidationServiceTests
    {
        private PidValidationService _pidValidationService;
        private Mock<IPidDataExtractionService> _pidDataExtractionService;
        private Pid _pidObject;
        private string _pid;

        [TestInitialize]
        public void Initialize()
        {
            _pidObject = new Pid()
            {
                YearSegment = 92,
                MontSegment = 07,
                DaySegment = 11,
                RegionSegment = 346,
                OrderSegment = 346,
                GenderSegment = 6,
                ChecksumSegment = 6
            };

            _pidDataExtractionService = new Mock<IPidDataExtractionService>();
            _pidDataExtractionService.Setup(x => x.Segregate(It.IsAny<string>()))
                                     .Callback<string>(pid =>
                                      {
                                          _pid = pid;
                                      }).Returns(_pidObject);

            _pidValidationService = new PidValidationService(_pidDataExtractionService.Object);
        }

        [TestMethod]
        public void ShouldThrowExceptionIfRequestIsNull()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(() => _pidValidationService.Validate(null));
            Assert.AreEqual("pid", exception.ParamName);
        }

        [TestMethod]
        public void ShouldThrowExceptionIfRequestIsEmpty()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(() => _pidValidationService.Validate(""));
            Assert.AreEqual("pid", exception.ParamName);
        }

        [TestMethod]
        public void ShouldThrowExceptionIfRequestIsNotInNumberFormat()
        {
            var exception = Assert.ThrowsException<FormatException>(() => _pidValidationService.Validate("NAN"));
            Assert.AreEqual("pid", exception.Message);
        }

        [TestMethod]
        public void ShouldThrowExceptionIfRequestIsOutOfTopRange()
        {
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _pidValidationService.Validate(0));
            Assert.AreEqual("pid", exception.ParamName);
        }

        [TestMethod]
        public void ShouldThrowExceptionIfRequestIsOutOfBottomRangeBottom()
        {
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _pidValidationService.Validate(99999999999));
            Assert.AreEqual("pid", exception.ParamName);
        }

        [DataTestMethod]
        [DataRow(true, false)]
        [DataRow(false, true)]
        [DataRow(false, false)]
        public void ShouldReturnCorrectStatusIfRequestDateIsNotValid(bool dateIsValid, bool monthIsValid)
        {
            _pidObject = new Pid()
            {
                YearSegment = 92,
                MontSegment = monthIsValid ? 07 : 99,
                DaySegment = dateIsValid ? 11 : 99,
                RegionSegment = 346,
                OrderSegment = 346,
                GenderSegment = 6,
                ChecksumSegment = 6
            };

            _pidDataExtractionService.Setup(x => x.Segregate(It.IsAny<string>()))
                                     .Returns(_pidObject);

            var actual = _pidValidationService.Validate(RandomPid());

            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Pid);
            Assert.AreEqual(PidValidationStatus.InvalidDate, actual.ValidationStatus);
        }

        [TestMethod]
        public void ShouldReturnCorrectStatusIfRequestChecksumIsNotValid()
        {
            _pidObject = new Pid()
            {
                YearSegment = 92,
                MontSegment = 07,
                DaySegment = 11,
                RegionSegment = 346,
                OrderSegment = 346,
                GenderSegment = 6,
                ChecksumSegment = 7 // Invalid checksum.
            };

            _pidDataExtractionService.Setup(x => x.Segregate(It.IsAny<string>()))
                                     .Returns(_pidObject);

            var actual = _pidValidationService.Validate("9207113467");

            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Pid);
            Assert.AreEqual(PidValidationStatus.InvalidChecksumSegment, actual.ValidationStatus);
        }


        [TestMethod]
        public void ShouldReturnValidResult()
        {
            _pidObject = new Pid()
            {
                YearSegment = 92,
                MontSegment = 07,
                DaySegment = 11,
                RegionSegment = 346,
                OrderSegment = 5,
                GenderSegment = 6,
                ChecksumSegment = 6
            };

            _pidDataExtractionService.Setup(x => x.Segregate(It.IsAny<string>()))
                                     .Returns(_pidObject);

            var actual = _pidValidationService.Validate(9207113466);

            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Pid);
            _pidObject.Should().BeEquivalentTo(actual.Pid);
            Assert.AreEqual(PidValidationStatus.Valid, actual.ValidationStatus);
        }

        // TODO: Move this to a more general place.
        private string RandomPid()
        {
            Random random = new Random();
            string r = "";
            int i;
            for (i = 1; i < 11; i++)
            {
                var minVal = i % 2 == 0 ? 0 : 1;
                r += random.Next(minVal, 9).ToString();
            }
            return r;
        }
    }
}
