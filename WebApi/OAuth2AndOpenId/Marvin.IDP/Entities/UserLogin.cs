using System.ComponentModel.DataAnnotations;

namespace Marvin.IDP.Entities
{
    public class UserLogin : IConcurrencyAware
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        [MaxLength(200)]
        [Required]
        public string Provider { get; set; } // external provider reference to link our user.

        [MaxLength(200)]
        [Required]
        public string ProviderIdentityKey { get; set; } // key of the user at the level of the external IDP


        public string ConcurrencyStamp { get; set; }
    }
}
