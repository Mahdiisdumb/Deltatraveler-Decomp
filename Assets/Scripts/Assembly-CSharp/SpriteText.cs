using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(CanvasRenderer))]
public class SpriteText : Graphic
{
	public enum AlignmentH
	{
		Left = 0,
		Center = 1,
		Right = 2
	}

	public enum AlignmentV
	{
		Top = 0,
		Center = 1,
		Bottom = 2
	}

	[TextArea]
	[SerializeField]
	private string text = "";

	[SerializeField]
	private string spritePath = "sprites/ui/spr_font_small";

	[SerializeField]
	private int characterSpacing;

	[SerializeField]
	private int spaceSize = 8;

	[SerializeField]
	private int lineSpacing;

	[SerializeField]
	private string customCharacterSet = "";

	[SerializeField]
	private Vector2 inset = new Vector2(4f, -4f);

	[SerializeField]
	private AlignmentH hAlignment;

	[SerializeField]
	private AlignmentV vAlignment;

	private readonly string CHARACTER_SET = "abcdefghijklmnopqrstuvwxyz0123456789!?-=.,#*/[]:";

	private Sprite[] spriteSet;

	private Texture texture;

	public string Text
	{
		get
		{
			return text;
		}
		set
		{
			if (!(text == value.ToLower()))
			{
				text = value.ToLower();
				SetVerticesDirty();
			}
		}
	}

	public string SpritePath
	{
		get
		{
			return spritePath;
		}
		set
		{
			if (!(spritePath == value))
			{
				spritePath = value;
				spriteSet = null;
				SetVerticesDirty();
			}
		}
	}

	public int CharacterSpacing
	{
		get
		{
			return characterSpacing;
		}
		set
		{
			if (characterSpacing != value)
			{
				characterSpacing = value;
				SetVerticesDirty();
			}
		}
	}

	public int SpaceSize
	{
		get
		{
			return spaceSize;
		}
		set
		{
			if (spaceSize != value)
			{
				spaceSize = value;
				SetVerticesDirty();
			}
		}
	}

	public int LineSpacing
	{
		get
		{
			return lineSpacing;
		}
		set
		{
			if (lineSpacing != value)
			{
				lineSpacing = value;
				SetVerticesDirty();
			}
		}
	}

	public string CustomCharacterSet
	{
		get
		{
			return customCharacterSet;
		}
		set
		{
			if (!(customCharacterSet == value))
			{
				customCharacterSet = value;
				SetVerticesDirty();
			}
		}
	}

	public Vector2 Inset
	{
		get
		{
			return inset;
		}
		set
		{
			if (!(inset == value))
			{
				inset = value;
				SetVerticesDirty();
			}
		}
	}

	public AlignmentH HAlignment
	{
		get
		{
			return hAlignment;
		}
		set
		{
			if (hAlignment != value)
			{
				hAlignment = value;
				SetVerticesDirty();
			}
		}
	}

	public AlignmentV VAlignment
	{
		get
		{
			return vAlignment;
		}
		set
		{
			if (vAlignment != value)
			{
				vAlignment = value;
				SetVerticesDirty();
			}
		}
	}

	public override Texture mainTexture => texture;

	private string GetCharacterSet()
	{
		if (customCharacterSet != "")
		{
			return customCharacterSet;
		}
		return CHARACTER_SET;
	}

	protected override void OnPopulateMesh(VertexHelper vh)
	{
		base.OnPopulateMesh(vh);
		if (spriteSet == null)
		{
			spriteSet = Resources.LoadAll<Sprite>(spritePath);
		}
		vh.Clear();
		string characterSet = GetCharacterSet();
		if (spriteSet.Length != characterSet.Length)
		{
			return;
		}
		texture = spriteSet[0].texture;
		string[] array = this.text.Split('\n');
		int num = 0;
		int num2 = ((int)spriteSet[0].rect.height + lineSpacing) * array.Length;
		string[] array2 = array;
		foreach (string text in array2)
		{
			int num3 = 0;
			List<int> list = new List<int>();
			List<Vector3> list2 = new List<Vector3>();
			for (int j = 0; j < text.Length; j++)
			{
				int num4 = characterSet.IndexOf(text[j]);
				list.Add(num4);
				if (num4 > -1 && num4 < spriteSet.Length)
				{
					list2.Add(new Vector3(num3, -num));
					num3 += (int)spriteSet[num4].rect.width;
				}
				else
				{
					list2.Add(Vector3.zero);
					num3 += spaceSize;
				}
				num3 += characterSpacing;
			}
			for (int k = 0; k < text.Length; k++)
			{
				int num5 = list[k];
				if (num5 >= 0)
				{
					Vector3 vector = list2[k];
					if (hAlignment == AlignmentH.Center)
					{
						vector.x -= num3 / 2;
					}
					else if (hAlignment == AlignmentH.Right)
					{
						vector.x -= num3;
					}
					if (vAlignment == AlignmentV.Center)
					{
						vector.y += num2 / 2;
					}
					else if (vAlignment == AlignmentV.Bottom)
					{
						vector.y += num2;
					}
					DrawLetter(vh, num5, vector);
				}
			}
			num += (int)spriteSet[0].rect.height + lineSpacing;
		}
	}

	private void DrawLetter(VertexHelper vh, int sprite, Vector2 pos)
	{
		pos -= inset;
		Sprite sprite2 = spriteSet[sprite];
		Vector2 vector = new Vector2(sprite2.rect.size.x, 0f - sprite2.rect.size.y);
		Vector2 vector2 = pos;
		Vector2 vector3 = vector + pos;
		vh.AddVert(new Vector2(vector2.x, vector2.y), color, new Vector2(sprite2.uv[0].x, sprite2.uv[0].y));
		vh.AddVert(new Vector2(vector2.x, vector3.y), color, new Vector2(sprite2.uv[0].x, sprite2.uv[3].y));
		vh.AddVert(new Vector2(vector3.x, vector3.y), color, new Vector2(sprite2.uv[3].x, sprite2.uv[3].y));
		vh.AddVert(new Vector2(vector3.x, vector2.y), color, new Vector2(sprite2.uv[3].x, sprite2.uv[0].y));
		int currentVertCount = vh.currentVertCount;
		vh.AddTriangle(currentVertCount - 1, currentVertCount - 2, currentVertCount - 3);
		vh.AddTriangle(currentVertCount - 3, currentVertCount - 4, currentVertCount - 1);
	}
}
