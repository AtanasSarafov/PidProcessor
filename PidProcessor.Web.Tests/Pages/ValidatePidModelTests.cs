using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PidProcessor.Core.Configurations;
using PidProcessor.Core.Domain;
using PidProcessor.Core.Interfaces;
using System;
using System.Linq;

namespace PidProcessor.Web.Pages
{
    [TestClass]
    public class ValidatePidModelTests
    {
        private string _pid;
        private Pid _pidObject;
        private PidValidationResult _pidValidationResult;
        private Mock<IPidValidationService> _pidValidationService;
        private ValidatePidModel _validatePidModel;

        [TestInitialize]
        public void Initialize()
        {
            _pid = "9207113466";

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

            _pidValidationResult = new PidValidationResult()
            {
                Pid = _pidObject,
                ValidationStatus = PidValidationStatus.Valid
            };

            _pidValidationService = new Mock<IPidValidationService>();
            _pidValidationService.Setup(x => x.Validate(It.IsAny<string>()))
                                 .Returns(_pidValidationResult);

            _validatePidModel = new ValidatePidModel(_pidValidationService.Object)
            {
                Pid = _pid
            };
        }

        [DataTestMethod]
        [DataRow(1, true)]
        [DataRow(0, false)]
        public void ShouldCallValidationServiceIfModelIsValid(int expectedValidationServiceCalls, bool isModelValid)
        {
            if (!isModelValid)
            {
                _validatePidModel.ModelState.AddModelError("JustAKey", "AnErrorMessage");
            }

            _validatePidModel.OnPost();

            _pidValidationService.Verify(x => x.Validate(_pid), Times.Exactly(expectedValidationServiceCalls));
        }

        [TestMethod]
        public void ShouldAddModelErrorIfPidDateIsInvalid()
        {
            _pidValidationResult.ValidationStatus = PidValidationStatus.InvalidDate;

            _validatePidModel.OnPost();

            Assert.IsTrue(_validatePidModel.ModelState.TryGetValue("Pid", out ModelStateEntry modelStateEntry));
            Assert.AreEqual(1, modelStateEntry.Errors.Count);
            Assert.AreEqual("Invalid PID(EGN)!  ---  Error info: Invalid PID Date! Please check the PID date segment - the first 6 digits.", modelStateEntry.Errors[0]?.ErrorMessage);
        }


        [TestMethod]
        public void ShouldAddModelErrorIfPidChecksumIsInvalid()
        {
            _pidValidationResult.ValidationStatus = PidValidationStatus.InvalidChecksumSegment;

            _validatePidModel.OnPost();

            Assert.IsTrue(_validatePidModel.ModelState.TryGetValue("Pid", out ModelStateEntry modelStateEntry));
            Assert.AreEqual(1, modelStateEntry.Errors.Count);
            Assert.AreEqual("Invalid PID(EGN)!  ---  Error info: Invalid PID Checksum! Please check the PID digits and try again.", modelStateEntry.Errors[0]?.ErrorMessage);
        }

        [TestMethod]
        public void ShouldNotAddModelErrorIfPidIsValid()
        {
            _pidValidationResult.ValidationStatus = PidValidationStatus.Valid;

            _validatePidModel.OnPost();

            Assert.IsFalse(_validatePidModel.ModelState.TryGetValue("Pid", out ModelStateEntry modelStateEntry));
        }

        [DataTestMethod]
        [DataRow(typeof(PageResult), false, null)]
        [DataRow(typeof(PageResult), true, PidValidationStatus.InvalidChecksumSegment)]
        [DataRow(typeof(RedirectToPageResult), true, PidValidationStatus.Valid)]
        public void ShouldReturnExpectedActionResult(Type expectedActionResultType, bool isModelValid, PidValidationStatus? pidValidationStatus)
        {
            if (!isModelValid)
            {
                _validatePidModel.ModelState.AddModelError("Pid", "Error Message");
            }

            if (pidValidationStatus.HasValue)
            {
                _pidValidationResult.ValidationStatus = pidValidationStatus.Value;
            }

            IActionResult actionResult = _validatePidModel.OnPost();

            Assert.IsInstanceOfType(actionResult, expectedActionResultType);
        }

        [TestMethod]
        public void ShouldRedirectToBookDeskConfirmationPage()
        {
            _pidValidationResult.ValidationStatus = PidValidationStatus.Valid;
            var birthDate = new DateTime(1992, 07, 11);
            var region = Config.Regions.FirstOrDefault(i => i.Range.Contains(_pidObject.RegionSegment)).Name;
            var gender = "Male";
            var birthOrder = 5;

            IActionResult actionResult = _validatePidModel.OnPost();

            var redirectToPageResult = (RedirectToPageResult)actionResult;
            Assert.AreEqual("PidValidationConfirmation", redirectToPageResult.PageName);

            var routeValues = redirectToPageResult.RouteValues;
            Assert.AreEqual(4, routeValues.Count);

            Assert.IsTrue(routeValues.TryGetValue("birthDate", out object actualBirthDate));
            Assert.AreEqual(birthDate, actualBirthDate);

            Assert.IsTrue(routeValues.TryGetValue("region", out object actualRegion));
            Assert.AreEqual(region, actualRegion);

            Assert.IsTrue(routeValues.TryGetValue("gender", out object actualGender));
            Assert.AreEqual(gender, actualGender);

            Assert.IsTrue(routeValues.TryGetValue("birthOrder", out object actualBirthOrder));
            Assert.AreEqual(birthOrder, actualBirthOrder);
        }
    }
}
