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
    public sealed class SuggestionVM : BaseVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public SuggestionStatus Status { get; set; }
        public int CommentCount { get; set; }
        public int LikeAmount { get; set; }
        public int DislikeAmount { get; set; }
        public int TotalReaction { get; set; }

        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }

        public Guid CreateById { get; set; }
        public string CreateByName { get; set; }

        public DateTime CreateDateTime { get; set; }
    }

    public sealed class SuggestionPaggingListVM
    {
        public List<SuggestionVM> Records { get; set; }

        public PaggingVM Pagging { get; set; }

        public ToolbarVM ToolbarData { get; set; }
    }

    public sealed class SuggestionSaveVM : SaveVM
    {
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Title is required")]
        [StringLength(maximumLength: 250, MinimumLength = 5, ErrorMessage = "Title cannot be longer than 250 characters and less than 5 characters")]
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Description is required")]
        [StringLength(maximumLength: 2000, MinimumLength = 5, ErrorMessage = "Description cannot be longer than 2000 characters and less than 5 characters")]
        public string Description { get; set; }
        [Required, DefaultValue(SuggestionStatus.Created)]
        public SuggestionStatus Status { get; set; }
        public int LikeAmount { get; set; }
        public int DislikeAmount { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }

        public Guid CreateBy { get; set; }
        public string CreateByName { get; set; }
        public DateTime CreateDT { get; set; }
    }

    public sealed class SuggestionDetailVM
    {
        public Guid Id { get; set; }
        public SuggestionSaveVM Rec { get; set; }

        public int TotalReaction
        {
            get
            {
                return Rec.DislikeAmount + Rec.LikeAmount + (SuggestionComments != null ? SuggestionComments.Count : 0) * 2;
            }
        }

        public List<SuggestionCommentVM> SuggestionComments { get; set; }
        public List<SuggestionReactionVM> SuggestionReactions { get; set; }

        public List<SelectListVM> Categories { get; set; }

        public bool CanEdit { get; set; }

        public string GeneralError { get; set; }
    }
}
