using System;
using System.Collections.Generic;
using System.Text;

namespace SuggestionBoard.Core.ViewModel
{
    public class APIErrorVM 
    {
        public Guid ErrorId { get; set; }
        public Guid? RequestId { get; set; }

        public DateTime DateTime { get; set; }

        public string StackTrace { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }

        public string InnerException { get; set; }
    }
}
