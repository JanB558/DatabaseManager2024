using System.ComponentModel.DataAnnotations;

namespace ASPWebAPI.Validation
{
    public class ValidationFilter<T> : IEndpointFilter where T : class
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            // Find the first argument matching the generic type T
            var argument = context.Arguments.FirstOrDefault(a => a is T) as T;
            if (argument is null)
                return Results.BadRequest("Invalid input data");

            // Perform validation
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(argument, serviceProvider: null, items: null);

            if (!Validator.TryValidateObject(argument, validationContext, validationResults, true))
            {
                var errors = validationResults.Select(vr => vr.ErrorMessage).ToList();
                return Results.BadRequest(new { Errors = errors });
            }

            // Proceed to the next step in the pipeline
            return await next(context);
        }
    }
}
