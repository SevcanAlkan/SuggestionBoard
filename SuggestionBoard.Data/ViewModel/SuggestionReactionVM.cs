using SuggestionBoard.Core.Enum;
using SuggestionBoard.Core.Validation;
using SuggestionBoard.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SuggestionBoard.Data.ViewModel
{
    public class SuggestionReactionVM : BaseVM
    {
        public UserReaction Reaction { get; set; }
    }

    public class SuggestionReactionSaveVM : SaveVM
    {
        [GuidValidation]
        public Guid SuggestionId { get; set; }
        [Required]
        public UserReaction Reaction { get; set; }
    }
}
