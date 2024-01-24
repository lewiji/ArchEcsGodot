using Godot;
using Godot.Collections;
namespace ArchEcsPlugin;

[Tool]
public partial class ArchEcsGodotPlugin : EditorPlugin
{
   PackedScene _ecsPanelPackedScene = GD.Load<PackedScene>("res://addons/arch_ecs_godot/arch_ecs_plugin.tscn");
   Control? _ecsPanel;
   public static readonly string ProjectSettingsPrefix = "arch_ecs/config";
   
   public override void _EnterTree()
   {
      _ecsPanel?.QueueFree();
      _ecsPanel = _ecsPanelPackedScene.Instantiate<Control>();
      
      AddControlToBottomPanel(_ecsPanel, "ArchECS");
      AddAutoloadSingleton("ArchEcs", "res://addons/arch_ecs_godot/ArchEcsAutoload.cs");

      AddProjectSetting("world_states", new Array<string>());
   }

   public static string GetProjectSettingPath(string name)
   {
      return $"{ProjectSettingsPrefix}/{name}";
   }

   void AddProjectSetting(string name, Variant value)
   {
      var settingPath = GetProjectSettingPath(name);
      if (!ProjectSettings.HasSetting(settingPath))
      {
         ProjectSettings.SetSetting(settingPath, value);
      }
   }

   public override void _ExitTree()
   {
      if (_ecsPanel != null)
      {
         RemoveControlFromBottomPanel(_ecsPanel);
         _ecsPanel.QueueFree();
         _ecsPanel = null;
         RemoveAutoloadSingleton("ArchEcs");
      }
   }
}