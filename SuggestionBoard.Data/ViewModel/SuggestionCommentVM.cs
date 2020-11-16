using SuggestionBoard.Core.Validation;
using SuggestionBoard.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SuggestionBoard.Data.ViewModel
{
    public sealed class SuggestionCommentVM : BaseVM
    {
        public string Text { get; set; }
        public Guid CreateBy { get; set; }
        public string CreateByName { get; set; }
        public DateTime CreateDT { get; set; }
    }

    public sealed class SuggestionCommentSaveVM : SaveVM
    {        
        [GuidValidation]
        public Guid SuggestionId { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Content")]
        [Required(ErrorMessage = "Comment text is required")]
        [StringLength(maximumLength: 500, MinimumLength = 5, ErrorMessage = "Comment text cannot be longer than 500 characters and less than 5 characters")]
        public string Text { get; set; }

        public string GeneralError { get; set; }
    }
}
