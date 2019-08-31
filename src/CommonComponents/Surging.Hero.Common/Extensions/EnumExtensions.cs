using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Surging.Hero.Common.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取Enum Member的Description属性
        /// </summary>
        /// <param name="enumValue">Enum Member</param>
        /// <returns>Description</returns>
        public static string GetDescription(this Enum enumValue)
        {
            var fi = enumValue.GetType().GetField(enumValue.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;

            return enumValue.ToString();
        }
    }
}
