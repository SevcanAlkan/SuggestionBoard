using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SuggestionBoard.Core.ViewModel
{
    public interface IIsResultVM
    {
        bool Result { get; set; }
    }

    public class APIResultVM : IIsResultVM
    {
        public Guid? RecId { get; set; }

        public object Rec { get; set; }
        public bool Result { get; set; }
        [JsonIgnore]
        public List<APIErrorVM> Errors { get; set; }
    }
}