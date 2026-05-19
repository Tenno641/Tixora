namespace Users.Domain.Users;

public sealed class Role
{
    public readonly static Role Administrator = new("Administrator");
    public readonly static Role Member = new("Member");

    private Role(string name)
    {
        Name = name;
    }

    private Role() { }

    public string Name { get; private set; }
}
