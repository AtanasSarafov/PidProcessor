using Microsoft.VisualStudio.TestTools.UnitTesting;
using PidProcessor.Core.Configurations;
using System;
using System.Linq;

namespace PidProcessor.Web.Pages
{
    [TestClass]
    public class PidValidationConfirmationModelTests
    {
        [TestMethod]
        public void ShouldStoreParameterValuesInProperties()
        {
            var birthDate = new DateTime(1992, 07, 11);
            var region = Config.Regions.FirstOrDefault().Name;
            var gender = "Male";
            var birthOrder = 5;

            var pidValidationConfirmationModel = new PidValidationConfirmationModel();

            pidValidationConfirmationModel.OnGet(birthDate, birthOrder, region, gender);

            Assert.AreEqual(birthDate, pidValidationConfirmationModel.BirthDate);
            Assert.AreEqual(region, pidValidationConfirmationModel.Region);
            Assert.AreEqual(gender, pidValidationConfirmationModel.Gender);
            Assert.AreEqual(birthOrder, pidValidationConfirmationModel.BirthOrder);
        }
    }
}
