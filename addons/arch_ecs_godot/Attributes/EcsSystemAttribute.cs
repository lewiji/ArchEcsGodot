using System;
namespace ArchEcsGodot.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class EcsSystemAttribute(int priority = 0, Type? runAfter = null) : Attribute
{
   public int Priority { get; set; } = priority;
   public Type? RunAfter { get; set; } = runAfter;
}