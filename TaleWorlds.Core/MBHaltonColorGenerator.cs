using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x020000AE RID: 174
	public class MBHaltonColorGenerator
	{
		// Token: 0x17000321 RID: 801
		// (get) Token: 0x0600092A RID: 2346 RVA: 0x0001E0F8 File Offset: 0x0001C2F8
		public int Base
		{
			get
			{
				return this._base;
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x0600092B RID: 2347 RVA: 0x0001E100 File Offset: 0x0001C300
		public float Offset
		{
			get
			{
				return this._offset;
			}
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x0001E108 File Offset: 0x0001C308
		public MBHaltonColorGenerator()
		{
			this.SetRandomOffset();
			this.SetBase();
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x0001E11C File Offset: 0x0001C31C
		public void SetBase()
		{
			this._base = 2;
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0001E125 File Offset: 0x0001C325
		public void SetBase(int baseValue)
		{
			this._base = baseValue;
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0001E12E File Offset: 0x0001C32E
		public void SetOffset(float offset)
		{
			this._offset = MathF.Clamp(offset, 0f, 1f);
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0001E146 File Offset: 0x0001C346
		public void SetRandomOffset()
		{
			this._offset = MBRandom.RandomFloat;
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0001E153 File Offset: 0x0001C353
		public Color GetColor(int index, int maxIndex)
		{
			return Color.FromHSV(MBHaltonColorGenerator.HaltonSequence(((float)index / (float)maxIndex + this._offset) % 1f, this._base), 1f, 1f);
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0001E184 File Offset: 0x0001C384
		private static float HaltonSequence(float normalizedIndex, int baseValue)
		{
			float num = 1f;
			float num2 = 0f;
			for (float num3 = normalizedIndex * (float)baseValue; num3 > 0f; num3 = (float)Math.Floor((double)(num3 / (float)baseValue)))
			{
				num /= (float)baseValue;
				num2 += num3 % (float)baseValue * num;
			}
			return num2;
		}

		// Token: 0x04000518 RID: 1304
		public const int DefaultBase = 2;

		// Token: 0x04000519 RID: 1305
		private int _base;

		// Token: 0x0400051A RID: 1306
		private float _offset;
	}
}
