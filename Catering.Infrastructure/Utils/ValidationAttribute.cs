using System.ComponentModel.DataAnnotations;

namespace Catering.Infrastructure.Utils
{
    public class ValidEnumValueAttribute : ValidationAttribute
    {
        private readonly Type enumType;

        public ValidEnumValueAttribute(Type _enumType)
        {
            enumType = _enumType;
        }

        public override bool IsValid(object? value)
        {
            if (value == null) return true;
            return Enum.IsDefined(enumType, value);
        }
    }

}
