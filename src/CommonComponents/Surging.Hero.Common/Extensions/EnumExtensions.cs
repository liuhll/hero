using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Surging.Hero.Common.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        ///     获取Enum Member的Description属性
        /// </summary>
        /// <param name="enumValue">Enum Member</param>
        /// <returns>Description</returns>
        public static string GetDescription(this Enum enumValue)
        {
            var fi = enumValue.GetType().GetField(enumValue.ToString());
            var attributes = (DescriptionAttribute[]) fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;

            return enumValue.ToString();
        }
        
        
        public static IEnumerable<EnumDto> GetAllEnumValues(this Type @enumType)
        {
            if (!@enumType.IsEnum)
            {
                throw new ArgumentException("不是枚举类型,不允许调用该方法");
            }

            var enumDtos = new List<EnumDto>();
            var vlaues = Enum.GetValues(@enumType);
            foreach (var val in vlaues)
            {
                string valDesc = string.Empty;
                var fi = @enumType.GetField(val.ToString());
                var attributes = (DescriptionAttribute[]) fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes != null && attributes.Length > 0)
                {
                    valDesc = attributes[0].Description;
                }

                enumDtos.Add(new EnumDto() {Id = Convert.ToInt32(val), Key = val.ToString(), Description = valDesc});
            }

            return enumDtos;
        }
    }
    
}