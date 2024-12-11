namespace ContactRegister.Domain.Entities.Abstractions;

public interface IAbstractEntity
{
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}