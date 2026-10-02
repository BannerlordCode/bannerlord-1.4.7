using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x020000B3 RID: 179
	public static class MBRandom
	{
		// Token: 0x17000325 RID: 805
		// (get) Token: 0x0600095B RID: 2395 RVA: 0x0001E9F6 File Offset: 0x0001CBF6
		private static MBFastRandom Random
		{
			get
			{
				if (Game.Current != null)
				{
					return Game.Current.RandomGenerator;
				}
				if (MBRandom._internalRandom == null)
				{
					MBRandom._internalRandom = new MBFastRandom();
				}
				return MBRandom._internalRandom;
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x0600095C RID: 2396 RVA: 0x0001EA20 File Offset: 0x0001CC20
		public static float RandomFloat
		{
			get
			{
				return MBRandom.Random.NextFloat();
			}
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x0001EA2C File Offset: 0x0001CC2C
		public static float RandomFloatRanged(float maxVal)
		{
			return MBRandom.RandomFloat * maxVal;
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x0001EA35 File Offset: 0x0001CC35
		public static float RandomFloatRanged(float minVal, float maxVal)
		{
			return minVal + MBRandom.RandomFloat * (maxVal - minVal);
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x0600095F RID: 2399 RVA: 0x0001EA44 File Offset: 0x0001CC44
		public static float RandomFloatNormal
		{
			get
			{
				int num = 4;
				float num2;
				float num4;
				do
				{
					num2 = 2f * MBRandom.RandomFloat - 1f;
					float num3 = 2f * MBRandom.RandomFloat - 1f;
					num4 = num2 * num2 + num3 * num3;
					num--;
				}
				while (num4 >= 1f || (num4 == 0f && num > 0));
				return num2 * num4 * 1f;
			}
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x0001EAA0 File Offset: 0x0001CCA0
		public static int RandomInt()
		{
			return MBRandom.Random.Next();
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0001EAAC File Offset: 0x0001CCAC
		public static int RandomInt(int maxValue)
		{
			return MBRandom.Random.Next(maxValue);
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x0001EAB9 File Offset: 0x0001CCB9
		public static int RandomInt(int minValue, int maxValue)
		{
			return MBRandom.Random.Next(minValue, maxValue);
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x0001EAC8 File Offset: 0x0001CCC8
		public static int RoundRandomized(float f)
		{
			int num = MathF.Floor(f);
			float num2 = f - (float)num;
			if (MBRandom.RandomFloat < num2)
			{
				num++;
			}
			return num;
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x0001EAF0 File Offset: 0x0001CCF0
		public static T ChooseWeighted<T>(IReadOnlyList<ValueTuple<T, float>> weightList)
		{
			int num;
			return MBRandom.ChooseWeighted<T>(weightList, out num);
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0001EB08 File Offset: 0x0001CD08
		public static T ChooseWeighted<T>(IReadOnlyList<ValueTuple<T, float>> weightList, out int chosenIndex)
		{
			chosenIndex = -1;
			float num = weightList.Sum<ValueTuple<T, float>>((ValueTuple<T, float> x) => x.Item2);
			float num2 = MBRandom.RandomFloat * num;
			for (int i = 0; i < weightList.Count; i++)
			{
				num2 -= weightList[i].Item2;
				if (num2 <= 0f)
				{
					chosenIndex = i;
					return weightList[i].Item1;
				}
			}
			if (weightList.Count > 0)
			{
				chosenIndex = 0;
				return weightList[0].Item1;
			}
			chosenIndex = -1;
			return default(T);
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x0001EBA4 File Offset: 0x0001CDA4
		public static float RandomFloatGaussian(float center, float spread, float min, float max)
		{
			float num = 1f - MBRandom.RandomFloat;
			float num2 = 1f - MBRandom.RandomFloat;
			float num3 = MathF.Sqrt(-2f * MathF.Log(num)) * MathF.Sin(6.2831855f * num2);
			return MathF.Clamp(center + spread * num3, min, max);
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x0001EBF4 File Offset: 0x0001CDF4
		public static void SetSeed(uint seed, uint seed2)
		{
			MBRandom.Random.SetSeed(seed, seed2);
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000968 RID: 2408 RVA: 0x0001EC02 File Offset: 0x0001CE02
		public static float NondeterministicRandomFloat
		{
			get
			{
				return MBRandom.NondeterministicRandom.NextFloat();
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x0001EC0E File Offset: 0x0001CE0E
		public static int NondeterministicRandomInt
		{
			get
			{
				return MBRandom.NondeterministicRandom.Next();
			}
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0001EC1A File Offset: 0x0001CE1A
		public static int RandomIntWithSeed(uint seed, uint seed2)
		{
			return MBFastRandom.GetRandomInt(seed, seed2);
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0001EC23 File Offset: 0x0001CE23
		public static float RandomFloatWithSeed(uint seed, uint seed2)
		{
			return MBFastRandom.GetRandomFloat(seed, seed2);
		}

		// Token: 0x04000526 RID: 1318
		public const int MaxSeed = 2000;

		// Token: 0x04000527 RID: 1319
		private static MBFastRandom _internalRandom = null;

		// Token: 0x04000528 RID: 1320
		private static readonly MBFastRandom NondeterministicRandom = new MBFastRandom();
	}
}
