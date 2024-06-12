using Godot;
using System;

public partial class ClickableObject : Sprite2D
{
	protected bool clicked; // is the object clicked
	protected TextureRect rect; // the object label rectangle
	protected Label prompt; // the object label text
	
	// returns true iff the object is clicked. sets clicked to false if the object is hidden
	public bool spriteClicked(){
		if (Visible == false)
		{
			clicked = false;
		}
		return clicked;
	}
	
	// handles events when the object is clicked or if the mouse is hovered over it
	public override void _Input(InputEvent @event)
	{
		// object clicked event
		if (@event is InputEventMouseButton inputEventMouse)
		{
			// check if click in in the object's hit box
			if (inputEventMouse.Pressed && inputEventMouse.ButtonIndex == MouseButton.Left)
			{
				if (GetRect().HasPoint(ToLocal(inputEventMouse.Position)))
				{
					clicked = true;
				}
				else
				{
					clicked = false;
				}
			}
		}
		// mouse over object event
		else if (@event is InputEventMouseMotion mouseMotion)
		{
			// check if mouse is over the object
			if (GetRect().HasPoint(ToLocal(mouseMotion.Position)))
			{
				rect.Visible = true;
				prompt.Visible = true;
			}
			else 
			{
				rect.Visible = false;
				prompt.Visible = false;
			}
		}
	}
}
