namespace RehabApp.Backend.Models
{
    public class Injury
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public DateTime CreatedUtc { get; init; }
        public DateTime UpdatedUtc { get; init; }
    }
}
