using System;
using System.Reflection;

namespace BookingSystem.Core.Extensions
{
  public static class EnumExtensions
  {
    public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
    {
      if (enumVal == null) return null;

      Type type = enumVal.GetType();
      MemberInfo[] memInfo = type.GetMember(enumVal.ToString());
      object[] attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
      return (attributes.Length > 0) ? (T)attributes[0] : null;
    }
  }
}