﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BluegrassDigitalPeopleDirectory.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        [NotMapped]
        public string PassWord { get; set; }
    }
}
