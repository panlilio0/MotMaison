using Godot;
using System;

public partial class living_room : Node2D
{
	private Sprite2D red;
	private Sprite2D orange;
	private Sprite2D yellow;
	private Sprite2D green;
	private Sprite2D blue;
	private Sprite2D purple;
	private Sprite2D pink;
	private Sprite2D brown;
	private UserInputLR input;
	
	// called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		red = GetNode<Sprite2D>("RedBook");
		orange = GetNode<Sprite2D>("OrangeBook");
		yellow = GetNode<Sprite2D>("YellowBook");
		green = GetNode<Sprite2D>("GreenBook");
		blue = GetNode<Sprite2D>("BlueBook");
		purple = GetNode<Sprite2D>("PurpleBook");
		pink = GetNode<Sprite2D>("PinkBook");
		brown = GetNode<Sprite2D>("BrownBook");
		
		input = GetNode<UserInputLR>("UserInputLR");
	}

	// called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// if none of the ClickableObjects are visible, send signal that the 
		// player has finished the scene to the UserInput
		if (red.Visible == false && orange.Visible == false 
			&& yellow.Visible == false && green.Visible == false 
			&& blue.Visible == false && purple.Visible == false 
			&& pink.Visible == false && brown.Visible == false)
		{
			input.setAllHidden();
		}
	}
}
