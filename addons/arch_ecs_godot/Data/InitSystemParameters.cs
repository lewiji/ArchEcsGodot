using System.Collections.Generic;
using Godot;
namespace ArchEcsGodot.Data;

public struct InitSystemParameters(Dictionary<string, Variant> data)
{
   public readonly Dictionary<string, Variant> Data = data;
}