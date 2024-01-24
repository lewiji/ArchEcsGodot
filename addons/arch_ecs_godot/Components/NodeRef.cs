using Godot;
namespace ArchEcsGodot.Components;

public readonly struct NodeRef(ref Node node)
{
   public Node Node { get; } = node;
}

public readonly struct NodeRef<T>(ref T node) where T : Node
{
   public T Node { get; } = node;
}