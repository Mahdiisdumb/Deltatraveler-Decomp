using System;
using UnityEngine;

public class SpearArrowBullet : BulletBase
{
	private int speed;

	private Vector3 dir;

	private ShieldAttackBase parent;

	private bool isLead;

	private bool activated;

	private BoxCollider2D boxcol;

	private bool reverse;

	private bool isSwapping;

	private bool hasSwapped;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 7;
		karmaImpact = 4;
		tpGrazeValue = 2.5f;
		boxcol = UnityEngine.Object.FindObjectOfType<BoxCollider2D>();
	}

	private void Update()
	{
		if (!activated)
		{
			return;
		}
		if (!reverse || (reverse && hasSwapped))
		{
			base.transform.position = base.transform.position + dir * ((float)speed / 48f);
		}
		else if (!isSwapping)
		{
			base.transform.position = base.transform.position - dir * ((float)speed / 48f);
			if (IsVertical())
			{
				float num = base.transform.position.y - dir.y * ((float)speed / 48f);
				if ((dir == Vector3.up && num <= 1.592f) || (dir == Vector3.down && num >= -1.508f))
				{
					boxcol.enabled = false;
					isSwapping = true;
				}
			}
			else
			{
				float num2 = base.transform.position.x - dir.x * ((float)speed / 48f);
				if ((dir == Vector3.right && num2 <= 1.55f) || (dir == Vector3.left && num2 >= -1.55f))
				{
					boxcol.enabled = false;
					isSwapping = true;
				}
			}
		}
		else
		{
			frames++;
			float num3 = Mathf.Lerp(1.5f, -1.5f, (float)frames / 10f);
			float f = Mathf.Lerp(base.transform.rotation.eulerAngles.z, base.transform.rotation.eulerAngles.z - 180f, (float)frames / 10f) * ((float)Math.PI / 180f);
			if (IsVertical())
			{
				base.transform.position = new Vector3(0f, -0.042f) + new Vector3(Mathf.Cos(f), num3 * dir.y);
			}
			else
			{
				base.transform.position = new Vector3(0f, -0.042f) + new Vector3(num3 * dir.x, Mathf.Sin(f));
			}
			if (frames == 10)
			{
				boxcol.enabled = true;
				isSwapping = false;
				hasSwapped = true;
			}
		}
	}

	public void Activate(int speed, Vector3 dir, bool isLead, ShieldAttackBase parent, Vector3 offset, bool reverse)
	{
		this.reverse = reverse;
		this.dir = dir;
		base.transform.right = dir;
		if (base.transform.rotation.eulerAngles.y == 180f)
		{
			base.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
		}
		float num = 1f;
		if (reverse)
		{
			GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/attacks/bullets/spr_speararrow_reverse");
			num = -1f;
		}
		this.speed = speed;
		this.parent = parent;
		if (isLead)
		{
			SetAsLead();
		}
		base.transform.position = new Vector3(0f, -0.042f) - dir * 7f * num + offset;
		activated = true;
	}

	private void OnDestroy()
	{
		if (isLead && parent != null)
		{
			parent.SetNewLeadBullet(base.gameObject);
		}
	}

	public void SetAsLead()
	{
		isLead = true;
		if (!reverse)
		{
			GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/attacks/bullets/spr_speararrow_highlight");
		}
	}

	private bool IsVertical()
	{
		if (!(dir == Vector3.down))
		{
			return dir == Vector3.up;
		}
		return true;
	}
}
