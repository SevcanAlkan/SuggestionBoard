using SuggestionBoard.Core.Validation;
using SuggestionBoard.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SuggestionBoard.Data.ViewModel
{
    public class SuggestionCommentVM : BaseVM
    {
        public string Text { get; set; }
    }

    public class SuggestionCommentAddVM : AddVM
    {
        [GuidValidation]
        public Guid SuggestionId { get; set; }
        [Required, MinLengthErrorMessage(5), MaxLengthErrorMessage(500)]
        public string Text { get; set; }
    }

    public class SuggestionCommentUpdateVM : UpdateVM
    {
        [GuidValidation]
        public Guid SuggestionId { get; set; }
        [Required, MinLengthErrorMessage(5), MaxLengthErrorMessage(500)]
        public string Text { get; set; }
    }
}
