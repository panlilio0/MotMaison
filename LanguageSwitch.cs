using Godot;
using System;

public partial class LanguageSwitch : TextureButton
{
	private bool isFrench;
	private LanguageManager languageManager;
	private Label label;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Node commonParent = GetParent();
		languageManager = commonParent.GetNode<LanguageManager>("LanguageManager");
		label = GetNode<Label>("Label");
		isFrench = false;
	}
	
	private void _on_pressed()
	{
		isFrench = !isFrench; // switches the language

		if (isFrench)
		{
			languageManager.SwitchLanguage("French");
			label.Text = "Prompts: French";
		}
		else
		{
			languageManager.SwitchLanguage("English");
			label.Text = "Prompts: English";
		}
	}
}
