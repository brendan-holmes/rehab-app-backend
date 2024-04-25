namespace RehabApp.Backend.Models;

public class InjuryDto
{
    public Guid Id { get; init; }
    public DateTime CreateUtc { get; init; }
    public DateTime UpdatedUtc { get; init; }
    public string? Name { get; init; }
}
