using SuggestionBoard.Core.EntityFramework;
using SuggestionBoard.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SuggestionBoard.Domain
{
    public class SuggestionCommentBase : TableEntity
    {
        [GuidValidation]
        public Guid SuggestionId { get; set; }
        [Required, MinLengthErrorMessage(5), MaxLengthErrorMessage(500)]
        public string Text { get; set; }
    }

    public class SuggestionComment : SuggestionCommentBase
    {

        public virtual Suggestion Suggestion { get; set; }

        public SuggestionComment()
        {

        }
    }
}
