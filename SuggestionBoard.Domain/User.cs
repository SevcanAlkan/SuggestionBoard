using Microsoft.AspNetCore.Identity;
using SuggestionBoard.Core.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SuggestionBoard.Domain
{
    public class User : IdentityUser<Guid>
    {
        [Range(0, int.MaxValue), DefaultValue(0)]
        public int SuggestionAmount { get; set; }
        [Range(0, int.MaxValue), DefaultValue(0)]
        public int ReactionAmount { get; set; }

        //Foreign Keys...


        public User()
        {

        }
    }
}
