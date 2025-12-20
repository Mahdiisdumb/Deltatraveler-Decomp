using System;
using UnityEngine;

[Serializable]
public struct Remark
{
	public int line;

	public string portrait;

	public Vector2 pos;

	public string text;

	private int frames;

	private int speed;

	private Vector2 dir;

	public Remark(int line, string portrait, string text, Vector2 pos)
	{
		this.line = line;
		this.portrait = portrait;
		this.text = text;
		this.pos = pos;
		frames = 5;
		speed = 2;
		dir = Vector2.left;
	}

	public Remark(int line, string portrait, string text, string posstring)
	{
		this.line = line;
		this.portrait = portrait;
		this.text = text;
		if (posstring.Length == 2)
		{
			int num = 0;
			int num2 = 0;
			char c = posstring[0];
			char c2 = posstring[1];
			switch (c)
			{
			case 'b':
				num2 = 68;
				break;
			case 't':
				num2 = -10;
				break;
			case 'c':
				num2 = 30;
				break;
			}
			switch (c2)
			{
			case 'l':
				num = 70;
				break;
			case 'r':
				num = 400;
				break;
			case 'c':
				num = 260;
				break;
			}
			pos = new Vector2(num, num2);
		}
		else
		{
			pos = Vector2.zero;
		}
		frames = 5;
		speed = 2;
		dir = Vector2.left;
	}

	public void SetExtra(int frames, int speed, Vector2 dir)
	{
		this.frames = frames;
		this.speed = speed;
		this.dir = dir;
	}

	public int GetFrames()
	{
		if (frames == 0)
		{
			return 5;
		}
		return frames;
	}

	public int GetSpeed()
	{
		if (speed == 0)
		{
			return 2;
		}
		return speed;
	}

	public Vector2 GetDir()
	{
		if (dir == Vector2.zero)
		{
			return Vector2.left;
		}
		return dir;
	}
}
