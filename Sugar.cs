using Godot;
using System;

public partial class Sugar : ClickableObject
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rect = GetNode<TextureRect>("SugarRect");
		prompt = GetNode<Label>("SugarPrompt");
	
		rect.Visible = false;
		prompt.Visible = false;
		clicked = false;
	}
}

