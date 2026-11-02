using MediatR;

namespace Application.Common.Behaviors;

using FluentValidation;


public sealed class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResult
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        var context = new ValidationContext<TRequest>(request);

        var failures = (await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, ct))))
            .SelectMany(r => r.Errors)
            .Where(e => e is not null)
            .ToList();

        if (failures.Count == 0)
            return await next(ct);

        var errors = failures.ConvertAll(error =>
            Error.Validation(
                code: error.PropertyName,
                description: error.ErrorMessage));

        // Convert errors into your application's failure result here.
        // The conversion depends on your IResult implementation.
        return (dynamic)errors;
    }
}

