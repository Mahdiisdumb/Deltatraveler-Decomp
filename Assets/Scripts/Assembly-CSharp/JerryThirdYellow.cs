public class JerryThirdYellow : JerryYellowAttack
{
	protected override void Awake()
	{
		numOfBullets = 40;
		base.Awake();
		bigBullets[0] = true;
		bigBullets[1] = true;
		bigBullets[7] = true;
		bigBullets[15] = true;
		bigBullets[20] = true;
		bigBullets[25] = true;
		bigBullets[26] = true;
		bigBullets[27] = true;
		bigBullets[30] = true;
		bigBullets[36] = true;
		bigBullets[37] = true;
		bigBullets[38] = true;
		bigBullets[39] = true;
		redBullets[3] = true;
		redBullets[8] = true;
		redBullets[10] = true;
		redBullets[18] = true;
		redBullets[24] = true;
		redBullets[25] = true;
		redBullets[28] = true;
		redBullets[31] = true;
		redBullets[35] = true;
		redBullets[39] = true;
		redBullets[4] = true;
		fallRate = 5;
		bulletSpeed = 1.4f;
		spawnRate = 3;
	}
}
