using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentValidationResults = FluentValidation.Results;

namespace Surging.Core.Validation.DataAnnotationValidation
{
    /// <summary>
    /// 验证class的Data Annotation属性
    /// </summary>
    public class DataAnnotationsValidation : BaseValidation
    {
        public DataAnnotationsValidation(object instance)
        {
            if (instance == null)
            {
                PrimaryErrorMessage = "The instance cannot be null!";
                ErrorMessages.Add(PrimaryErrorMessage);
                IsValid = false;
                return;
            }

            if (instance is FluentValidationResults.ValidationResult)
            {
                var validationResult = (FluentValidationResults.ValidationResult)instance;
                IsValid = validationResult.IsValid;
                if (validationResult.Errors.Any())
                {
                    ErrorMessages = validationResult.Errors.Select(p => p.ErrorMessage).ToList();
                    PrimaryErrorMessage = ErrorMessages.First();
                }

            }

            var validationContext = new ValidationContext(instance);
            var validationResults = new List<ValidationResult>();
            IsValid = Validator.TryValidateObject(instance, validationContext, validationResults, true);
            ErrorMessages = validationResults.Select(p => p.ErrorMessage).ToList();
            PrimaryErrorMessage = ErrorMessages.FirstOrDefault();
        }
    }
}
