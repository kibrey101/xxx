using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace FriendOrganizer.Model
{
    public class Friend
    {
        [Key]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false), StringLength(60)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(50), EmailAddress]
        public string Email { get; set; }
        public int? ProgrammingLanguageId { get; set; }
        public ProgrammingLanguage ProgrammingLanguage { get; set; }
        public ICollection<FriendPhoneNumber> PhoneNumbers { get; set; } = new Collection<FriendPhoneNumber>();
    }
}
