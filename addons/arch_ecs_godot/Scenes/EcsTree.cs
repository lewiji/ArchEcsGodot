using Godot;

namespace ArchEcsGodot.Scenes;

public partial class EcsTree : PanelContainer
{
	
	[Export] public string Title
	{
		get => _title;
		set
		{
			_title = value;
			CallDeferred(MethodName.SetLabelTitleText);
		}
	}

	string _title = "EcsTree";
	Label? _labelTitle;

	public override void _Ready()
	{
		_labelTitle = GetNode<Label>("%ListTitle");
	}

	void SetLabelTitleText()
	{
		if (_labelTitle != null) 
			_labelTitle.Text = _title;
	}
}