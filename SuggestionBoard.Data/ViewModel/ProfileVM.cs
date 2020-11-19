using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SuggestionBoard.Data.ViewModel
{
    public sealed class ProfileVM
    {
        [Display(Name = "E-Mail Address")]
        public string EMail { get; set; }
        public int SuggetionCount { get; set; }
        public Guid UserId { get; set; }
        public Guid CurrentUserId { get; set; }
        public string PictureUrl { get; set; }

        public SuggestionPaggingListVM Suggestion { get; set; }
        public IList<ProfileReactionVM> Reactions { get; set; }
        public IList<ProfileCommentVM> Comments { get; set; }
    }
}
