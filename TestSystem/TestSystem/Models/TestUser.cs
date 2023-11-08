namespace TestSystem.Models;

public class TestUser
{
    public int Id { get; set; }
    public Test Test { get; set; } = null!;
    public User User { get; set; } = null!;
    public int RightAnswersCount { get; set; }
}