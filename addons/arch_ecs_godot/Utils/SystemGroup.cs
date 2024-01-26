using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Arch.System;
using ArchEcsGodot.Attributes;
using Medallion.Collections;

namespace ArchEcsGodot.Utils;

public class SystemGroup<T>(List<ISystem<T>>? systems = null)
{
    readonly List<ISystem<T>> _systems = systems ?? [];
    public IEnumerable<ISystem<T>> Systems => _systems;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddSystem(ISystem<T> system)
    {
        _systems.Add(system);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddSystems(IEnumerable<ISystem<T>> addSystems)
    {
        _systems.AddRange(addSystems);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void RemoveSystem(ISystem<T> system)
    {
        _systems.Remove(system);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Tick(T updateValue)
    {
        foreach (var s in _systems)
        {
            s.BeforeUpdate(updateValue);
            s.Update(updateValue);
            s.AfterUpdate(updateValue);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void InitializeSystems()
    {
        foreach (var s in _systems)
        {
            s.Initialize();
        }
    }
}