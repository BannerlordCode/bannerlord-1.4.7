using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200005B RID: 91
	public static class FaceGen
	{
		// Token: 0x0600071D RID: 1821 RVA: 0x00018C34 File Offset: 0x00016E34
		public static void SetInstance(IFaceGen faceGen)
		{
			FaceGen._instance = faceGen;
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x00018C3C File Offset: 0x00016E3C
		public static BodyProperties GetRandomBodyProperties(int race, bool isFemale, BodyProperties bodyPropertiesMin, BodyProperties bodyPropertiesMax, int hairCoverType, int seed, string hairTags, string beardTags, string tatooTags, float variationAmount)
		{
			if (FaceGen._instance != null)
			{
				return FaceGen._instance.GetRandomBodyProperties(race, isFemale, bodyPropertiesMin, bodyPropertiesMax, hairCoverType, seed, hairTags, beardTags, tatooTags, variationAmount);
			}
			return bodyPropertiesMin;
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x00018C6C File Offset: 0x00016E6C
		public static int GetRaceCount()
		{
			IFaceGen instance = FaceGen._instance;
			if (instance == null)
			{
				return 0;
			}
			return instance.GetRaceCount();
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x00018C7E File Offset: 0x00016E7E
		public static int GetRaceOrDefault(string raceId)
		{
			IFaceGen instance = FaceGen._instance;
			if (instance == null)
			{
				return 0;
			}
			return instance.GetRaceOrDefault(raceId);
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x00018C91 File Offset: 0x00016E91
		public static string GetBaseMonsterNameFromRace(int race)
		{
			IFaceGen instance = FaceGen._instance;
			return ((instance != null) ? instance.GetBaseMonsterNameFromRace(race) : null) ?? null;
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00018CAA File Offset: 0x00016EAA
		public static string[] GetRaceNames()
		{
			IFaceGen instance = FaceGen._instance;
			return ((instance != null) ? instance.GetRaceNames() : null) ?? null;
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x00018CC2 File Offset: 0x00016EC2
		public static Monster GetMonster(string monsterID)
		{
			IFaceGen instance = FaceGen._instance;
			if (instance == null)
			{
				return null;
			}
			return instance.GetMonster(monsterID);
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x00018CD5 File Offset: 0x00016ED5
		public static Monster GetMonsterWithSuffix(int race, string suffix)
		{
			IFaceGen instance = FaceGen._instance;
			if (instance == null)
			{
				return null;
			}
			return instance.GetMonsterWithSuffix(race, suffix);
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00018CE9 File Offset: 0x00016EE9
		public static Monster GetBaseMonsterFromRace(int race)
		{
			IFaceGen instance = FaceGen._instance;
			if (instance == null)
			{
				return null;
			}
			return instance.GetBaseMonsterFromRace(race);
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x00018CFC File Offset: 0x00016EFC
		public static void GenerateParentKey(BodyProperties childBodyProperties, int race, ref BodyProperties motherBodyProperties, ref BodyProperties fatherBodyProperties)
		{
			IFaceGen instance = FaceGen._instance;
			if (instance == null)
			{
				return;
			}
			instance.GenerateParentBody(childBodyProperties, race, ref motherBodyProperties, ref fatherBodyProperties);
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x00018D11 File Offset: 0x00016F11
		public static void SetHair(ref BodyProperties bodyProperties, int hair, int beard, int tattoo)
		{
			IFaceGen instance = FaceGen._instance;
			if (instance == null)
			{
				return;
			}
			instance.SetHair(ref bodyProperties, hair, beard, tattoo);
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00018D26 File Offset: 0x00016F26
		public static void SetBody(ref BodyProperties bodyProperties, int build, int weight)
		{
			IFaceGen instance = FaceGen._instance;
			if (instance == null)
			{
				return;
			}
			instance.SetBody(ref bodyProperties, build, weight);
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x00018D3A File Offset: 0x00016F3A
		public static void SetPigmentation(ref BodyProperties bodyProperties, int skinColor, int hairColor, int eyeColor)
		{
			IFaceGen instance = FaceGen._instance;
			if (instance == null)
			{
				return;
			}
			instance.SetPigmentation(ref bodyProperties, skinColor, hairColor, eyeColor);
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00018D4F File Offset: 0x00016F4F
		public static BodyProperties GetBodyPropertiesWithAge(ref BodyProperties originalBodyProperties, float age)
		{
			if (FaceGen._instance != null)
			{
				return FaceGen._instance.GetBodyPropertiesWithAge(ref originalBodyProperties, age);
			}
			return originalBodyProperties;
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00018D6B File Offset: 0x00016F6B
		public static BodyMeshMaturityType GetMaturityTypeWithAge(float age)
		{
			if (FaceGen._instance != null)
			{
				return FaceGen._instance.GetMaturityTypeWithAge(age);
			}
			return BodyMeshMaturityType.Child;
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x00018D81 File Offset: 0x00016F81
		public static int[] GetHairIndicesByTag(int race, int curGender, float age, string tag)
		{
			if (FaceGen._instance != null)
			{
				return FaceGen._instance.GetHairIndicesByTag(race, curGender, age, tag);
			}
			return Array.Empty<int>();
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x00018D9E File Offset: 0x00016F9E
		public static int[] GetFacialIndicesByTag(int race, int curGender, float age, string tag)
		{
			if (FaceGen._instance != null)
			{
				return FaceGen._instance.GetFacialIndicesByTag(race, curGender, age, tag);
			}
			return Array.Empty<int>();
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00018DBB File Offset: 0x00016FBB
		public static int[] GetTattooIndicesByTag(int race, int curGender, float age, string tag)
		{
			if (FaceGen._instance != null)
			{
				return FaceGen._instance.GetTattooIndicesByTag(race, curGender, age, tag);
			}
			return Array.Empty<int>();
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x00018DD8 File Offset: 0x00016FD8
		public static float GetTattooZeroProbability(int race, int curGender, float age)
		{
			if (FaceGen._instance != null)
			{
				return FaceGen._instance.GetTattooZeroProbability(race, curGender, age);
			}
			return 0f;
		}

		// Token: 0x04000392 RID: 914
		public const string MonsterSuffixSettlement = "_settlement";

		// Token: 0x04000393 RID: 915
		public const string MonsterSuffixSettlementSlow = "_settlement_slow";

		// Token: 0x04000394 RID: 916
		public const string MonsterSuffixSettlementFast = "_settlement_fast";

		// Token: 0x04000395 RID: 917
		public const string MonsterSuffixChild = "_child";

		// Token: 0x04000396 RID: 918
		public static bool ShowDebugValues;

		// Token: 0x04000397 RID: 919
		public static bool UpdateDeformKeys;

		// Token: 0x04000398 RID: 920
		private static IFaceGen _instance;
	}
}
