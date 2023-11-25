using System;
using System.Threading.Tasks;
using Splat;
using TestSystem.ViewModels;

namespace TestSystem.Models;

public class QueryAnswer
{
    public int Id { get; set; }
    public string Answer { get; set; } = null!;

    public bool IsCorrect { get; set; }

    private QueryAnswer()
    {
        
    }

    private QueryAnswer(string answerValue)
    {
        Answer = answerValue;
    }
}