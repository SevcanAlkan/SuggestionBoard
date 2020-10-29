using SuggestionBoard.Core.Enum;
using SuggestionBoard.Core.Validation;
using SuggestionBoard.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SuggestionBoard.Data.ViewModel
{
    public class SuggestionVM : BaseVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public SuggestionStatus Status { get; set; }
        public int CommentCount { get; set; }
        public int LikeAmount { get; set; }
        public int DislikeAmount { get; set; }
        public int TotalReaction { get; set; }
    }

    public class SuggestionSaveVM : SaveVM
    {
        [DataType(DataType.Text)]
        [Required, MinLengthErrorMessage(5), MaxLengthErrorMessage(250)]
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        [Required, MinLengthErrorMessage(5), MaxLengthErrorMessage(2000)]
        public string Description { get; set; }
        [Required, DefaultValue(SuggestionStatus.Created)]
        public SuggestionStatus Status { get; set; }
        [Range(0, int.MaxValue), DefaultValue(0)]
        public int LikeAmount { get; set; }
        [Range(0, int.MaxValue), DefaultValue(0)]
        public int DislikeAmount { get; set; }
        public int TotalReaction { 
            get {
                return DislikeAmount + LikeAmount;
            } 
        }

        public List<SuggestionCommentVM> suggestionComments { get; set; }
        public List<SuggestionReactionVM> suggestionReactions { get; set; }
    }
}
