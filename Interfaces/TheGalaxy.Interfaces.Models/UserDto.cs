using TheGalaxy.Interfaces.Enums.Enums;

namespace TheGalaxy.Interfaces.Models
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string Email { get; set; }

        public string CarNumber { get; set; }

        public RoleEnum Role { get; set; }
    }
}
