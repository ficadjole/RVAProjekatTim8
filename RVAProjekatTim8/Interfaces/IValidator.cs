using System.Collections.Generic;

namespace RVAProjekatTim8.Validators
{
    public interface IValidator<T>
    {
        string ValidateProperty(T instance, string propertyName);

        IReadOnlyDictionary<string, string> ValidateAll(T instance);
    }
}
