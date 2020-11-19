using SuggestionBoard.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SuggestionBoard.Data.ViewModel
{
    public class ToolbarVM
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public List<SelectListVM> Categories { get; set; }

        [DefaultValue(true)]
        public bool ShowSearch { get; set; }
    }
}
