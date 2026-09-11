using Godot;
using System;

public partial class GameOver : Node2D
{
	Button restartButton;
	Button menuButton;
	RichTextLabel scoreText;
	RichTextLabel highscoreText;
	public override void _Ready()
	{
		restartButton = GetNode<Button>("RestartButton");
		menuButton = GetNode<Button>("MenuButton");

		scoreText = GetNode<RichTextLabel>("Label/ScoreText");
		highscoreText = GetNode<RichTextLabel>("highScore/HighscoreText");

		restartButton.Pressed += RestartButtonPressed;
		menuButton.Pressed += MenuButtonPressed;
	}

	public void SetScore(int score, int highscore)
	{
		scoreText.Text = "" + score;
		highscoreText.Text = "" + highscore;
	}




	private void RestartButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Main.tscn");
	}

	private void MenuButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
	}




}
