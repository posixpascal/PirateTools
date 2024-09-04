using FluentValidation;

namespace PirateTools.Models.AskYourChairs;

public class QuestionValidator : AbstractValidator<Question> {
	public QuestionValidator() {
		RuleFor(q => q.Title).MaximumLength(255);
		RuleFor(q => q.Content).MaximumLength(4096);
		RuleFor(q => q.EMail).MaximumLength(1024);
	}
}

public class AskQuestionRequestValidator : AbstractValidator<AskQuestionRequest> {
	public AskQuestionRequestValidator() {
		RuleFor(r => r.Token).NotNull();
		RuleFor(r => r.Question).SetValidator(new QuestionValidator());
	}
}