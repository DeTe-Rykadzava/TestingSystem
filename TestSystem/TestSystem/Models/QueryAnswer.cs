using System;
using System.Threading.Tasks;
using Splat;
using TestSystem.ViewModels;

namespace TestSystem.Models;

public class QueryAnswer
{
    public int Id { get; set; }
    public string AnswerValue { get; set; } = null!;

    public bool IsCorrect { get; set; }

    private QueryAnswer(string answerValue)
    {
        AnswerValue = answerValue;
    }
}