using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200021E RID: 542
	public struct FactoredNumber
	{
		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x06001F69 RID: 8041 RVA: 0x0006CCD7 File Offset: 0x0006AED7
		public float ResultNumber
		{
			get
			{
				return MathF.Clamp(this.BaseNumber + this.BaseNumber * this._sumOfFactors, this.LimitMinValue, this.LimitMaxValue);
			}
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x06001F6A RID: 8042 RVA: 0x0006CCFE File Offset: 0x0006AEFE
		// (set) Token: 0x06001F6B RID: 8043 RVA: 0x0006CD06 File Offset: 0x0006AF06
		public float BaseNumber { get; private set; }

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x06001F6C RID: 8044 RVA: 0x0006CD0F File Offset: 0x0006AF0F
		public float LimitMinValue
		{
			get
			{
				return this._limitMinValue;
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x06001F6D RID: 8045 RVA: 0x0006CD18 File Offset: 0x0006AF18
		public float LimitMaxValue
		{
			get
			{
				return this._limitMaxValue;
			}
		}

		// Token: 0x06001F6E RID: 8046 RVA: 0x0006CD21 File Offset: 0x0006AF21
		public FactoredNumber(float baseNumber = 0f)
		{
			this.BaseNumber = baseNumber;
			this._sumOfFactors = 0f;
			this._limitMinValue = float.MinValue;
			this._limitMaxValue = float.MaxValue;
		}

		// Token: 0x06001F6F RID: 8047 RVA: 0x0006CD4B File Offset: 0x0006AF4B
		public void Add(float value)
		{
			if (value.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				return;
			}
			this.BaseNumber += value;
		}

		// Token: 0x06001F70 RID: 8048 RVA: 0x0006CD6E File Offset: 0x0006AF6E
		public void AddFactor(float value)
		{
			if (value.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				return;
			}
			this._sumOfFactors += value;
		}

		// Token: 0x06001F71 RID: 8049 RVA: 0x0006CD91 File Offset: 0x0006AF91
		public void LimitMin(float minValue)
		{
			this._limitMinValue = minValue;
		}

		// Token: 0x06001F72 RID: 8050 RVA: 0x0006CD9A File Offset: 0x0006AF9A
		public void LimitMax(float maxValue)
		{
			this._limitMaxValue = maxValue;
		}

		// Token: 0x06001F73 RID: 8051 RVA: 0x0006CDA3 File Offset: 0x0006AFA3
		public void Clamp(float minValue, float maxValue)
		{
			this.LimitMin(minValue);
			this.LimitMax(maxValue);
		}

		// Token: 0x04000AD9 RID: 2777
		private float _limitMinValue;

		// Token: 0x04000ADA RID: 2778
		private float _limitMaxValue;

		// Token: 0x04000ADB RID: 2779
		private float _sumOfFactors;
	}
}
