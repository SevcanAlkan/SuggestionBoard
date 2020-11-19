using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SuggestionBoard.Data.ViewModel
{
    public sealed class ProfileUpdateVM
    {
        [Required(ErrorMessage = "E-Mail is required")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-Mail Address")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "E-Mail address cannot be longer than 100 characters and less than 5 characters")]
        public string EMail { get; set; }

        [Required(ErrorMessage = "Old Password is required")]
        [DataType(DataType.Password, ErrorMessage = "Invalid password")]
        [Display(Name = "Old Password")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Password cannot be longer than 50 characters and less than 10 characters")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password, ErrorMessage = "Invalid new password")]
        [Display(Name = "New Password")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "New password cannot be longer than 50 characters and less than 10 characters")]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password, ErrorMessage = "Invalid password")]
        [Compare("NewPassword", ErrorMessage = "Confirm password is not equal to Password")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Picture URL")]
        public string PictureUrl { get; set; }

        public string GeneralError { get; set; }
    }
}
