using Arch.Core;
using ArchEcsGodot.Data;
using ArchEcsGodot.Utils;
using Godot;
namespace ArchEcsPlugin.WorldState;

public partial class WorldState(InitSystemParameters initSystemParameters = default)
{
   public readonly World World = World.Create();
   readonly SystemGroup<InitSystemParameters> _initSystems = new();
   readonly SystemGroup<double> _processSystems = new();
   readonly SystemGroup<double> _physicsSystems = new();
   readonly SystemGroup<double> _pauseSystems = new();
   readonly SystemGroup<double> _resumeSystems = new();
   readonly SystemGroup<double> _exitSystems = new();
   public void Start()
   {
      _initSystems.Tick(initSystemParameters);
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
}