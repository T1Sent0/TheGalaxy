namespace TheGalaxy.Interfaces.Models
{
    public class UserTransportDto
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        public Guid? UserId { get; set; }
    }
}
