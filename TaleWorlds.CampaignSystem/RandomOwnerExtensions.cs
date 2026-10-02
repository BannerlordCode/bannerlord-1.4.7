using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000099 RID: 153
	public static class RandomOwnerExtensions
	{
		// Token: 0x060012BD RID: 4797 RVA: 0x00054CA6 File Offset: 0x00052EA6
		public static int RandomIntWithSeed(this IRandomOwner obj, uint seed)
		{
			return MBRandom.RandomIntWithSeed((uint)obj.RandomValue, seed);
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x00054CB4 File Offset: 0x00052EB4
		public static int RandomIntWithSeed(this IRandomOwner obj, uint seed, int max)
		{
			return obj.RandomIntWithSeed(seed, 0, max);
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x00054CBF File Offset: 0x00052EBF
		public static int RandomIntWithSeed(this IRandomOwner obj, uint seed, int min, int max)
		{
			return RandomOwnerExtensions.Random(obj.RandomIntWithSeed(seed), min, max);
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x00054CCF File Offset: 0x00052ECF
		public static float RandomFloatWithSeed(this IRandomOwner obj, uint seed)
		{
			return MBRandom.RandomFloatWithSeed((uint)obj.RandomValue, seed);
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x00054CDD File Offset: 0x00052EDD
		public static float RandomFloatWithSeed(this IRandomOwner obj, uint seed, float max)
		{
			return obj.RandomFloatWithSeed(seed, 0f, max);
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x00054CEC File Offset: 0x00052EEC
		public static float RandomFloatWithSeed(this IRandomOwner obj, uint seed, float min, float max)
		{
			return RandomOwnerExtensions.Random(obj.RandomFloatWithSeed(seed), min, max);
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x00054CFC File Offset: 0x00052EFC
		public static int RandomInt(this IRandomOwner obj)
		{
			return obj.RandomValue;
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x00054D04 File Offset: 0x00052F04
		public static int RandomInt(this IRandomOwner obj, int max)
		{
			return obj.RandomInt(0, max);
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x00054D0E File Offset: 0x00052F0E
		public static int RandomInt(this IRandomOwner obj, int min, int max)
		{
			return RandomOwnerExtensions.Random(obj.RandomInt(), min, max);
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x00054D1D File Offset: 0x00052F1D
		public static float RandomFloat(this IRandomOwner obj)
		{
			return (float)obj.RandomValue / 2.1474836E+09f;
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x00054D2C File Offset: 0x00052F2C
		public static float RandomFloat(this IRandomOwner obj, float max)
		{
			return obj.RandomFloat(0f, max);
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x00054D3A File Offset: 0x00052F3A
		public static float RandomFloat(this IRandomOwner obj, float min, float max)
		{
			return RandomOwnerExtensions.Random(obj.RandomFloat(), min, max);
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x00054D4C File Offset: 0x00052F4C
		private static int Random(int randomValue, int min, int max)
		{
			int num = max - min;
			if (num == 0)
			{
				Debug.FailedAssert("invalid Random parameters", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\IRandomOwner.cs", "Random", 79);
				return 0;
			}
			return min + randomValue % num;
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x00054D80 File Offset: 0x00052F80
		private static float Random(float randomValue, float min, float max)
		{
			float num = max - min;
			if (num <= 1E-45f)
			{
				Debug.FailedAssert("invalid Random parameters", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\IRandomOwner.cs", "Random", 91);
				return min;
			}
			return min + randomValue * num;
		}
	}
}
