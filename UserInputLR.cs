using Godot;
using System;
using System.Collections.Generic;

public partial class UserInputLR : LineEdit
{
	// ClickableObjects
	private RedBook red;
	private OrangeBook orange;
	private YellowBook yellow;
	private GreenBook green;
	private BlueBook blue;
	private PurpleBook purple;
	private PinkBook pink;
	private BrownBook brown;
	
	// Final position books, "completion objects"
	private Sprite2D redfin;
	private Sprite2D orangefin;
	private Sprite2D yellowfin;
	private Sprite2D greenfin;
	private Sprite2D bluefin;
	private Sprite2D purplefin;
	private Sprite2D pinkfin;
	private Sprite2D brownfin;
	
	private Label instructions; // top of the screen instruction text
	private Timer feedbackTimer; // timer for text
	
	private List<string> answers; // list of answers for an object
	private ClickableObject current; // current clicked object
	private Sprite2D fin; // final position object
	private bool end; // has the scene sent the signal that all objects are hidden?
	private Label congrats; // end completion text
	private int textEntered; // -1 means incorrect, 1 means correct, 0 is neutral, used for text timing
	private bool selected; // is an object currently selected (clicked)
	
	private int consecutiveWrongGuesses = 0; // how many times the player has guessed the same object wrong
	private int hintIndex = 0; // index for how many letters to give as a hint
	private ClickableObject currentSelectedSprite; // the clicked sprite atm
	private Dictionary<ClickableObject, List<string>> objectAnswers; // maps object name to answer list
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Node commonParent = GetParent();
		
		red = commonParent.GetNode<RedBook>("RedBook");
		orange = commonParent.GetNode<OrangeBook>("OrangeBook");
		yellow = commonParent.GetNode<YellowBook>("YellowBook");
		green = commonParent.GetNode<GreenBook>("GreenBook");
		blue = commonParent.GetNode<BlueBook>("BlueBook");
		purple = commonParent.GetNode<PurpleBook>("PurpleBook");
		pink = commonParent.GetNode<PinkBook>("PinkBook");
		brown = commonParent.GetNode<BrownBook>("BrownBook");
		
		redfin = commonParent.GetNode<Sprite2D>("RedFinal");
		orangefin = commonParent.GetNode<Sprite2D>("OrangeFinal");
		yellowfin = commonParent.GetNode<Sprite2D>("YellowFinal");
		greenfin = commonParent.GetNode<Sprite2D>("GreenFinal");
		bluefin = commonParent.GetNode<Sprite2D>("BlueFinal");
		purplefin = commonParent.GetNode<Sprite2D>("PurpleFinal");
		pinkfin = commonParent.GetNode<Sprite2D>("PinkFinal");
		brownfin = commonParent.GetNode<Sprite2D>("BrownFinal");
		
		instructions = commonParent.GetNode<Label>("InstructionsLR");
		feedbackTimer = commonParent.GetNode<Timer>("FeedbackTimerLR");
		congrats = commonParent.GetNode<Label>("CongratsLR");
		
		redfin.Visible = false;
		orangefin.Visible = false;
		yellowfin.Visible = false;
		greenfin.Visible = false;
		bluefin.Visible = false;
		purplefin.Visible = false;
		pinkfin.Visible = false;
		brownfin.Visible = false;
		
		end = false;
		congrats.Visible = false;
		textEntered = 0;
		selected = false;
		
		instructions.Text = LanguageManager.Instance.GetLocalizedString("living_room_instructions");
		
		Visible = false;
		
		// set the acceptable answers for each object
		objectAnswers = new Dictionary<ClickableObject, List<string>>()
		{
			{ red, new List<string> { "rouge", "roux", "le rouge" } },
			{ orange, new List<string> { "orange", "l'orange" } },
			{ yellow, new List<string> { "jaune", "le jaune" } },
			{ green, new List<string> { "vert", "verte", "le verte" } },
			{ blue, new List<string> { "bleu", "bleue", "le bleu" } },
			{ purple, new List<string> { "violet", "violette", "le violet" } },
			{ pink, new List<string> { "rose", "la rose" } },
			{ brown, new List<string> { "brun", "marron", "le brun", "le marron" } }
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
				instructions.Text = LanguageManager.Instance.GetLocalizedString("living_room_prompt");
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
				instructions.Text = LanguageManager.Instance.GetLocalizedString("living_room_instructions");
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
			fin.Visible = true;
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
				instructions.Text = LanguageManager.Instance.GetLocalizedString("living_room_instructions");
			}
		}
	}
	
	// connected method from the living room, that checks if all the objects are hidden/completed
	public void setAllHidden(){
		end = true;
	}
	
	// returns the selected object
	private ClickableObject GetSelectedSprite()
	{
		if (red.spriteClicked()){
			fin = redfin;
			return red;
		}
		else if (orange.spriteClicked()){
			fin = orangefin;
			return orange;
		}
		else if (yellow.spriteClicked()){
			fin = yellowfin;
			return yellow;
		}
		else if (green.spriteClicked()){
			fin = greenfin;
			return green;
		}
		else if (blue.spriteClicked()){
			fin = bluefin;
			return blue;
		}
		else if (purple.spriteClicked()){
			fin = purplefin;
			return purple;
		}
		else if (pink.spriteClicked()){
			fin = pinkfin;
			return pink;
		}
		else if (brown.spriteClicked()){
			fin = brownfin;
			return brown;
		}
		else{
			fin = null;
			return null;
		}
	}
}
