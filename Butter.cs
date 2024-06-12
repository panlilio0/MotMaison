using Godot;
using System;

public partial class Butter : ClickableObject
{	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rect = GetNode<TextureRect>("ButterRect");
		prompt = GetNode<Label>("ButterPrompt");
	
		rect.Visible = false;
		prompt.Visible = false;
		clicked = false;
	}
}

