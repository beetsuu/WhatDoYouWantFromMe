using Godot;
using System;

public partial class Main : Node2D
{
	[Export]
	int lives;
	AnimatedSprite2D wuschelHead;
	AnimatedSprite2D wuschelBody;
	AnimatedSprite2D cloud;
	TextureButton walkies;
	TextureButton food;
	TextureButton pet;
	Sprite2D walkiesThink;
	Sprite2D foodThink;
	Sprite2D petThink;
	Sprite2D chimera;

	AudioStreamPlayer vineBoom;
	AudioStreamPlayer correct;
	AudioStreamPlayer wrong;
	AudioStreamPlayer chimeraSong;
	AudioStreamPlayer bgm;

	TopUi topUi;
	GameOver gameOverScreen;
	int input = 0;
	Random random = new Random();
	int answer = 0;
	int sameAnswerCount = 0;

	int chimeraCheck = 0;
	bool chimeraBool = false;

	int score = 0;
	int highscore = 0;

	const string SavePath = "user://savegame.json";


	public override void _Ready()
	{
		GetNodes();
		LoadHighscore();
		gameOverScreen.Hide();


		walkies.Pressed += () => ButtonPressed(1);
		food.Pressed += () => ButtonPressed(2);
		pet.Pressed += () => ButtonPressed(3);
		cloud.Show();

		topUi.scoreText.Text = score.ToString();

		NextQuestion();
	}

	public void GetNodes()
	{
		wuschelHead = GetNode<AnimatedSprite2D>("WuschelFrankensteinBody/WuschelHead");
		wuschelBody = GetNode<AnimatedSprite2D>("WuschelFrankensteinBody");
		walkies = GetNode<TextureButton>("Walkies");
		food = GetNode<TextureButton>("Food");
		pet = GetNode<TextureButton>("Pet");
		cloud = GetNode<AnimatedSprite2D>("Think/Cloud");
		vineBoom = GetNode<AudioStreamPlayer>("Sound/VineBoom");
		correct = GetNode<AudioStreamPlayer>("Sound/Correct");
		wrong = GetNode<AudioStreamPlayer>("Sound/Wrong");
		chimeraSong = GetNode<AudioStreamPlayer>("Sound/LocalForecast");
		bgm = GetNode<AudioStreamPlayer>("Sound/Investigation");
		gameOverScreen = GetNode<GameOver>("GameOver");

		walkiesThink = GetNode<Sprite2D>("Think/WalkiesThink");
		foodThink = GetNode<Sprite2D>("Think/FoodThink");
		petThink = GetNode<Sprite2D>("Think/PetThink");
		chimera = GetNode<Sprite2D>("Chimera");

		topUi = GetNode<TopUi>("TopUI");
	}

	public void ButtonPressed(int buttonNumber)
	{
		SetButtonsDisabled(true);
		vineBoom.Play();

		topUi.timer.Paused = true;

		input = buttonNumber;

		wuschelHead.Play("pause");

	}

	public void SetButtonsDisabled(bool disabled)
	{
		walkies.Disabled = disabled;
		food.Disabled = disabled;
		pet.Disabled = disabled;
	}

	public override void _Process(double delta)
	{

		if (topUi.timeout == true)
		{
			topUi.timeout = false;
			LoseLife();
		}


	}

	public void LoseLife()
	{
		if (chimeraBool)
		{
			score++;
			topUi.scoreText.Text = score.ToString();
			chimeraBool = false;
			chimera.Hide();
			chimeraSong.Stop();
			bgm.VolumeDb = 0;
			wuschelBody.Show();
			NextQuestion();
		}
		else
		{
			lives--;

			if (lives < 1)
			{
				//GD.Print("you lose..");
				GameOver();
			}
		}

	}

	public void NextQuestion()
	{
		chimeraCheck = random.Next(1, 51);
		//GD.Print(chimeraCheck);
		//chimeraCheck = 25;
		if (chimeraCheck == 25)
		{
			topUi.timer.Start();
			topUi.timer.Paused = false;
			chimeraBool = true;
			ChimeraMethod();
		}
		else
		{

			if (score == 25)
			{
				topUi.timer.WaitTime = 3;
			}

			int oldAnswer = answer;
			answer = random.Next(1, 4);

			if (oldAnswer == answer)
			{
				sameAnswerCount++;
			}
			else
			{
				sameAnswerCount = 1;
			}

			if (sameAnswerCount > 3)
			{
				do
				{
					answer = random.Next(1, 4);
				}
				while (answer == oldAnswer);
				sameAnswerCount = 1;
			}


			topUi.timer.Start();
			topUi.timer.Paused = false;

			walkiesThink.Hide();
			foodThink.Hide();
			petThink.Hide();

			if (answer == 1)
			{
				WalkLevel();
			}
			else if (answer == 2)
			{
				FoodLevel();
			}
			else if (answer == 3)
			{
				PetLevel();
			}

			SetButtonsDisabled(false);
		}
	}

	public void FoodLevel()
	{
		if (score < 5)
		{
			foodThink.Show();
			wuschelHead.Play("hungry");

		}
		else if (score < 10)
		{
			ThinkBubble();
			wuschelHead.Play("hungry");
		}
		else if (score < 20)
		{
			wuschelHead.Play("hungry_2");
		}
	}

	public void PetLevel()
	{
		if (score < 5)
		{
			petThink.Show();
			wuschelHead.Play("pet");
		}
		else if (score < 10)
		{
			ThinkBubble();
			wuschelHead.Play("pet");
		}
		else if (score < 20)
		{
			wuschelHead.Play("pet_2");
		}
	}

	public void WalkLevel()
	{
		if (score < 5)
		{
			walkiesThink.Show();
			wuschelHead.Play("walk");
		}
		else if (score < 10)
		{
			ThinkBubble();
			wuschelHead.Play("walk");
		}
		else if (score < 20)
		{
			wuschelHead.Play("walk_2");
		}
	}

	public void ThinkBubble()
	{
		cloud.Hide();
	}

	private void onHeadAnimationFinished()
	{
		if (wuschelHead.Animation == "pause")
		{
			if (answer == input)
			{
				correct.Play();
				wuschelHead.Play("rightAnswer");
				score++;
				topUi.scoreText.Text = score.ToString();
			}
			else
			{
				wrong.Play();
				wuschelHead.Play("wrongAnswer");
			}
		}
		else if (wuschelHead.Animation == "rightAnswer")
		{
			//GD.Print("nice!");
			wuschelHead.Play("default");

			NextQuestion();
		}
		else if (wuschelHead.Animation == "wrongAnswer")
		{
			wuschelHead.Play("default");

			LoseLife();
		}

	}

	private void GameOver()
	{
		topUi.timer.Paused = true;

		if (score > highscore)
		{
			highscore = score;
			SaveHighscore();
		}
		gameOverScreen.SetScore(score, highscore);
		gameOverScreen.Show();
		SetButtonsDisabled(true);
	}


	private void LoadHighscore()
	{
		if (FileAccess.FileExists(SavePath))
		{
			using var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Read);

			string json = file.GetAsText();

			var data = Json.ParseString(json).AsGodotDictionary();

			highscore = (int)data["highscore"];
		}
	}

	private void SaveHighscore()
	{
		using var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Write);

		var data = new Godot.Collections.Dictionary
	{
		{ "highscore", highscore }
	};

		file.StoreString(Json.Stringify(data));
	}

	void ChimeraMethod()
	{
		SetButtonsDisabled(true);
		wuschelBody.Hide();

		bgm.VolumeDb = -80;
		chimeraSong.Play();

		chimera.Show();


	}
}
