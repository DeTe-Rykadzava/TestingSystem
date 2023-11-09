using System.Collections.Generic;
using System.Linq;
using Splat;
using TestSystem.ViewModels;

namespace TestSystem.Models;

public class Test
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public User CreatorUser { get; set; } = null!;
    public List<TestAsk> Asks { get; set; } = null!;

    private Test(User creatorUser)
    {
        CreatorUser = creatorUser;
    }

    public static TestViewModel CreateNewBlackTest()
    {
        var userId = User.GetCurrentUser().UserId;
        var user = Locator.GetLocator().GetService<TestingSystemDbContext>()!.User.FirstOrDefault(x => x.Id == userId)!;
        return new TestViewModel(new Test(user));
    }

}