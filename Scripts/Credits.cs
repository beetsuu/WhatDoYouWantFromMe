using Godot;
using System;

public partial class Credits : Node2D
{
	Button returnButton;
	public override void _Ready()
	{
		returnButton = GetNode<Button>("ReturnButton");

		returnButton.Pressed += ReturnButtonPressed;
	}

	private void ReturnButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
	}

	public override void _Process(double delta)
	{
	}
}
