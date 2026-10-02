using System;

namespace TaleWorlds.Library
{
	// Token: 0x0200006A RID: 106
	public static class MathF
	{
		// Token: 0x06000359 RID: 857 RVA: 0x0000C222 File Offset: 0x0000A422
		public static float Sqrt(float x)
		{
			return (float)Math.Sqrt((double)x);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000C22C File Offset: 0x0000A42C
		public static float Sin(float x)
		{
			return (float)Math.Sin((double)x);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000C236 File Offset: 0x0000A436
		public static float Asin(float x)
		{
			return (float)Math.Asin((double)x);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000C240 File Offset: 0x0000A440
		public static float Cos(float x)
		{
			return (float)Math.Cos((double)x);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000C24A File Offset: 0x0000A44A
		public static float Acos(float x)
		{
			return (float)Math.Acos((double)x);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000C254 File Offset: 0x0000A454
		public static float Tan(float x)
		{
			return (float)Math.Tan((double)x);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000C25E File Offset: 0x0000A45E
		public static float Tanh(float x)
		{
			return (float)Math.Tanh((double)x);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000C268 File Offset: 0x0000A468
		public static float Atan(float x)
		{
			return (float)Math.Atan((double)x);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000C272 File Offset: 0x0000A472
		public static float Atan2(float y, float x)
		{
			return (float)Math.Atan2((double)y, (double)x);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000C27E File Offset: 0x0000A47E
		public static double Pow(double x, double y)
		{
			return Math.Pow(x, y);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000C287 File Offset: 0x0000A487
		[Obsolete("Types must match!", true)]
		public static double Pow(float x, double y)
		{
			return Math.Pow((double)x, y);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000C291 File Offset: 0x0000A491
		[Obsolete("Types must match!", true)]
		public static double Pow(double x, float y)
		{
			return Math.Pow(x, (double)y);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000C29B File Offset: 0x0000A49B
		public static float Pow(float x, float y)
		{
			return (float)Math.Pow((double)x, (double)y);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000C2A7 File Offset: 0x0000A4A7
		public static int PowTwo32(int x)
		{
			return 1 << x;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000C2AF File Offset: 0x0000A4AF
		public static ulong PowTwo64(int x)
		{
			return 1UL << x;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000C2B8 File Offset: 0x0000A4B8
		public static bool IsValidValue(float f)
		{
			return !float.IsNaN(f) && !float.IsInfinity(f);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000C2CD File Offset: 0x0000A4CD
		public static float Clamp(float value, float minValue, float maxValue)
		{
			if (value < minValue)
			{
				return minValue;
			}
			if (value > maxValue)
			{
				return maxValue;
			}
			return value;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000C2DC File Offset: 0x0000A4DC
		public static float AngleClamp(float angle)
		{
			while (angle < 0f)
			{
				angle += 6.2831855f;
			}
			while (angle > 6.2831855f)
			{
				angle -= 6.2831855f;
			}
			return angle;
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000C305 File Offset: 0x0000A505
		public static float Lerp(float valueFrom, float valueTo, float amount, float minimumDifference = 1E-05f)
		{
			if (Math.Abs(valueFrom - valueTo) <= minimumDifference)
			{
				return valueTo;
			}
			return valueFrom + (valueTo - valueFrom) * amount;
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000C31B File Offset: 0x0000A51B
		[Obsolete("Blend amount must be float")]
		public static float Lerp(float valueFrom, float valueTo, int amount, float minimumDifference = 1E-05f)
		{
			return 0f;
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000C324 File Offset: 0x0000A524
		public static float AngleLerp(float angleFrom, float angleTo, float amount, float minimumDifference = 1E-05f)
		{
			float num = (angleTo - angleFrom) % 6.2831855f;
			float num2 = 2f * num % 6.2831855f - num;
			return MathF.AngleClamp(angleFrom + num2 * amount);
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000C355 File Offset: 0x0000A555
		public static int Round(double f)
		{
			return (int)Math.Round(f);
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000C35E File Offset: 0x0000A55E
		public static int Round(float f)
		{
			return (int)Math.Round((double)f);
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000C368 File Offset: 0x0000A568
		public static float Round(float f, int digits)
		{
			return (float)Math.Round((double)f, digits);
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000C373 File Offset: 0x0000A573
		[Obsolete("Type is already int!", true)]
		public static int Round(int f)
		{
			return (int)Math.Round((double)((float)f));
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000C37E File Offset: 0x0000A57E
		public static int Floor(double f)
		{
			return (int)Math.Floor(f);
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000C387 File Offset: 0x0000A587
		public static int Floor(float f)
		{
			return (int)Math.Floor((double)f);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000C391 File Offset: 0x0000A591
		[Obsolete("Type is already int!", true)]
		public static int Floor(int f)
		{
			return (int)Math.Floor((double)((float)f));
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000C39C File Offset: 0x0000A59C
		public static int Ceiling(double f)
		{
			return (int)Math.Ceiling(f);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000C3A5 File Offset: 0x0000A5A5
		public static int Ceiling(float f)
		{
			return (int)Math.Ceiling((double)f);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000C3AF File Offset: 0x0000A5AF
		[Obsolete("Type is already int!", true)]
		public static int Ceiling(int f)
		{
			return (int)Math.Ceiling((double)((float)f));
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000C3BA File Offset: 0x0000A5BA
		public static double Abs(double f)
		{
			if (f < 0.0)
			{
				return -f;
			}
			return f;
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000C3CC File Offset: 0x0000A5CC
		public static float Abs(float f)
		{
			if (f < 0f)
			{
				return -f;
			}
			return f;
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000C3DA File Offset: 0x0000A5DA
		public static int Abs(int f)
		{
			if ((float)f < 0f)
			{
				return -f;
			}
			return f;
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000C3E9 File Offset: 0x0000A5E9
		public static double Max(double a, double b)
		{
			if (a <= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000C3F2 File Offset: 0x0000A5F2
		public static float Max(float a, float b)
		{
			if (a <= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000C3FB File Offset: 0x0000A5FB
		public static ValueTuple<float, float> MinMax(float a, float b)
		{
			if (a < b)
			{
				return new ValueTuple<float, float>(a, b);
			}
			return new ValueTuple<float, float>(b, a);
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000C410 File Offset: 0x0000A610
		[Obsolete("Types must match!", true)]
		public static float Max(float a, int b)
		{
			if (a <= (float)b)
			{
				return (float)b;
			}
			return a;
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000C41B File Offset: 0x0000A61B
		[Obsolete("Types must match!", true)]
		public static float Max(int a, float b)
		{
			if ((float)a <= b)
			{
				return b;
			}
			return (float)a;
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000C426 File Offset: 0x0000A626
		public static int Max(int a, int b)
		{
			if (a <= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000C42F File Offset: 0x0000A62F
		public static long Max(long a, long b)
		{
			if (a <= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000C438 File Offset: 0x0000A638
		public static uint Max(uint a, uint b)
		{
			if (a <= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000C441 File Offset: 0x0000A641
		public static float Max(float a, float b, float c)
		{
			return Math.Max(a, Math.Max(b, c));
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000C450 File Offset: 0x0000A650
		public static double Min(double a, double b)
		{
			if (a >= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0000C459 File Offset: 0x0000A659
		public static float Min(float a, float b)
		{
			if (a >= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000C462 File Offset: 0x0000A662
		public static short Min(short a, short b)
		{
			if (a >= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000C46B File Offset: 0x0000A66B
		public static int Min(int a, int b)
		{
			if (a >= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000C474 File Offset: 0x0000A674
		public static long Min(long a, long b)
		{
			if (a >= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000C47D File Offset: 0x0000A67D
		public static uint Min(uint a, uint b)
		{
			if (a >= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000C486 File Offset: 0x0000A686
		[Obsolete("Types must match!", true)]
		public static int Min(int a, float b)
		{
			if ((float)a >= b)
			{
				return (int)b;
			}
			return a;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000C491 File Offset: 0x0000A691
		[Obsolete("Types must match!", true)]
		public static int Min(float a, int b)
		{
			if (a >= (float)b)
			{
				return b;
			}
			return (int)a;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000C49C File Offset: 0x0000A69C
		public static float Min(float a, float b, float c)
		{
			return Math.Min(a, Math.Min(b, c));
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000C4AC File Offset: 0x0000A6AC
		public static float PingPong(float min, float max, float time)
		{
			int num = (int)(min * 100f);
			int num2 = (int)(max * 100f);
			int num3 = (int)(time * 100f);
			int num4 = num2 - num;
			bool flag = num3 / num4 % 2 == 0;
			int num5 = num3 % num4;
			return (float)(flag ? (num5 + num) : (num2 - num5)) / 100f;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000C4F8 File Offset: 0x0000A6F8
		public static int GreatestCommonDivisor(int a, int b)
		{
			while (b != 0)
			{
				int num = a % b;
				a = b;
				b = num;
			}
			return a;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000C508 File Offset: 0x0000A708
		public static float Log(float a)
		{
			return (float)Math.Log((double)a);
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000C512 File Offset: 0x0000A712
		public static float Log(float a, float newBase)
		{
			return (float)Math.Log((double)a, (double)newBase);
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000C51E File Offset: 0x0000A71E
		public static int Sign(float f)
		{
			return Math.Sign(f);
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000C526 File Offset: 0x0000A726
		public static int Sign(int f)
		{
			return Math.Sign(f);
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0000C52E File Offset: 0x0000A72E
		public static void SinCos(float a, out float sa, out float ca)
		{
			sa = MathF.Sin(a);
			ca = MathF.Cos(a);
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000C540 File Offset: 0x0000A740
		public static float Log10(float val)
		{
			return (float)Math.Log10((double)val);
		}

		// Token: 0x04000132 RID: 306
		public const float DegToRad = 0.017453292f;

		// Token: 0x04000133 RID: 307
		public const float RadToDeg = 57.29578f;

		// Token: 0x04000134 RID: 308
		public const float TwoPI = 6.2831855f;

		// Token: 0x04000135 RID: 309
		public const float PI = 3.1415927f;

		// Token: 0x04000136 RID: 310
		public const float HalfPI = 1.5707964f;

		// Token: 0x04000137 RID: 311
		public const float E = 2.7182817f;

		// Token: 0x04000138 RID: 312
		public const float Epsilon = 1E-05f;
	}
}
