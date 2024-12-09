namespace Contact.Register.Domain.ValueObjects;

public class Phone(int ddd, int number)
{
    public int Ddd { get; set; } = ddd;
    public int Number { get; set; } = number;
}