using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000145 RID: 325
	public struct PlayerDataExperience
	{
		// Token: 0x170002CC RID: 716
		// (get) Token: 0x060008E6 RID: 2278 RVA: 0x0000D183 File Offset: 0x0000B383
		// (set) Token: 0x060008E7 RID: 2279 RVA: 0x0000D18B File Offset: 0x0000B38B
		public int Experience { get; private set; }

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x060008E8 RID: 2280 RVA: 0x0000D194 File Offset: 0x0000B394
		public int Level
		{
			get
			{
				return PlayerDataExperience.CalculateLevelFromExperience(this.Experience);
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x060008E9 RID: 2281 RVA: 0x0000D1A1 File Offset: 0x0000B3A1
		public int ExperienceToNextLevel
		{
			get
			{
				return PlayerDataExperience.CalculateExperienceFromLevel(this.Level + 1) - this.Experience;
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x060008EA RID: 2282 RVA: 0x0000D1B7 File Offset: 0x0000B3B7
		public int ExperienceInCurrentLevel
		{
			get
			{
				return this.Experience - PlayerDataExperience.CalculateExperienceFromLevel(this.Level);
			}
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0000D1CB File Offset: 0x0000B3CB
		static PlayerDataExperience()
		{
			PlayerDataExperience.InitializeXPRequirements();
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x0000D1D9 File Offset: 0x0000B3D9
		public PlayerDataExperience(int experience)
		{
			this.Experience = experience;
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0000D1E4 File Offset: 0x0000B3E4
		public static int CalculateLevelFromExperience(int experience)
		{
			int num = 1;
			int i = 0;
			while (i <= experience)
			{
				i += PlayerDataExperience.ExperienceRequiredForLevel(num + 1);
				if (i <= experience)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0000D20F File Offset: 0x0000B40F
		public static int CalculateExperienceFromLevel(int level)
		{
			if (level == 1)
			{
				return 0;
			}
			if (level < PlayerDataExperience._maxLevelForXPRequirementCalculation)
			{
				return PlayerDataExperience._levelToXP[level];
			}
			return PlayerDataExperience.ExperienceRequiredForLevel(level) + PlayerDataExperience.CalculateExperienceFromLevel(level - 1);
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x0000D236 File Offset: 0x0000B436
		public static int ExperienceRequiredForLevel(int level)
		{
			return Convert.ToInt32(Math.Floor(100.0 * Math.Pow((double)(level - 1), 1.03)));
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x0000D260 File Offset: 0x0000B460
		private static void InitializeXPRequirements()
		{
			PlayerDataExperience._levelToXP = new int[PlayerDataExperience._maxLevelForXPRequirementCalculation];
			int num = 0;
			for (int i = 2; i < PlayerDataExperience._maxLevelForXPRequirementCalculation; i++)
			{
				num += PlayerDataExperience.ExperienceRequiredForLevel(i);
				PlayerDataExperience._levelToXP[i] = num;
			}
		}

		// Token: 0x040003C6 RID: 966
		private static int[] _levelToXP;

		// Token: 0x040003C7 RID: 967
		private static readonly int _maxLevelForXPRequirementCalculation = 30;
	}
}
