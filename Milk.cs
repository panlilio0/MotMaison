using Godot;
using System;

public partial class Milk : ClickableObject
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rect = GetNode<TextureRect>("MilkRect");
		prompt = GetNode<Label>("MilkPrompt");
	
		rect.Visible = false;
		prompt.Visible = false;
		clicked = false;
	}
}

