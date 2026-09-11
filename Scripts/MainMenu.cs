using Godot;
using System;

public partial class MainMenu : Node2D
{
	Button startButton;
	Button creditButton;
	Button endButton;



	public override void _Ready()
	{
		startButton = GetNode<Button>("StartButton");
		creditButton = GetNode<Button>("CreditButton");
		endButton = GetNode<Button>("EndButton");

		startButton.Pressed += ButtonPressed;
		creditButton.Pressed += CreditButtonPressed;
		endButton.Pressed += EndButtonPressed;
	}

	private void ButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Main.tscn");
	}

	private void CreditButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Credits.tscn");
	}

	private void EndButtonPressed()
	{
		GetTree().Quit();
	}



	public override void _Process(double delta)
	{
	}
}
