using SuggestionBoard.Core.EntityFramework;
using SuggestionBoard.Core.Enum;
using SuggestionBoard.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SuggestionBoard.Domain
{
    public class SuggestionReactionBase : TableEntity
    {
        [GuidValidation]
        public Guid SuggestionId { get; set; }
        [Required]
        public UserReaction Reaction { get; set; }
    }

    public class SuggestionReaction : SuggestionReactionBase
    {

        public virtual Suggestion Suggestion { get; set; }

        public SuggestionReaction()
        {

        }
    }
}
