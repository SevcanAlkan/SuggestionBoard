using System;
using System.Collections.Generic;
using System.Text;

namespace SuggestionBoard.Core.ViewModel
{
    public interface IBaseEntityVM
    {
        Guid Id { get; set; }
    }
    public class BaseEntityVM : IBaseEntityVM
    {
        public Guid Id { get; set; }
    }
}
