using System;
using System.Collections.Generic;
using System.Text;

namespace SuggestionBoard.Core.ViewModel
{
    public interface ISaveVM : IBaseVM
    {
    }
    public class SaveVM : BaseVM, ISaveVM
    {
    }
}