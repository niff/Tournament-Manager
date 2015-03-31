using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace IglaClub.Web.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayAttributeFrom(this Enum enumValue, Type enumType)
        {
            return enumType.GetMember(enumValue.ToString())
                           .First()
                           .GetCustomAttribute<DisplayAttribute>()
                           .Name;
        }
    }
}