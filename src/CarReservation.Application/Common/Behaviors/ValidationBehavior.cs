namespace CarReservation.Application.Common.Behaviors;

using FluentValidation;
using MediatR;
using CarReservation.Domain.Common;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                var errors = string.Join("; ", failures.Select(f => f.ErrorMessage));
                
                // Handle Result<T> response types
                if (typeof(TResponse).IsGenericType && 
                    typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
                {
                    var resultType = typeof(Result<>).MakeGenericType(
                        typeof(TResponse).GetGenericArguments()[0]);
                    var failureMethod = resultType.GetMethod("Failure", 
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    return (TResponse)failureMethod!.Invoke(null, new object[] { errors })!;
                }
                
                // Handle Result response type
                if (typeof(TResponse) == typeof(Result))
                {
                    return (TResponse)(object)Result.Failure(errors);
                }
            }
        }

        return await next();
    }
}
