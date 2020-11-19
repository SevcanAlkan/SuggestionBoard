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
        public string PictureUrl { get; set; }

        //Foreign Keys...

        public User()
        {

        }
    }
}
