using SuggestionBoard.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuggestionBoard.Data.ViewModel
{
    public sealed class UserVM : BaseVM
    {
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public int SuggetionCount { get; set; }
        public int CommentCount { get; set; }
        public int ReactionCount { get; set; }

        public int TotalReactionPoint
        {
            get
            {
                return (SuggetionCount * 3) + (CommentCount * 2) + ReactionCount;
            }
        }
    }

    public sealed class UserPaggingListVM
    {
        public List<UserVM> Records { get; set; }

        public ToolbarVM ToolbarData { get; set; }
        public PaggingVM Pagging { get; set; }
        public Guid CurrentUserId { get; set; }
    }
}
