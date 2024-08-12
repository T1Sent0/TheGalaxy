using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using TheGalaxy.Interfaces.Domain.Roles;
using TheGalaxy.Interfaces.Domain.Transport;

namespace TheGalaxy.Interfaces.Domain.Users
{

    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    
        public string MiddleName { get; set; }
    
        public string Email { get; set; }

        public string Password { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        public Guid? TranportId { get; set; }

        public UserTransport Transport { get; set; }
    }
}
