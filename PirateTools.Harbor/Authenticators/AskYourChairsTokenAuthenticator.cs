using PirateTools.Harbor.Services;

namespace PirateTools.Harbor.Authenticators;

public class AskYourChairsTokenAuthenticator : IAuthenticator {
    private readonly DBService _dbService;

    public AskYourChairsTokenAuthenticator(DBService dbService) {
        _dbService = dbService;
    }

    public Task<bool> Authenticate(HttpRequest request, RequestMetadata metadata) {
        if (!request.Headers.TryGetValue("AuthToken", out var token))
            return Task.FromResult(false);

        var usesLeft = _dbService.CheckToken(token.ToString());
        metadata.Set("AuthTokenUsesLeft", usesLeft);
        metadata.Set("AuthToken", token.ToString());

        return Task.FromResult(usesLeft > 0);
    }
}