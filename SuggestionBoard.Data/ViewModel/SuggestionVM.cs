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

    public class SuggestionDetailVM : BaseVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public SuggestionStatus Status { get; set; }
        public int LikeAmount { get; set; }
        public int DislikeAmount { get; set; }
        public int TotalReaction { get; set; }

        public List<SuggestionCommentVM> suggestionComments { get; set; }
        public List<SuggestionReactionVM> suggestionReactions { get; set; }
    }

    public class SuggestionAddVM : AddVM
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

    public class SuggestionUpdateVM : UpdateVM
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
}
