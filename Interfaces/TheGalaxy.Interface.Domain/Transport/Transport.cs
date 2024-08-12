using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using TheGalaxy.Interfaces.Domain.Users;

namespace TheGalaxy.Interfaces.Domain.Transport
{
    public class UserTransport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Number {  get; set; }

        public Guid? UserId { get; set; }

        public User User { get; set; }
    }
}
