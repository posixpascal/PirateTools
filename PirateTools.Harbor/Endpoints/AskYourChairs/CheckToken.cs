using PirateTools.Harbor.Authenticators;
using PirateTools.Harbor.Services;

namespace PirateTools.Harbor.Endpoints.AskYourChairs;

public class CheckToken : SimpleEmptyEndpoint {
    private readonly DBService _dbService;

    public CheckToken(DBService dbService) {
        _dbService = dbService;
    }

    protected override void Configure() {
        Route("/CheckToken");
        AsGet();
        Authenticate(new AskYourChairsTokenAuthenticator(_dbService));
    }

    protected override Task<IResult> HandleAsync(RequestMetadata metadata)
        => Task.FromResult(Ok(metadata.GetInt("AuthTokenUsesLeft")));
}