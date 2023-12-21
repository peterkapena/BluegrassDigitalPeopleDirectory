using Microsoft.AspNetCore.Identity;

namespace BluegrassDigitalPeopleDirectory.Models
{
    public class AppUser : IdentityUser
    {
#nullable enable
        public DateTime? BirthDate { get; set; }
        public DateTime? DateAdded { get; set; }
        public long? Gender { get; set; }
        public string? Address { get; set; }
        public bool? ReceivePromotions { get; set; }
        public bool? ReceiveAdStatus { get; set; }
        public string? ReferalUserId { get; set; }
#nullable disable
    }
}
