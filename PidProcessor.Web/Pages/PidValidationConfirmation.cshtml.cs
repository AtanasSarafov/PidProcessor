using Microsoft.AspNetCore.Mvc.RazorPages;
using PidProcessor.Core.Domain;
using System;

namespace PidProcessor.Web.Pages
{
    public class PidValidationConfirmationModel : PageModel
    {
        public DateTime BirthDate { get; set; }

        public int BirthOrder { get; set; }

        public string Region { get; set; }

        public string Gender { get; set; }

        public PidValidationStatus PidValidationStatus { get; set; }

        public void OnGet(DateTime birthDate, int birthOrder, string region, string gender)
        {
            BirthDate = birthDate;
            BirthOrder = birthOrder;
            Region = region;
            Gender = gender;
        }
    }
}
