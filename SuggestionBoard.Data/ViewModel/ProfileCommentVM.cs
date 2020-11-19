using System;
using System.Collections.Generic;
using System.Text;

namespace SuggestionBoard.Data.ViewModel
{
    public sealed class ProfileCommentVM
    {
        public Guid SuggestionId { get; set; }
        public string SuggestionTitle { get; set; }
        public string CommentText { get; set; }
        public DateTime CreateDT { get; set; }
    }
}
