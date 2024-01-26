using System;
namespace ArchEcsGodot.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class EcsSystemAttribute(string worldState, int priority = 0, Type? runAfter = null) : Attribute
{
   public readonly int Priority = priority;
   public readonly Type? RunAfter = runAfter;
   public readonly string WorldState = worldState;
}