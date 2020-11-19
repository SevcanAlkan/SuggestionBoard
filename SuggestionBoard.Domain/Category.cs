using SuggestionBoard.Core.EntityFramework;
using SuggestionBoard.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SuggestionBoard.Domain
{  
    public class CategoryBase : TableEntity
    {
        [Required, MinLengthErrorMessage(5), MaxLengthErrorMessage(100)]
        public string Name { get; set; }
    }

    public class Category : CategoryBase
    {
        public virtual ICollection<Suggestion> Suggestions { get; set; }

        public Category()
        {
            Suggestions = new HashSet<Suggestion>();
        }
    }
}
