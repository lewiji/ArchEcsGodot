using System;
using System.Collections.Generic;
using ArchEcsGodot.Data;
using Godot;

namespace ArchEcsGodot;

public partial class ArchEcsAutoload : Node
{
    readonly Dictionary<string, WorldState.WorldState> _worldStates = new();
    readonly LinkedList<WorldState.WorldState> _stateStack = [];

    public override void _Ready()
    {
        var worldStates = ArchEcsGodotPlugin.PopulateStatesFromProjectSettings();
        foreach (var worldState in worldStates)
        {
            CreateState(worldState, new WorldState.WorldState(worldState));
        }
    }

    public void CreateState(string name, WorldState.WorldState state, bool switchTo = false)
    {
        GD.Print(!_worldStates.TryAdd(name, state)
            ? $"Can't create {name} state: Already exists."
            : $"Created WorldState {name}");

        if (switchTo)
        {
            PushState(name);
        }
    }

    public void PushState(string name, InitSystemParameters initSystemParameters = default)
    {
        if (!_worldStates.ContainsKey(name))
        {
            GD.Print($"{name} state not found. Use `CreateState`");
            return;
        }
        
        if (_stateStack.Contains(_worldStates[name]))
        {
            GD.Print($"Can't push {name} state: Already exists on stack");
            return;
        }
        
        GD.Print($"Switching to state: {name}");
        _stateStack.First?.Value.Pause();
        _stateStack.AddFirst(_worldStates[name]);
        _worldStates[name].Start();
    }

    public void PopState()
    {
        try
        {
            _stateStack.First?.Value.Pause();
            _stateStack.RemoveFirst();
            _stateStack.First?.Value.Resume();
        }
        catch (InvalidOperationException)
        {
            GD.Print("Can't pop WorldState - stack is empty");
        }
    }

    public void DestroyState(string name)
    {
        if (!_worldStates.ContainsKey(name))
        {
           GD.Print($"WorldState {name} doesn't exist for removal.");
           return;
        }
        var state = _worldStates[name];
        if (_stateStack.First?.Value == state)
        {
            _stateStack.RemoveFirst();
            _worldStates[name].Exit();
            _stateStack.First?.Value.Resume();
        }
        else
        {
            _stateStack.Remove(state);
            _worldStates[name].Exit();
        }
        _worldStates.Remove(name);
    }
    
    public override void _Process(double delta)
    {
        _stateStack.First?.Value.TickProcess(delta);
    }
   
    public override void _PhysicsProcess(double delta)
    {
        _stateStack.First?.Value.TickPhysics(delta);
    }
}