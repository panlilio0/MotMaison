using Godot;
using System;

public partial class kitchen : Node2D
{
	private Sprite2D cake;
	private bool allHidden; // boolean for all of the clicable objects being hidden
	private UserInput input;
	private TextureButton txtbt; // next scene arrow button
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cake = GetNode<Sprite2D>("Cake");
		input = GetNode<UserInput>("UserInput");
		txtbt = GetNode<TextureButton>("TextureButton");
		
		cake.Visible = false;
		txtbt.Visible = false;
		allHidden = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		CheckIfDone();
	}
	
	// checks to see if the user has completed the scene
	public void CheckIfDone(){
		bool allHidden = true;
		foreach (Node child in GetChildren()){
			if (child is ClickableObject sprite)
			{
				// check if the child is visible
				if (sprite.Visible)
				{
					// if any ClickableObject is visible, set allHidden to false and break out of the loop
					allHidden = false;
					break;
				}
			}
		}
		// when player competes scene, display end objects
		if (allHidden)
		{
			cake.Visible = true;
			txtbt.Visible = true;
			input.setAllHidden();
		}
	}
	// switch to next scene when arrow button is pressed
	private void _on_texture_button_pressed()
	{
		var nextScene = (PackedScene)ResourceLoader.Load("res://Living Room.tscn");
		GetTree().ChangeSceneToPacked(nextScene);
	}
}
