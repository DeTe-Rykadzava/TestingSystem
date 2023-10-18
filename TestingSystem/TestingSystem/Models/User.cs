namespace TestingSystem.Models;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string SecondName { get; set; } = null!;
    public string? LastName { get; set; }
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public Role UserRole { get; set; } = null!;
}