using SuggestionBoard.Core.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SuggestionBoard.Data.ViewModel
{
    public sealed class ProfileReactionVM
    {
        public Guid SuggestionId { get; set; }
        public string SuggestionTitle { get; set; }
        public UserReaction Reaction { get; set; }
        public DateTime CreateDT { get; set; }
    }
}
