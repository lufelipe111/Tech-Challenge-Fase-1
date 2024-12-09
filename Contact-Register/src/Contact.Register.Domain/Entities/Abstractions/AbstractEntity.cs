namespace Contact.Register.Domain.Entities.Abstractions;

public class AbstractEntity<TId>
    where TId : notnull
{
    public TId Id = default!;
}