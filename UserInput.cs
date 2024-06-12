using Godot;
using System;
using System.Collections.Generic;

public partial class UserInput : LineEdit
{
	// ClickableObjects
	private Bowl bowl;
	private Butter butter;
	private Eggs eggs;
	private Flour flour;
	private Milk milk;
	private Sugar sugar;
	private Spoon spoon;
	
	private Label instructions; // top of the screen instruction text
	private Timer feedbackTimer; // timer for text
	
	private List<string> answers; // list of answers for an object
	private ClickableObject current; // current clicked object
	private bool end; // has the scene sent the signal that all objects are hidden?
	private Label congrats; // end completion text
	private int textEntered; // -1 means incorrect, 1 means correct, 0 is neutral, used for text timing
	private bool selected; // is an object currently selected (clicked)
	
	private int consecutiveWrongGuesses = 0; // how many times the use has guessed the same object wrong
	private int hintIndex = 0; // index for how many letters to give as a hint
	private ClickableObject currentSelectedSprite; // the clicked sprite atm
	private Dictionary<ClickableObject, List<string>> objectAnswers; // maps object name to answer list
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Node commonParent = GetParent();
		bowl = commonParent.GetNode<Bowl>("Bowl");
		butter = commonParent.GetNode<Butter>("Butter");
		eggs = commonParent.GetNode<Eggs>("Eggs");
		flour = commonParent.GetNode<Flour>("Flour");
		milk = commonParent.GetNode<Milk>("Milk");
		sugar = commonParent.GetNode<Sugar>("Sugar");
		spoon = commonParent.GetNode<Spoon>("Spoon");
		instructions = commonParent.GetNode<Label>("Instructions");
		feedbackTimer = commonParent.GetNode<Timer>("FeedbackTimer");
		congrats = commonParent.GetNode<Label>("Congrats");
		
		end = false;
		congrats.Visible = false;
		textEntered = 0;
		selected = false;
		
		Visible = false;
		
		instructions.Text = LanguageManager.Instance.GetLocalizedString("kitchen_instructions");
		
		// set the acceptable answers for each object
		objectAnswers = new Dictionary<ClickableObject, List<string>>()
		{
			{ bowl, new List<string> { "bol", "un bol", "le bol" } },
			{ butter, new List<string> { "beurre", "le beurre" } },
			{ eggs, new List<string> { "oeufs", "oeuf", "œufs", "des oeufs" } },
			{ flour, new List<string> { "farine", "la farine" } },
			{ milk, new List<string> { "lait", "le lait" } },
			{ sugar, new List<string> { "sucre", "le sucre" } },
			{ spoon, new List<string> { "cuillere", "la cuillere", "cuillère", "la cuillère" } }
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{	
		ClickableObject newSelectedSprite = GetSelectedSprite();
		// reset text, hintIndex, and # of wrong guesses when the player clicks on a different object
		if (currentSelectedSprite != newSelectedSprite)
		{
			currentSelectedSprite = newSelectedSprite;
			consecutiveWrongGuesses = 0;
			hintIndex = 0;
			Text = "";
		}
		
		if (currentSelectedSprite != null)
		{
			// neutral/no answer means return to instruction text
			if (textEntered == 0)
			{
				instructions.Text = LanguageManager.Instance.GetLocalizedString("kitchen_prompt");
			}
			selected = true;
			Visible = true;
			GrabFocus();
			answers = objectAnswers[currentSelectedSprite];
			current = currentSelectedSprite;
		}
		else 
		{
			selected = false;
			if (textEntered == 0)
			{
				instructions.Text = LanguageManager.Instance.GetLocalizedString("kitchen_instructions");
				Text = "";
				Visible = false;
			}
		}
	}
	// when player submits an answer
	private void _on_text_submitted(string new_text)
	{
		if (answers != null && answers.Contains(new_text.ToLower()))
		{
			textEntered = 1;
			instructions.Text = LanguageManager.Instance.GetLocalizedString("correct");
			feedbackTimer.Start(1f); 
			current.Visible = false;
			Text = "";
			Visible = false;
			consecutiveWrongGuesses = 0;
			hintIndex = 0;
		}
		else
		{
			textEntered = -1;
			Text = "";
			instructions.Text = LanguageManager.Instance.GetLocalizedString("incorrect");
			feedbackTimer.Start(1f);
			consecutiveWrongGuesses++;
			
			if (consecutiveWrongGuesses >= 3 && hintIndex < answers[0].Length)
			{
				Text = answers[0].Substring(0, hintIndex + 1);
				hintIndex++;
				CaretColumn = hintIndex+1;
			}
			else if (hintIndex >= answers[0].Length){
				Text = answers[0];
				CaretColumn = answers[0].Length + 1;
			}
		}
	}
	
	// timer for how long to show certain text
	private void _on_feedback_timer_timeout()
	{
		textEntered = 0;
		if (end)
		{
			instructions.Visible = false;
			congrats.Text = LanguageManager.Instance.GetLocalizedString("complete");
			congrats.Visible = true;
		}
		else 
		{
			if (selected == false)
			{
				instructions.Text = LanguageManager.Instance.GetLocalizedString("kitchen_instructions");
			}
		}
	}
	
	// connected method from the kitchen, that checks if all the objects are hidden/completed
	public void setAllHidden(){
		end = true;
	}
	
	// returns the selected object
	private ClickableObject GetSelectedSprite()
	{
		if (bowl.spriteClicked()){
			return bowl;
		}
		else if (butter.spriteClicked()){
			return butter;
		}
		else if (eggs.spriteClicked()){
			return eggs;
		}
		else if (flour.spriteClicked()){
			return flour;
		}
		else if (milk.spriteClicked()){
			return milk;
		}
		else if (sugar.spriteClicked()){
			return sugar;
		}
		else if (spoon.spriteClicked()){
			return spoon;
		}
		else{
			return null;
		}
	}
}



