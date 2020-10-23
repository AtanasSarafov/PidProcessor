using Microsoft.VisualStudio.TestTools.UnitTesting;
using PidProcessor.Core.Services;
using System;

namespace PidProcessor.Core.Tests
{
    [TestClass]
    public class PidValidationServiceTests
    {
        private PidValidationService _pidValidationService;

        [TestInitialize]
        public void Initialize()
        {
            _pidValidationService = new PidValidationService();
        }

        [TestMethod]
        public void ShouldThrowExceptionIfRequestIsZero()
        {

            // TODO: Finish the implementation.
            var exception = Assert.ThrowsException<ArgumentNullException>(() => _pidValidationService.Validate(0));

        }

        [TestMethod]
        public void ShouldThrowExceptionIfRequestLengthIsLessThenTen()
        {
            // TODO: Finish the implementation.
            // Arrange

            // Act

            // Assert
        }

        [TestMethod]
        public void ShouldThrowExceptionIfRequestDateIsNotValid()
        {
            // TODO: Finish the implementation.
            // Arrange

            // Act

            // Assert
        }

        [TestMethod]
        public void ShouldThrowExceptionIfRequestSegmentsAreNotValid()
        {
            // TODO: Finish the implementation.
            // Arrange

            // Act

            // Assert
        }
    }
}
