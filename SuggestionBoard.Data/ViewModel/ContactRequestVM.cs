using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SuggestionBoard.Data.ViewModel
{
    public class ContactRequestVM
    {
        [Required(ErrorMessage = "Name is required")]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Name address cannot be longer than 100 characters and less than 5 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "E-Mail is required")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-Mail Address")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "E-Mail address cannot be longer than 100 characters and less than 5 characters")]
        public string EMail { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [DataType(DataType.Text)]
        [Display(Name = "Subject")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Subject address cannot be longer than 100 characters and less than 5 characters")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Message")]
        [StringLength(2000, MinimumLength = 100, ErrorMessage = "Message address cannot be longer than 2000 characters and less than 100 characters")]
        public string Message { get; set; }

        public string GeneralError { get; set; }
    }
}
