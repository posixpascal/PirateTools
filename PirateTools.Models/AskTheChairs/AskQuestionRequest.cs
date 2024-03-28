namespace PirateTools.Models.AskTheChairs;

public class AskQuestionRequest {
    public required string Token { get; set; }
    public required Question Question { get; set; }
}