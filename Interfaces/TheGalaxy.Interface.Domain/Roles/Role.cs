using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using TheGalaxy.Interfaces.Enums.Enums;

namespace TheGalaxy.Interfaces.Domain.Roles
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public RoleEnum Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
