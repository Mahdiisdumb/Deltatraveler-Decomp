using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnoPanels : MonoBehaviour
{
	private RectTransform[] panels = new RectTransform[4];

	private Image arrow;

	private bool panelSet;

	private float angle;

	private float curAngle;

	private int turn = -1;

	private void Awake()
	{
		arrow = base.transform.Find("TurnArrow").GetComponent<Image>();
	}

	private void Update()
	{
		for (int i = 0; i < 4; i++)
		{
			panels[i].localPosition = new Vector3(Mathf.Lerp(panels[i].localPosition.x, (turn == i) ? (-290) : (-310), 0.5f), panels[i].localPosition.y);
		}
		curAngle = Mathf.Lerp(curAngle, angle, 0.25f);
		arrow.transform.localEulerAngles = new Vector3(0f, 0f, curAngle);
		if (turn != -1)
		{
			arrow.transform.localPosition = new Vector3(arrow.transform.localPosition.x, Mathf.Lerp(arrow.transform.localPosition.y, panels[turn].localPosition.y, 0.33f));
		}
	}

	public void ReverseTurnOrder()
	{
		angle += 180f;
	}

	public void SetDone(int done)
	{
		panels[done].GetComponent<Image>().color = Color.grey;
		panels[done].Find("name").GetComponent<Text>().color = Color.grey;
	}

	public void UpdateTurn(int turn)
	{
		if (this.turn != -1)
		{
			panels[this.turn].Find("Portrait").GetComponent<Image>().enabled = false;
		}
		arrow.enabled = true;
		this.turn = turn;
		panels[this.turn].Find("Portrait").GetComponent<Image>().enabled = true;
	}

	public void SetPanels(List<int> turnOrder)
	{
		if (!panelSet)
		{
			panelSet = true;
			for (int i = 0; i < 4; i++)
			{
				panels[i] = base.transform.GetChild(turnOrder[i]).GetComponent<RectTransform>();
			}
			for (int j = 0; j < 4; j++)
			{
				panels[j].SetAsLastSibling();
				panels[j].localPosition = new Vector3(-310f, 105 - j * 35);
			}
		}
	}
}
