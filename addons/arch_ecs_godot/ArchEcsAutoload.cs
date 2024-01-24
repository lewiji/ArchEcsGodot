using System;
using System.Collections.Generic;
using ArchEcsGodot.Data;
using Godot;

namespace ArchEcsPlugin;

public partial class ArchEcsAutoload : Node
{
    readonly Dictionary<string, WorldState.WorldState> _worldStates = new();
    readonly LinkedList<WorldState.WorldState> _stateStack = [];

    public void PushState(string name, InitSystemParameters initSystemParameters = default)
    {
        if (!_worldStates.ContainsKey(name))
        {
            var newState = new WorldState.WorldState(initSystemParameters);
            _worldStates.Add(name, newState);
        }
        
        if (_stateStack.Contains(_worldStates[name]))
        {
            GD.Print($"Can't push {name} state: Already exists on stack");
            return;
        }
        
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
            throw new ArgumentException($"WorldState {name} doesn't exist for removal.");
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