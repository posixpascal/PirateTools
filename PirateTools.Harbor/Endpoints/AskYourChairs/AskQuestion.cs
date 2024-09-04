using PirateTools.Harbor.Authenticators;
using PirateTools.Harbor.Services;
using PirateTools.Models.AskYourChairs;

namespace PirateTools.Harbor.Endpoints.AskYourChairs;

public class AskQuestion : SimpleEndpoint<Question> {
    private readonly DBService _dbService;

    public AskQuestion(DBService dbService) {
        _dbService = dbService;
    }

    protected override void Configure() {
        Route("/AskQuestion");
        AsPost();
        Validate(new QuestionValidator());
        Authenticate(new AskYourChairsTokenAuthenticator(_dbService));
    }

    protected override Task<IResult> HandleAsync(Question question, RequestMetadata metadata) {
        _dbService.AddQuestion(metadata.GetString("AuthToken"), question);
        return Task.FromResult(Ok());
    }
}