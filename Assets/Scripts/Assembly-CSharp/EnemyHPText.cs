using UnityEngine;

public class EnemyHPText : MonoBehaviour
{
	private Transform textTransform;

	protected int frames;

	private float txtChange;

	private float offset;

	protected virtual void Update()
	{
		frames++;
		textTransform.localPosition += new Vector3(0f, txtChange);
		if (txtChange > 0f)
		{
			txtChange -= 1.35f;
		}
		else
		{
			txtChange -= 0.6f;
		}
		if (frames >= 16)
		{
			textTransform.localPosition = new Vector3(textTransform.localPosition.x, offset);
		}
	}

	public virtual void StartHP(int dmg, int oldHp, int maxHp, int partyMember, bool mercy, EnemyHPBar hpBar)
	{
		txtChange = 7.35f;
		int num = oldHp - dmg - oldHp;
		textTransform = base.transform.Find("text");
		Color color = ((!mercy) ? PartyPanels.defaultColors[partyMember] : PartyPanels.defaultColors[2]);
		if (partyMember == 0 && (int)Object.FindObjectOfType<GameManager>().GetFlag(107) == 1 && (int)Util.GameManager().GetFlag(108) == 1)
		{
			color = ((num == 0) ? new Color(0.765f, 0.765f, 0.765f) : Color.red);
		}
		string text;
		Color color2;
		if (mercy)
		{
			text = ((dmg < 0) ? (dmg + "%") : ("+" + dmg + "%"));
			color2 = PartyPanels.defaultColors[2];
			if (dmg >= 100)
			{
				color2 = new Color(0f, 1f, 0f);
			}
		}
		else if (num < 0)
		{
			text = dmg.ToString();
			color2 = color;
		}
		else if (num > 0)
		{
			text = "+" + dmg.ToString().Substring(1);
			color2 = new Color(0f, 1f, 0f);
		}
		else
		{
			text = "*";
			color2 = color;
		}
		textTransform.GetComponent<SpriteText>().Text = text;
		textTransform.GetComponent<SpriteText>().color = color2;
		textTransform.GetComponent<SpriteText>().CharacterSpacing = ((!(text == "*")) ? (-4) : 0);
		textTransform.GetComponent<SpriteText>().Inset = new Vector2((text == "*") ? 62 : (2 + 32 * text.Length / 2), -31f);
		if ((bool)hpBar)
		{
			int numPartyMembersEffecting = hpBar.GetNumPartyMembersEffecting();
			offset = 32 * numPartyMembersEffecting;
			if (numPartyMembersEffecting >= 3)
			{
				offset = -60f;
			}
		}
		textTransform.localPosition = new Vector3(0f, offset);
	}
}
