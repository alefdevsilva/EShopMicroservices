using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;
using System.Linq;
using System.Windows.Input;

namespace BuildingBlocks.Behaviors;

public class ValidationBehavior<TResquest, TResponse>
    (IEnumerable<IValidator<TResquest>> validatiors)
    : IPipelineBehavior<TResquest, TResponse>
    where TResquest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TResquest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TResquest>(request);
        var validationResults = await Task.WhenAll(validatiors.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Any())
            throw new ValidationException(failures);

        return await next();
    }
}
