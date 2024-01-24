using System.Collections.Generic;
using Godot;

namespace ArchEcsPlugin.Scenes;

[Tool]
public partial class WorldStatesEditor : Control
{
	LineEdit _newStateInput = default!;
	Button _newStateButton = default!;
	ItemList _stateItemList = default!;
	readonly HashSet<string> _stateNames = [];
	
	public override void _Ready()
	{
		_newStateInput = GetNode<LineEdit>("%NewStateInput");
		_newStateButton = GetNode<Button>("%NewStateButton");
		_stateItemList = GetNode<ItemList>("%WorldStates/%ItemList");
		PopulateStatesFromProjectSettings();
		_newStateInput.TextChanged += OnNewStateTextChanged;
		_newStateButton.Pressed += OnNewStateButtonPressed;
		OnNewStateTextChanged(_newStateInput.Text);
	}

	void PopulateStatesFromProjectSettings()
	{
		var projStates = ProjectSettings.GetSetting(ArchEcsGodotPlugin.GetProjectSettingPath("world_states"));
		if (projStates.Obj == null) return;
		foreach (var worldState in projStates.AsStringArray())
		{
			if (_stateNames.Add(worldState))
			{
				_stateItemList.AddItem(worldState);
			}
		}
	}

	void OnNewStateTextChanged(string text)
	{
		_newStateButton.Disabled = text.Length == 0;
	}

	void OnNewStateButtonPressed()
	{
		if (!_stateNames.Add(_newStateInput.Text)) return;
		_stateItemList.AddItem(_newStateInput.Text);
		var projStates = ProjectSettings.GetSetting(ArchEcsGodotPlugin.GetProjectSettingPath("world_states"));
		projStates.AsGodotArray<string>().Add(_newStateInput.Text);
		ProjectSettings.SetSetting(ArchEcsGodotPlugin.GetProjectSettingPath("world_states"), projStates);
		_newStateInput.Text = "";
		OnNewStateTextChanged("");
	}
}