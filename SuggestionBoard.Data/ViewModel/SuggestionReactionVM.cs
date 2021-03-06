﻿using SuggestionBoard.Core.Enum;
using SuggestionBoard.Core.Validation;
using SuggestionBoard.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SuggestionBoard.Data.ViewModel
{
    public sealed class SuggestionReactionVM : BaseVM
    {
        public UserReaction Reaction { get; set; }
        public Guid CreateBy { get; set; }
        public string CreateByName { get; set; }
        public DateTime CreateDT { get; set; }
    }

    public sealed class SuggestionReactionSaveVM : SaveVM
    {
        [GuidValidation]
        public Guid SuggestionId { get; set; }
        [Required]
        public UserReaction Reaction { get; set; }
    }
}
