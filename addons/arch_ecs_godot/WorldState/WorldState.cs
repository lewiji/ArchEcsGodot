using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Arch.Core;
using Arch.System;
using ArchEcsGodot.Attributes;
using ArchEcsGodot.Data;
using ArchEcsGodot.Utils;
using Medallion.Collections;

namespace ArchEcsGodot.WorldState;

public class WorldState
{
   readonly World _world = World.Create();
   readonly SystemGroup<InitSystemParameters> _initSystems = new();
   readonly SystemGroup<double> _processSystems = new();
   readonly SystemGroup<double> _physicsSystems = new();
   readonly SystemGroup<double> _pauseSystems = new();
   readonly SystemGroup<double> _resumeSystems = new();
   readonly SystemGroup<double> _exitSystems = new();
   readonly InitSystemParameters _initSystemParameters;
   public readonly string Name;

   public WorldState(string name, InitSystemParameters initSystemParameters = default)
   {
      _initSystemParameters = initSystemParameters;
      Name = name;
      AddSystemsByAttribute<ProcessSystemAttribute, double>(_processSystems);
      AddSystemsByAttribute<PhysicsSystemAttribute, double>(_physicsSystems);
      AddSystemsByAttribute<PauseSystemAttribute, double>(_pauseSystems);
      AddSystemsByAttribute<ResumeSystemAttribute, double>(_resumeSystems);
      AddSystemsByAttribute<ExitSystemAttribute, double>(_exitSystems);
      AddSystemsByAttribute<InitSystemAttribute, InitSystemParameters>(_initSystems);
   }

   public void Start()
   {
      _initSystems.Tick(_initSystemParameters);
   }

   public void Exit()
   {
      _exitSystems.Tick(0);
   }

   public void Pause()
   {
      _pauseSystems.Tick(0);
   }

   public void Resume()
   {
      _resumeSystems.Tick(0);
   }

   public void TickProcess(double delta)
   {
      _processSystems.Tick(delta);
   }

   public void TickPhysics(double delta)
   {
      _physicsSystems.Tick(delta);
   }
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   void AddSystemsByAttribute<TAttribute, TParam>(SystemGroup<TParam> systemGroup) where TAttribute : EcsSystemAttribute
   {
      var attributeSystemTypes = AttributeHelper.GetTypesWithHelpAttribute<TAttribute>().ToArray();
      
      var sorted = attributeSystemTypes
         .Where(ats => ats.GetCustomAttribute<TAttribute>()!.WorldState == Name)
         .OrderByDescending(ats => ats.GetCustomAttribute<TAttribute>()!.Priority)
         .StableOrderTopologicallyBy(ats => {
            var runAfter = ats.GetCustomAttribute<TAttribute>()!.RunAfter;
            return runAfter != null 
               ? [attributeSystemTypes.First(other => runAfter == other)] 
               : Enumerable.Empty<Type>();
         });

      var systems = sorted
         .Select(pt => Activator.CreateInstance(pt, _world))
         .OfType<ISystem<TParam>>();
      systemGroup.AddSystems(systems);
   }
}