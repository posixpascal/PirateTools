namespace PirateTools.Harbor.SimpleEndpoints;

public interface IAuthenticator {
    Task<bool> Authenticate(HttpRequest request, RequestMetadata metadata);
}