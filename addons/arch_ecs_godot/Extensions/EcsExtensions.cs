using System;
using Arch.Core;
using Arch.Core.Extensions;
using ArchEcsGodot.Components;
using ArchEcsGodot.WorldState;
using Godot;
namespace ArchEcsGodot.Extensions;

public static class EcsExtensions
{
    public static void SetElement<T>(this World world, T element) where T : notnull
    {
        ElementStorage.AddOrReplaceElement(element);
    }

    public static T GetElement<T>(this World world) where T : notnull
    {
        return ElementStorage.GetElement<T>();
    }

    public static bool TryGetElement<T>(this World world, out T? element) where T : notnull
    {
        try
        {
            element = world.GetElement<T>();
            return true;
        }
        catch (Exception)
        {
            element = default(T);
            return false;
        }
    }
    
    public static Entity SpawnFromScene<TNode>(this World world, PackedScene packedScene, out TNode tNode) where TNode : Node
    {
        var node = packedScene.Instantiate();
        tNode = (TNode)node;
        var entity = world.Create();
        entity.Add(new NodeRef(ref node));
        entity.Add(new NodeRef<TNode>(ref tNode));
        return entity;
    }

    public static void DestroyAndFree(this World world, EntityReference entityReference)
    {
        world.DestroyAndFree(entityReference.Entity);
    }
    
    public static void DestroyAndFree(this World world, Entity entity)
    {
        if (!entity.Has<NodeRef>()) return;
        entity.Get<NodeRef>().Node.QueueFree();
        world.Destroy(entity);
    }
}