using Godot;
using System;

public partial class Flour : ClickableObject
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rect = GetNode<TextureRect>("FlourRect");
		prompt = GetNode<Label>("FlourPrompt");
	
		rect.Visible = false;
		prompt.Visible = false;
		clicked = false;
	}
}

