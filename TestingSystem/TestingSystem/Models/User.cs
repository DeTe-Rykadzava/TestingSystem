namespace TestingSystem.Models;

public class User
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string SecondName { get; set; }
    public string LastName { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public Role UserRole { get; set; }
}