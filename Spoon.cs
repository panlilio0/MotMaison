using Godot;
using System;

public partial class Spoon : ClickableObject
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rect = GetNode<TextureRect>("SpoonRect");
		prompt = GetNode<Label>("SpoonPrompt");
	
		rect.Visible = false;
		prompt.Visible = false;
		clicked = false;
	}
}

