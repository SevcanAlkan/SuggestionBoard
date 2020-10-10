using System;
using System.Collections.Generic;
using System.Text;

namespace SuggestionBoard.Core.Enum
{
    public enum SuggestionStatus : sbyte
    {
        Created = 1,
        InVoiting = 2,
        Completed = 3,
        Cancelled = 4
    }
}
