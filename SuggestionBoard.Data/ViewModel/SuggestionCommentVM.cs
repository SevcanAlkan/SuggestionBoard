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
    }

    public sealed class SuggestionCommentSaveVM : SaveVM
    {
        [GuidValidation]
        public Guid SuggestionId { get; set; }
        [Required, MinLength(5), MaxLength(500)]
        public string Text { get; set; }
    }
}
