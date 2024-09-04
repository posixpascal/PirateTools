using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace PirateTools.Harbor.SimpleEndpoints;

public abstract class SimpleEndpoint {
    protected string route = "";
    protected IEndpointRouteBuilder? _app;
    protected IAuthenticator? authenticator;

    public void Map(string pattern, IEndpointRouteBuilder app) {
        route = pattern;
        _app = app;

        Configure();
    }

    protected abstract void Configure();

    protected void Route(string pattern) {
        route += pattern;
    }

    protected abstract Task<IResult> HandleInternal(HttpRequest request);

    protected async Task<bool> HandleAuthentication(HttpRequest request, RequestMetadata metadata) {
        if (authenticator == null)
            return true;

        return await authenticator.Authenticate(request, metadata);
    }

    protected void Authenticate(IAuthenticator authenticator) {
        this.authenticator = authenticator;
    }

    protected void AsGet() => _app?.MapGet(route, HandleInternal);
    protected void AsPost() => _app?.MapPost(route, HandleInternal);

    protected IResult Ok() => TypedResults.Ok();
    protected IResult Ok<T>(T data) => TypedResults.Ok(data);

    protected IResult Unauthorized() => TypedResults.Unauthorized();
    protected IResult Forbidden() => TypedResults.StatusCode(403);

    protected IResult BadRequest() => TypedResults.BadRequest();
    protected IResult BadRequest<T>(T data) => TypedResults.BadRequest(data);
}

public abstract class SimpleEmptyEndpoint : SimpleEndpoint {
    protected sealed override async Task<IResult> HandleInternal(HttpRequest request) {
        var metadata = new RequestMetadata();

        // Authorization
        if (!await HandleAuthentication(request, metadata))
            return Unauthorized();

        return await HandleAsync(metadata);
    }

    protected abstract Task<IResult> HandleAsync(RequestMetadata metadata);
}

public abstract class SimpleEndpoint<TRequest> : SimpleEndpoint {
    private IValidator<TRequest>? validator;

    protected sealed override async Task<IResult> HandleInternal(HttpRequest request) {
        var metadata = new RequestMetadata();

        // Authorization
        if (!await HandleAuthentication(request, metadata))
            return Unauthorized();

        // Model Binding
        var model = await request.ReadFromJsonAsync<TRequest>();
        if (model == null)
            return BadRequest();

        // Validation
        if (validator != null) {
            var result = await validator.ValidateAsync(model);

            if (!result.IsValid)
                return BadRequest();
        }

        return await HandleAsync(model, metadata);
    }

    protected abstract Task<IResult> HandleAsync(TRequest request, RequestMetadata metadata);

    protected void Validate(IValidator<TRequest> validator) {
        this.validator = validator;
    }
}

public static class WebApplicationSimpleEndpointsExtensions {
    public static WebApplication MapSimple<TEndpoint>(this WebApplication app, string route)
        where TEndpoint : SimpleEndpoint {
        ActivatorUtilities.CreateInstance<TEndpoint>(app.Services).Map(route, app);
        return app;
    }
}