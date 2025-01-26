using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class AudioManager : Node
{
	[Export]
	AudioStreamPlayer musicPlayer;
	[Export]
	AudioStreamPlayer2D baseLowHealthStreamPlayer;
	[Export]
	AudioStream baseDestroyedStream;
	[Export]
	AudioStream bubblePopStream;
	[Export]
	AudioStream confirmUiStream;
	[Export]
	int soundEffectPlayerCount = 5;
	[Export]
	String soundEffectBusName = "SFX";

	private Queue<AudioStream> audioQueue = new Queue<AudioStream>();
	private Queue<AudioStreamPlayer2D> availableAudioPlayers = new Queue<AudioStreamPlayer2D>();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		for (int i = 0; i < soundEffectPlayerCount; i++)
		{
			var audioPlayer = new AudioStreamPlayer2D();
			audioPlayer.PanningStrength = 2;
			audioPlayer.Bus =soundEffectBusName;
			AddChild(audioPlayer);
			audioPlayer.Finished += () => OnStreamFinished(audioPlayer);
			availableAudioPlayers.Enqueue(audioPlayer);
		}
	}

	private void OnStreamFinished(AudioStreamPlayer2D audioPlayer)
	{
		Print($"Stream finished on {audioPlayer.Name}");
		availableAudioPlayers.Enqueue(audioPlayer);
	}

	public void PlayMusicStream()
	{
		musicPlayer.Play();
		//Print($"Music player playing? {musicPlayer.Playing}");
		musicPlayer.StreamPaused = false;
	}

	public void PauseMusicStream()
	{
		musicPlayer.StreamPaused = true;
	}

	public void PlayAndLoopBaseLowHealthStream()
	{
		if(!baseLowHealthStreamPlayer.Playing)
		{
			baseLowHealthStreamPlayer.Play();
		}
		
	}

	public void StopBaseLowHealthStream()
	{
		baseLowHealthStreamPlayer.Stop();
	}

	public void PlayBaseDestroyedStream()
	{
		audioQueue.Enqueue(baseDestroyedStream);
	}

	public void PlayBubblePopStream()
	{
		audioQueue.Enqueue(bubblePopStream);
	}

	public void PlayConfirmStream()
	{
		audioQueue.Enqueue(confirmUiStream);
	}

	public override void _Process(double delta)
	{
		if (audioQueue.Any()) Print($"Audio queue count {audioQueue.Count}, available players count {availableAudioPlayers.Count}");
		if (audioQueue.Any() && availableAudioPlayers.Any())
		{
			var audioPlayer = availableAudioPlayers.Dequeue();
			var audioStream = audioQueue.Dequeue();
			audioPlayer.Stream = audioStream;
			audioPlayer.Play();

		}

		if (testPop)
		{
			PlayBubblePopStream();
			testPop = false;
		}
		if (testConfirm)
		{
			PlayConfirmStream();
			testConfirm = false;
		}
		if(testBaseLowHealth)
		{
			PlayAndLoopBaseLowHealthStream();
		}
		else
		{
			StopBaseLowHealthStream();
		}
		if(testMusic)
		{
		PlayMusicStream();
		}
		if(testBaseDestroyed)
		{
			PlayBaseDestroyedStream();
			testBaseDestroyed = false;
		}
		
	}
	[Export]
	public bool testPop;
	[Export]
	public bool testConfirm;
	[Export]
	public bool testMusic;
	[Export]
	public bool testBaseLowHealth;
	[Export]
	public bool testBaseDestroyed;

	[Export] bool debug = false;
	private void Print(string message)
	{
		if (debug)
		{
			GD.Print(message);
		}
	}
}
