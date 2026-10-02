using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000027 RID: 39
	public class CubicBezier
	{
		// Token: 0x060000EF RID: 239 RVA: 0x000053F6 File Offset: 0x000035F6
		public static CubicBezier CreateEase(double controlPoint1X, double controlPoint1Y, double controlPoint2X, double controlPoint2Y)
		{
			return new CubicBezier(controlPoint1X, controlPoint1Y, controlPoint2X, controlPoint2Y, 0.0, 1.0);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00005413 File Offset: 0x00003613
		public static CubicBezier CreateYBeginToYEndWithRelativeControlDirs(double yBegin, double yEnd, double controlDir1X, double controlDir1Y, double controlDir2X, double controlDir2Y)
		{
			return new CubicBezier(controlDir1X, yBegin + controlDir1Y, 1.0 + controlDir2X, yEnd + controlDir2Y, yBegin, yEnd);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00005430 File Offset: 0x00003630
		public static CubicBezier CreateYBeginToYEnd(double yBegin, double yEnd, double controlPoint1X, double controlPoint1Y, double controlPoint2X, double controlPoint2Y)
		{
			return new CubicBezier(controlPoint1X, controlPoint1Y, controlPoint2X, controlPoint2Y, yBegin, yEnd);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00005440 File Offset: 0x00003640
		private CubicBezier(double x1, double y1, double x2, double y2, double yBegin = 0.0, double yEnd = 1.0)
		{
			this._y0 = yBegin;
			this._y3 = yEnd;
			this._x1 = x1;
			this._y1 = y1;
			this._x2 = x2;
			this._y2 = y2;
			if (0.0 > this._x1 || this._x1 > 1.0 || 0.0 > this._x2 || this._x2 > 1.0)
			{
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\CubicBezier.cs", ".ctor", 129);
				throw new ArgumentOutOfRangeException();
			}
			for (int i = 0; i < 11; i++)
			{
				this._sampleValues[i] = CubicBezier.CalcBezierX((double)i * 0.1, this._x1, this._x2);
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00005524 File Offset: 0x00003724
		public double Sample(double x)
		{
			if (CubicBezier.AlmostEq(x, 0.0))
			{
				return this._y0;
			}
			if (CubicBezier.AlmostEq(x, 1.0))
			{
				return this._y3;
			}
			double num = this._y3 - this._y0;
			if (CubicBezier.AlmostEq(this._x1 * num, this._y1 - this._y0) && CubicBezier.AlmostEq((1.0 - this._x2) * num, this._y3 - this._y2))
			{
				return this._y0 + x * num;
			}
			return CubicBezier.CalcBezierY(this.GetTForX(x), this._y0, this._y1, this._y2, this._y3);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000055E0 File Offset: 0x000037E0
		private static bool AlmostEq(double a, double b)
		{
			return MathF.Abs(a - b) < 1E-09;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000055F5 File Offset: 0x000037F5
		private static double AY(double aA0, double aA1, double aA2, double aA3)
		{
			return 1.0 * aA3 - 3.0 * aA2 + 3.0 * aA1 - 1.0 * aA0;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00005626 File Offset: 0x00003826
		private static double A(double aA1, double aA2)
		{
			return 1.0 - 3.0 * aA2 + 3.0 * aA1;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00005649 File Offset: 0x00003849
		private static double BY(double aA0, double aA1, double aA2)
		{
			return 3.0 * aA2 - 6.0 * aA1 + 3.0 * aA0;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000566E File Offset: 0x0000386E
		private static double B(double aA1, double aA2)
		{
			return 3.0 * aA2 - 6.0 * aA1;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00005687 File Offset: 0x00003887
		private static double CY(double aA0, double aA1)
		{
			return 3.0 * aA1 - 3.0 * aA0;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x000056A0 File Offset: 0x000038A0
		private static double C(double aA1)
		{
			return 3.0 * aA1;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000056AD File Offset: 0x000038AD
		private static double DY(double aA0)
		{
			return aA0;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000056B0 File Offset: 0x000038B0
		private static double CalcBezierY(double aT, double aA0, double aA1, double aA2, double aA3)
		{
			return ((CubicBezier.AY(aA0, aA1, aA2, aA3) * aT + CubicBezier.BY(aA0, aA1, aA2)) * aT + CubicBezier.CY(aA0, aA1)) * aT + CubicBezier.DY(aA0);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x000056DA File Offset: 0x000038DA
		private static double CalcBezierX(double aT, double aA1, double aA2)
		{
			return ((CubicBezier.A(aA1, aA2) * aT + CubicBezier.B(aA1, aA2)) * aT + CubicBezier.C(aA1)) * aT;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000056F8 File Offset: 0x000038F8
		private static double GetSlopeX(double aT, double aA1, double aA2)
		{
			return 3.0 * CubicBezier.A(aA1, aA2) * aT * aT + 2.0 * CubicBezier.B(aA1, aA2) * aT + CubicBezier.C(aA1);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000572C File Offset: 0x0000392C
		private static double BinarySubdivide(double aX, double aA, double aB, double mX1, double mX2)
		{
			int num = 0;
			double num2;
			double num3;
			do
			{
				num2 = aA + (aB - aA) / 2.0;
				num3 = CubicBezier.CalcBezierX(num2, mX1, mX2) - aX;
				if (num3 > 0.0)
				{
					aB = num2;
				}
				else
				{
					aA = num2;
				}
			}
			while (MathF.Abs(num3) > 1E-07 && ++num < 10);
			return num2;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00005788 File Offset: 0x00003988
		private double NewtonRaphsonIterate(double aX, double aGuessT, double mX1, double mX2)
		{
			for (int i = 0; i < 4; i++)
			{
				double slopeX = CubicBezier.GetSlopeX(aGuessT, mX1, mX2);
				if (MathF.Abs(slopeX) < 1E-07)
				{
					return aGuessT;
				}
				double num = CubicBezier.CalcBezierX(aGuessT, mX1, mX2) - aX;
				aGuessT -= num / slopeX;
			}
			return aGuessT;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000057D4 File Offset: 0x000039D4
		private double GetTForX(double aX)
		{
			double num = 0.0;
			int num2 = 1;
			int num3 = 10;
			while (num2 != num3 && this._sampleValues[num2] <= aX)
			{
				num += 0.1;
				num2++;
			}
			num2--;
			double num4 = (aX - this._sampleValues[num2]) / (this._sampleValues[num2 + 1] - this._sampleValues[num2]);
			double num5 = num + num4 * 0.1;
			double slopeX = CubicBezier.GetSlopeX(num5, this._x1, this._x2);
			double num6;
			if (slopeX >= 0.001)
			{
				num6 = this.NewtonRaphsonIterate(aX, num5, this._x1, this._x2);
			}
			else if (CubicBezier.AlmostEq(slopeX, 0.0))
			{
				num6 = num5;
			}
			else
			{
				num6 = CubicBezier.BinarySubdivide(aX, num, num + 0.1, this._x1, this._x2);
			}
			return num6;
		}

		// Token: 0x04000078 RID: 120
		private readonly double _x1;

		// Token: 0x04000079 RID: 121
		private readonly double _y1;

		// Token: 0x0400007A RID: 122
		private readonly double _x2;

		// Token: 0x0400007B RID: 123
		private readonly double _y2;

		// Token: 0x0400007C RID: 124
		private readonly double _y0;

		// Token: 0x0400007D RID: 125
		private readonly double _y3;

		// Token: 0x0400007E RID: 126
		private const int NewtonIterations = 4;

		// Token: 0x0400007F RID: 127
		private const double NewtonMinSlope = 0.001;

		// Token: 0x04000080 RID: 128
		private const double SubdivisionPrecision = 1E-07;

		// Token: 0x04000081 RID: 129
		private const int SubdivisionMaxIterations = 10;

		// Token: 0x04000082 RID: 130
		private const int KSplineTableSize = 11;

		// Token: 0x04000083 RID: 131
		private const double KSampleStepSize = 0.1;

		// Token: 0x04000084 RID: 132
		private readonly double[] _sampleValues = new double[11];
	}
}
