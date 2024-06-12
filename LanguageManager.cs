using Godot;
using System;
using System.Collections.Generic;

public partial class LanguageManager : Node
{
	private Dictionary<string, string> english = new Dictionary<string, string>();
	private Dictionary<string, string> french = new Dictionary<string, string>();
	private string currentLanguage = "English"; // default language
	
	public static LanguageManager Instance; // necessary for the other classes to have constant access to the LM
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LoadLanguageData();
		Instance = this;
	}
	
	// loads each langauge into its disctionary from its file
	private void LoadLanguageData()
	{
		LoadLanguageFile("English", "English_Text.txt", english);
		LoadLanguageFile("French", "French_Text.txt", french);
	}
	
	// reads the lines of each language file into the respective dictionary
	private void LoadLanguageFile(string language, string filePath, Dictionary<string, string> languageData)
	{
		string[] lines = System.IO.File.ReadAllLines(filePath);
		foreach (string line in lines)
		{
			string[] parts = line.Split(':');
			if (parts.Length == 2)
			{
				string key = parts[0].Trim();
				string value = parts[1].Trim();
				languageData[key] = value;
			}
			else
			{
				GD.PrintErr("Invalid line format in language file: " + line);
			}
		}
	}
	
	// switches the language
	public void SwitchLanguage(string language)
	{
		if (language.Equals("English", StringComparison.OrdinalIgnoreCase))
		{
			currentLanguage = "English";
		}
		else if (language.Equals("French", StringComparison.OrdinalIgnoreCase))
		{
			currentLanguage = "French";
		}
		else
		{
			GD.PrintErr("Language not supported: " + language);
		}
	}
	
	// returns the string in the correct language that corresponds to the prompt key requested
	public string GetLocalizedString(string key)
	{
		if (currentLanguage.Equals("English"))
		{
			if (english.ContainsKey(key))
			{
				return english[key];
			}
			else
			{
				GD.PrintErr("Localized string not found for key: " + key);
				return key;
			}
		}
		else if (currentLanguage.Equals("French"))
		{
			if (french.ContainsKey(key))
			{
				return french[key];
			}
			else
			{
				GD.PrintErr("Localized string not found for key: " + key);
				return key;
			}
		}
		else
		{
			GD.PrintErr("Unsupported language: " + currentLanguage);
			return key;
		}
	}

	// returns the dictionary that corresponds with the current language
	private Dictionary<string, string> GetCurrentLanguageData()
	{
		return currentLanguage.Equals("English") ? english : french;
	}
	
	// when the play button is pressed, switch to the first scene
	private void _on_play_pressed()
	{
		var nextScene = (PackedScene)ResourceLoader.Load("res://Kitchen.tscn");
		GetTree().ChangeSceneToPacked(nextScene);
	}
	
	// when quit is pressed, close the application
	private void _on_quit_pressed()
	{
		GetTree().Quit();
	}
}
