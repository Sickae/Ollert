using System;
using System.ComponentModel;
using System.Linq;

namespace Ollert.DataAccess.Helpers
{
    /// <summary>
    /// Enumokhoz segítő műveletek
    /// </summary>
    public static class EnumHelper
    {
        public static string GetName(this Enum value)
        {
            return Enum.GetName(value.GetType(), value);
        }

        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());

            if (memberInfo.Length <= 0)
            {
                return value.ToString();
            }

            var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0
                ? ((DescriptionAttribute)attributes[0]).Description
                : value.ToString();
        }

        public static bool IsIn<T>(this T value, params T[] enums) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enum type!");
            }

            return enums.Contains(value);
        }

        public static bool IsNullableEnum(this Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);
            return underlyingType != null && underlyingType.IsEnum;
        }
    }
}
