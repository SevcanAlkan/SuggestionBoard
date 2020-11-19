using System;
using System.Collections.Generic;
using System.Text;

namespace SuggestionBoard.Data.ViewModel
{
    public class PaggingVM
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public bool IsNextPageExist { get; set; }
    }
}
