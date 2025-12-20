public class JerrySecondYellow : JerryYellowAttack
{
	protected override void Awake()
	{
		numOfBullets = 32;
		base.Awake();
		bigBullets[15] = true;
		bigBullets[20] = true;
		redBullets[26] = true;
		bigBullets[31] = true;
		redBullets[10] = true;
		redBullets[23] = true;
		redBullets[28] = true;
		redBullets[4] = true;
		fallRate = 5;
	}
}
