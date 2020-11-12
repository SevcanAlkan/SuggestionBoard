using SuggestionBoard.Core.EntityFramework;
using SuggestionBoard.Core.Enum;
using SuggestionBoard.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SuggestionBoard.Domain
{
    public class SuggestionBase : TableEntity
    {
        [Required, MinLengthErrorMessage(5), MaxLengthErrorMessage(250)]
        public string Title { get; set; }
        [Required, MinLengthErrorMessage(5), MaxLengthErrorMessage(2000)]
        public string Description { get; set; }
        [Required, DefaultValue(SuggestionStatus.Created)]
        public SuggestionStatus Status { get; set; }
        [Range(0, int.MaxValue), DefaultValue(0)]
        public int LikeAmount { get; set; }
        [Range(0, int.MaxValue), DefaultValue(0)]
        public int DislikeAmount { get; set; }
    }

    public class Suggestion : SuggestionBase
    {
        //Foreign Keys...

        public virtual ICollection<SuggestionComment> SuggestionComments { get; set; }
        public virtual ICollection<SuggestionReaction> SuggestionReactions { get; set; }

        public Suggestion()
        {
            SuggestionComments = new HashSet<SuggestionComment>();
            SuggestionReactions = new HashSet<SuggestionReaction>();
        }
    }
}
