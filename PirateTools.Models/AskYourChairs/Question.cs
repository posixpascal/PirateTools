using System;

namespace PirateTools.Models.AskYourChairs;

public class Question {
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";

    /// <summary>Optional if the person wants an immediate answer via E-Mail</summary>
    public string? EMail { get; set; }

    public DateTime AskedDateTime { get; set; }

    public Question Clone() {
        return new Question {
            Title = Title,
            Content = Content,
            EMail = EMail,
            AskedDateTime = AskedDateTime
        };
    }
}