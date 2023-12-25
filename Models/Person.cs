using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BluegrassDigitalPeopleDirectory.Models
{
    public class Person : CommonModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public Address PAddress { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string MobileNumber { get; set; }
        public string Gender { get; set; }
        public string EmailAddress { get; set; }
        public class Address : CommonModel
        {
            public string Country { get; set; }
            public string City { get; set; }
        }
    }

}
