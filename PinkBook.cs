using Godot;
using System;

public partial class PinkBook : ClickableObject
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rect = GetNode<TextureRect>("Rect");
		prompt = GetNode<Label>("Prompt");
	
		rect.Visible = false;
		prompt.Visible = false;
		clicked = false;
	}
}
