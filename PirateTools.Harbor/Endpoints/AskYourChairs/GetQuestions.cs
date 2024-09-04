using PirateTools.Harbor.Services;

namespace PirateTools.Harbor.Endpoints.AskYourChairs;

public class GetQuestions : SimpleEmptyEndpoint {
    private readonly DBService _dbService;

    public GetQuestions(DBService dbService) {
        _dbService = dbService;
    }

    protected override void Configure() {
        Route("/GetQuestions");
        AsGet();
    }

    protected override Task<IResult> HandleAsync(RequestMetadata metadata)
        => Task.FromResult(Ok(_dbService.GetQuestions()));
}