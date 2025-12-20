public class JerryFourthYellow : JerryYellowAttack
{
	protected override void Awake()
	{
		numOfBullets = 60;
		base.Awake();
		bigBullets[0] = true;
		bigBullets[1] = true;
		bigBullets[2] = true;
		bigBullets[3] = true;
		bigBullets[7] = true;
		bigBullets[12] = true;
		bigBullets[14] = true;
		bigBullets[16] = true;
		bigBullets[18] = true;
		bigBullets[25] = true;
		bigBullets[26] = true;
		bigBullets[27] = true;
		bigBullets[28] = true;
		bigBullets[35] = true;
		bigBullets[40] = true;
		bigBullets[45] = true;
		bigBullets[46] = true;
		bigBullets[49] = true;
		bigBullets[50] = true;
		bigBullets[55] = true;
		bigBullets[56] = true;
		bigBullets[57] = true;
		bigBullets[58] = true;
		bigBullets[59] = true;
		redBullets[0] = true;
		redBullets[4] = true;
		redBullets[5] = true;
		redBullets[8] = true;
		redBullets[9] = true;
		redBullets[13] = true;
		redBullets[14] = true;
		redBullets[17] = true;
		redBullets[20] = true;
		redBullets[22] = true;
		redBullets[24] = true;
		redBullets[26] = true;
		redBullets[30] = true;
		redBullets[32] = true;
		redBullets[34] = true;
		redBullets[36] = true;
		redBullets[38] = true;
		redBullets[42] = true;
		redBullets[43] = true;
		redBullets[46] = true;
		redBullets[47] = true;
		redBullets[50] = true;
		redBullets[52] = true;
		redBullets[53] = true;
		redBullets[57] = true;
		fallRate = 4;
		bulletSpeed = 1.5f;
		spawnRate = 3;
	}
}
