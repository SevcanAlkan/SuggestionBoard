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
    public sealed class CategoryVM : BaseVM
    {
        public string Name { get; set; }

        public Guid CreateById { get; set; }
        public string CreateByName { get; set; }

        public DateTime CreateDateTime { get; set; }
    }

    public sealed class CategoryPaggingListVM
    {
        public List<CategoryVM> Records { get; set; }

        public PaggingVM Pagging { get; set; }
        public Guid CurrentUserId { get; set; }
    }

    public sealed class CategorySaveVM : SaveVM
    {
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Name is required")]
        [StringLength(maximumLength: 100, MinimumLength = 5,
            ErrorMessage = "Name cannot be longer than 100 characters and less than 5 characters")]
        public string Name { get; set; }

        public Guid CreateBy { get; set; }
        public string CreateByName { get; set; }
        public DateTime CreateDT { get; set; }
    }

    public sealed class CategoryDetailVM
    {
        public Guid Id { get; set; }
        public CategorySaveVM Rec { get; set; }

        public bool CanEdit { get; set; }

        public string GeneralError { get; set; }
    }
}
