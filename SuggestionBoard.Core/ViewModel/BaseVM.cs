using System;
using System.Collections.Generic;
using System.Text;

namespace SuggestionBoard.Core.ViewModel
{
    public interface IBaseVM
    {
        Guid Id { get; set; }
    }
    public class BaseVM : IBaseVM
    {
        public Guid Id { get; set; }
    }
}