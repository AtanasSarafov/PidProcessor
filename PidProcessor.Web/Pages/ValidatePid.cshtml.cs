using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PidProcessor.Core.Configurations;
using PidProcessor.Core.Domain;
using PidProcessor.Core.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PidProcessor.Web.Pages
{
    public class ValidatePidModel : PageModel
    {
        private readonly IPidValidationService _pidValidationService;

        public ValidatePidModel(IPidValidationService pidValidationService)
        {
            this._pidValidationService = pidValidationService;
        }

        [BindProperty]
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "The PIN should be consist of 10 digits.")]
        [Range(0001000000, 9999999999, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public string Pid { get; set; }

        public IActionResult OnPost()
        {
            IActionResult actionResult = Page();

            if (ModelState.IsValid)
            {
                try
                {
                    var result = _pidValidationService.Validate(Pid);
                    if (result.ValidationStatus == PidValidationStatus.Valid)
                    {
                        var validationModel = GenerateValidationConfirmationModel(result.Pid);
                        actionResult = RedirectToPage("PidValidationConfirmation", validationModel);
                    }
                    else if (result.ValidationStatus == PidValidationStatus.InvalidDate)
                    {
                        ModelState.AddModelError("Pid", "Invalid PID(EGN)!  ---  Error info: Invalid PID Date! Please check the PID date segment - the first 6 digits.");
                    }
                    else if (result.ValidationStatus == PidValidationStatus.InvalidChecksumSegment)
                    {
                        ModelState.AddModelError("Pid", "Invalid PID(EGN)!  ---  Error info: Invalid PID Checksum! Please check the PID digits and try again.");
                    }
                }
                catch (Exception)
                {
                    // TODO: Add error handling and logging.
                    throw;
                }
            }

            return actionResult;
        }

        private object GenerateValidationConfirmationModel(Pid pid)
        {
            DateTime.TryParse($"{pid.MontSegment}.{pid.DaySegment}.{pid.YearSegment}", out var birthDate);
            var region = Config.Regions.FirstOrDefault(i => i.Range.Contains(pid.RegionSegment)).Name;
            var gender = pid.GenderSegment % 2 == 0 ? "Male" : "Female";
            var birthOrder = pid.OrderSegment;

            return new
            {
                birthDate,
                birthOrder,
                region,
                gender
            };
        }
    }
}
