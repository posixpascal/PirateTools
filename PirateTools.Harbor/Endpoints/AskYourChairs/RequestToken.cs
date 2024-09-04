using PirateTools.Harbor.Services;

namespace PirateTools.Harbor.Endpoints.AskYourChairs;

public class RequestToken : SimpleEndpoint<string> {
    private readonly DBService _dbService;
    private readonly SmtpService _smtpService;

    public RequestToken(DBService dbService, SmtpService smtpService) {
        _dbService = dbService;
        _smtpService = smtpService;
    }

    protected override void Configure() {
        Route("/RequestToken");
        AsPost();
    }

    protected override Task<IResult> HandleAsync(string email, RequestMetadata metadata) {
        var token = _dbService.GenerateToken();
        _smtpService.SendMail(email, "Ask your BuVo Token",
            $"Hier ist dein Ask your BuVo Token:\n{token}");

        return Task.FromResult(Ok());
    }
}