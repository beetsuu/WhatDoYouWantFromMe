using Godot;
using System;

public partial class TopUi : Node2D
{

	[Export]
	public Timer timer;

	public bool timeout = false;

	public RichTextLabel scoreText;
	RichTextLabel timerText;

	public override void _Ready()
	{
		scoreText = GetNode<RichTextLabel>("ScoreText");
		timerText = GetNode<RichTextLabel>("TimerText");

		timer.Start();
	}

	public override void _Process(double delta)
	{
		if (timer.Paused != true)
		{
			getTimeForText();
		}
	}

	public void getTimeForText()
	{
		timerText.Text = Mathf.CeilToInt((float)timer.TimeLeft).ToString();

		if (timer.TimeLeft <= 3)
		{
			timerText.Modulate = new Color(1, 0, 0);
		}
		else
		{
			timerText.Modulate = new Color(1, 1, 1);
		}
	}

	private void onTimerTimeout()
	{
		timeout = true;

	}

}
