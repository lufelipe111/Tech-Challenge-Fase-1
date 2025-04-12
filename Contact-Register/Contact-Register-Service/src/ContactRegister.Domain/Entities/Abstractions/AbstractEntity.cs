namespace ContactRegister.Domain.Entities.Abstractions;

public class AbstractEntity<TId> : IAbstractEntity
    where TId : notnull
{
    public TId Id { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}