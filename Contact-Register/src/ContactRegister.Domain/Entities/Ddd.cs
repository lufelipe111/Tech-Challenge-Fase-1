using ContactRegister.Domain.Entities.Abstractions;

namespace ContactRegister.Domain.Entities;

public class Ddd : AbstractEntity<int>
{
    public int Code { get; set; }
	public string State { get; set; }
	public string Region { get; set; }

    public Ddd() { }
    
    public Ddd(int code, string state, string region)
    {
        Code = code;
        State = state;
        Region = region;
    }
}