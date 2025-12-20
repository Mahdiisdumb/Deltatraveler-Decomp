using UnityEngine;

public class ShieldAttackTest : ShieldAttackBase
{
	protected override void Awake()
	{
		base.Awake();
		bulletInfo = new object[30][]
		{
			new object[4]
			{
				0,
				5,
				Vector3.down,
				new Vector3(0f, -1.25f)
			},
			null,
			new object[4]
			{
				0,
				5,
				Vector3.down,
				new Vector3(0f, -1.25f)
			},
			null,
			new object[4]
			{
				0,
				5,
				Vector3.down,
				new Vector3(0f, -1.25f)
			},
			null,
			null,
			null,
			null,
			new object[4]
			{
				0,
				20,
				Vector3.left,
				Vector3.zero
			},
			new object[4]
			{
				0,
				20,
				Vector3.up,
				Vector3.zero
			},
			new object[4]
			{
				0,
				20,
				Vector3.right,
				Vector3.zero
			},
			new object[4]
			{
				0,
				20,
				Vector3.up,
				Vector3.zero
			},
			new object[4]
			{
				0,
				20,
				Vector3.left,
				Vector3.zero
			},
			new object[4]
			{
				0,
				20,
				Vector3.up,
				Vector3.zero
			},
			new object[4]
			{
				0,
				20,
				Vector3.right,
				Vector3.zero
			},
			new object[4]
			{
				0,
				20,
				Vector3.down,
				Vector3.zero
			},
			new object[4]
			{
				0,
				20,
				Vector3.down,
				Vector3.zero
			},
			new object[4]
			{
				1,
				15,
				Vector3.left,
				Vector3.zero
			},
			new object[4]
			{
				1,
				15,
				Vector3.up,
				Vector3.zero
			},
			new object[4]
			{
				1,
				15,
				Vector3.right,
				Vector3.zero
			},
			new object[4]
			{
				1,
				15,
				Vector3.down,
				Vector3.zero
			},
			null,
			new object[6]
			{
				2,
				12,
				Vector3.left,
				Vector3.zero,
				2,
				2
			},
			null,
			new object[6]
			{
				2,
				12,
				Vector3.up,
				Vector3.zero,
				2,
				2
			},
			null,
			new object[6]
			{
				2,
				12,
				Vector3.right,
				Vector3.zero,
				2,
				2
			},
			null,
			new object[6]
			{
				2,
				60,
				Vector3.down,
				Vector3.zero,
				4,
				4
			}
		};
	}
}
