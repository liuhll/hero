using FluentValidation.Results;

namespace Surging.Core.Validation.DataAnnotationValidation
{

    /// <summary>
    /// 验证DTO，INPUT对象
    /// </summary>
    public static class InputValidationExtensions
    {
       

        /// <summary>
        /// Class的Data Annotation属性验证
        /// </summary>
        /// <param name="instance">class</param>
        /// <returns></returns>
        public static BaseValidation DataAnnotationsCheck(this object instance)
        {
            return new DataAnnotationsValidation(instance);
        }

        public static BaseValidation DataAnnotationsCheck(this ValidationResult validationResult)
        {
            return new DataAnnotationsValidation(validationResult);
        }
    }
}
