using System;
using TaleWorlds.Library;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200000A RID: 10
	public static class AnimationInterpolation
	{
		// Token: 0x0600006E RID: 110 RVA: 0x00003068 File Offset: 0x00001268
		public static float Ease(AnimationInterpolation.Type type, AnimationInterpolation.Function function, float ratio)
		{
			switch (type)
			{
			case AnimationInterpolation.Type.Linear:
				return ratio;
			case AnimationInterpolation.Type.EaseIn:
				return default(AnimationInterpolation.EaseInInterpolator).Ease(function, ratio);
			case AnimationInterpolation.Type.EaseOut:
				return default(AnimationInterpolation.EaseOutInterpolator).Ease(function, ratio);
			case AnimationInterpolation.Type.EaseInOut:
				return default(AnimationInterpolation.EaseInOutInterpolator).Ease(function, ratio);
			default:
				Debug.FailedAssert(string.Format("Brush interpolation type not implemented: {0}", type), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\Brush\\AnimationInterpolation.cs", "Ease", 41);
				return ratio;
			}
		}

		// Token: 0x02000071 RID: 113
		public enum Type
		{
			// Token: 0x040003EE RID: 1006
			Linear,
			// Token: 0x040003EF RID: 1007
			EaseIn,
			// Token: 0x040003F0 RID: 1008
			EaseOut,
			// Token: 0x040003F1 RID: 1009
			EaseInOut
		}

		// Token: 0x02000072 RID: 114
		public enum Function
		{
			// Token: 0x040003F3 RID: 1011
			Sine,
			// Token: 0x040003F4 RID: 1012
			Quad,
			// Token: 0x040003F5 RID: 1013
			Cubic,
			// Token: 0x040003F6 RID: 1014
			Quart,
			// Token: 0x040003F7 RID: 1015
			Quint
		}

		// Token: 0x02000073 RID: 115
		private struct EaseInInterpolator
		{
			// Token: 0x060008D7 RID: 2263 RVA: 0x00022D94 File Offset: 0x00020F94
			public float Ease(AnimationInterpolation.Function function, float t)
			{
				switch (function)
				{
				case AnimationInterpolation.Function.Sine:
					return 1f - MathF.Cos(t * 3.1415927f / 2f);
				case AnimationInterpolation.Function.Quad:
					return t * t;
				case AnimationInterpolation.Function.Cubic:
					return t * t * t;
				case AnimationInterpolation.Function.Quart:
					return t * t * t * t;
				case AnimationInterpolation.Function.Quint:
					return t * t * t * t * t;
				default:
					Debug.FailedAssert(string.Format("Brush ease function not implemented: {0}", function), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\Brush\\AnimationInterpolation.cs", "Ease", 63);
					return t;
				}
			}
		}

		// Token: 0x02000074 RID: 116
		private struct EaseOutInterpolator
		{
			// Token: 0x060008D8 RID: 2264 RVA: 0x00022E14 File Offset: 0x00021014
			public float Ease(AnimationInterpolation.Function function, float t)
			{
				switch (function)
				{
				case AnimationInterpolation.Function.Sine:
					return MathF.Sin(t * 3.1415927f / 2f);
				case AnimationInterpolation.Function.Quad:
					return 1f - (1f - t) * (1f - t);
				case AnimationInterpolation.Function.Cubic:
					return 1f - MathF.Pow(1f - t, 3f);
				case AnimationInterpolation.Function.Quart:
					return 1f - MathF.Pow(1f - t, 4f);
				case AnimationInterpolation.Function.Quint:
					return 1f - MathF.Pow(1f - t, 5f);
				default:
					Debug.FailedAssert(string.Format("Brush ease function not implemented: {0}", function), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\Brush\\AnimationInterpolation.cs", "Ease", 86);
					return t;
				}
			}
		}

		// Token: 0x02000075 RID: 117
		private struct EaseInOutInterpolator
		{
			// Token: 0x060008D9 RID: 2265 RVA: 0x00022ED0 File Offset: 0x000210D0
			public float Ease(AnimationInterpolation.Function function, float t)
			{
				switch (function)
				{
				case AnimationInterpolation.Function.Sine:
					return -(MathF.Cos(3.1415927f * t) - 1f) / 2f;
				case AnimationInterpolation.Function.Quad:
					if (t >= 0.5f)
					{
						return 1f - MathF.Pow(-2f * t + 2f, 2f) / 2f;
					}
					return 2f * t * t;
				case AnimationInterpolation.Function.Cubic:
					if (t >= 0.5f)
					{
						return 1f - MathF.Pow(-2f * t + 2f, 3f) / 2f;
					}
					return 4f * t * t * t;
				case AnimationInterpolation.Function.Quart:
					if (t >= 0.5f)
					{
						return 1f - MathF.Pow(-2f * t + 2f, 4f) / 2f;
					}
					return 8f * t * t * t * t;
				case AnimationInterpolation.Function.Quint:
					if (t >= 0.5f)
					{
						return 1f - MathF.Pow(-2f * t + 2f, 5f) / 2f;
					}
					return 16f * t * t * t * t * t;
				default:
					Debug.FailedAssert(string.Format("Brush ease function not implemented: {0}", function), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\Brush\\AnimationInterpolation.cs", "Ease", 115);
					return t;
				}
			}
		}
	}
}
