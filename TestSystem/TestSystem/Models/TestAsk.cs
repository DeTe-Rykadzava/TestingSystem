using System.Collections.Generic;

namespace TestSystem.Models;

public class TestAsk
{
    public int Id { get; set; }
    public string Ask { get; set; } = null!;
    public AskType Type { get; set; } = null!;
    public List<AskAnswer> Answers { get; set; } = null!;
    public AskAnswer RightAnswer { get; set; } = null!;
    public List<TestAsk>? SubAsk { get; set; } = null;
}