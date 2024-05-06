using System.Globalization;
using System.Windows.Controls;

namespace NativeAudioGen.Validators
{
    public class ResourceValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string resourceName = (string)value;

            if (string.IsNullOrEmpty(resourceName))
            {
                return new ValidationResult(false, "Resource name cannot be empty.");
            }

            if(resourceName.Contains(" "))
            {
                return new ValidationResult(false, "Name can't include spaces.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
