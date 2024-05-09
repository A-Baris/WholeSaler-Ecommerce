namespace WholeSaler.Web.FluentValidation.Configs
{
    public interface IValidationService<T>
    {

        IEnumerable<ValidationError> GetValidationErrors(T model);


    }
}
