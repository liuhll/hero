using FluentValidation.Results;
using Surging.Core.CPlatform.Exceptions;
using System.Linq;
using System.Text;

namespace Surging.Core.Validation.DataAnnotationValidation
{
    public static class InputValidationExtensions
    {
        public static void CheckValidResult(this ValidationResult validationResult, bool isAll = false)
        {
            if (!validationResult.IsValid)
            {
                string errorMessage = string.Empty;
                if (isAll)
                {
                    var sb = new StringBuilder();
                    foreach (var error in validationResult.Errors)
                    {
                        sb.Append(error.ErrorMessage + "|");
                    }
                    errorMessage = sb.ToString().Remove(sb.Length - 1);
                }
                else
                {
                    errorMessage = validationResult.Errors.First().ErrorMessage;
                }

                throw new ValidateException(errorMessage);
            }
        }

        public static BaseValidation CheckDataAnnotations(this object instance)
        {
            return new DataAnnotationsValidation(instance);
        }

        public static void CheckValidResult(this BaseValidation validation, bool isAll = false)
        {
            if (!validation.IsValid)
            {
                string errorMessage = string.Empty;
                if (isAll)
                {
                    var sb = new StringBuilder();
                    foreach (var error in validation.ErrorMessages)
                    {
                        sb.Append(error + "|");
                    }
                    errorMessage = sb.ToString().Remove(sb.Length - 1);
                }
                else
                {
                    errorMessage = validation.PrimaryErrorMessage;
                }

                throw new ValidateException(errorMessage);
            }
        }
    }
}
