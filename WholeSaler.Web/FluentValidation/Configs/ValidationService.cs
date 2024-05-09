
using FluentValidation;

namespace WholeSaler.Web.FluentValidation.Configs
{
    public class ValidationService<T> : IValidationService<T>
    {
        private readonly IValidator<T> _validator;

        public ValidationService(IValidator<T> validator)
        {
            _validator = validator;
        }
        public IEnumerable<ValidationError> GetValidationErrors(T model)
        {
            var validationResult = _validator.Validate(model);

            if (!validationResult.IsValid)
            {
                return validationResult.Errors.Select(failure => new ValidationError
                {
                    PropertyName = failure.PropertyName,
                    ErrorMessage = failure.ErrorMessage
                });
            }

            return Enumerable.Empty<ValidationError>();
        }
    }
}
