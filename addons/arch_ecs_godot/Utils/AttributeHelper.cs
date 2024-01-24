using System;
using System.Collections.Generic;
using System.Linq;
namespace ArchEcsGodot.Utils;

public class AttributeHelper
{
   public static IEnumerable<Type> GetTypesWithHelpAttribute<TAttribute>() where TAttribute : Attribute
   {
      return typeof(TAttribute).Assembly.GetTypes().AsParallel()
         .Where(type => type.GetCustomAttributes(typeof(TAttribute), true).Length > 0);
   }
}