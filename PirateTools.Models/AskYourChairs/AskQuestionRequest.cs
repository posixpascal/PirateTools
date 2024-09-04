namespace PirateTools.Models.AskYourChairs;

public class AskQuestionRequest {
    public required string Token { get; set; }
    public required Question Question { get; set; }
}