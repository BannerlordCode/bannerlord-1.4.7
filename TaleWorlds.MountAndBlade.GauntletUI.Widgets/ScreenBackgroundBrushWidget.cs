using System;
using System.Linq;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200003D RID: 61
	public class ScreenBackgroundBrushWidget : BrushWidget
	{
		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600038E RID: 910 RVA: 0x0000B62E File Offset: 0x0000982E
		// (set) Token: 0x0600038F RID: 911 RVA: 0x0000B636 File Offset: 0x00009836
		public bool IsParticleVisible { get; set; }

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000390 RID: 912 RVA: 0x0000B63F File Offset: 0x0000983F
		// (set) Token: 0x06000391 RID: 913 RVA: 0x0000B647 File Offset: 0x00009847
		public bool IsSmokeVisible { get; set; }

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000392 RID: 914 RVA: 0x0000B650 File Offset: 0x00009850
		// (set) Token: 0x06000393 RID: 915 RVA: 0x0000B658 File Offset: 0x00009858
		public bool IsFullscreenImageEnabled { get; set; }

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000394 RID: 916 RVA: 0x0000B661 File Offset: 0x00009861
		// (set) Token: 0x06000395 RID: 917 RVA: 0x0000B669 File Offset: 0x00009869
		public bool AnimEnabled { get; set; }

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000396 RID: 918 RVA: 0x0000B672 File Offset: 0x00009872
		// (set) Token: 0x06000397 RID: 919 RVA: 0x0000B67A File Offset: 0x0000987A
		public Widget ParticleWidget1 { get; set; }

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000398 RID: 920 RVA: 0x0000B683 File Offset: 0x00009883
		// (set) Token: 0x06000399 RID: 921 RVA: 0x0000B68B File Offset: 0x0000988B
		public Widget ParticleWidget2 { get; set; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600039A RID: 922 RVA: 0x0000B694 File Offset: 0x00009894
		// (set) Token: 0x0600039B RID: 923 RVA: 0x0000B69C File Offset: 0x0000989C
		public Widget SmokeWidget1 { get; set; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600039C RID: 924 RVA: 0x0000B6A5 File Offset: 0x000098A5
		// (set) Token: 0x0600039D RID: 925 RVA: 0x0000B6AD File Offset: 0x000098AD
		public Widget SmokeWidget2 { get; set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x0600039E RID: 926 RVA: 0x0000B6B6 File Offset: 0x000098B6
		// (set) Token: 0x0600039F RID: 927 RVA: 0x0000B6BE File Offset: 0x000098BE
		public float SmokeSpeedModifier { get; set; } = 1f;

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x0000B6C7 File Offset: 0x000098C7
		// (set) Token: 0x060003A1 RID: 929 RVA: 0x0000B6CF File Offset: 0x000098CF
		public float ParticleSpeedModifier { get; set; } = 1f;

		// Token: 0x060003A2 RID: 930 RVA: 0x0000B6D8 File Offset: 0x000098D8
		public ScreenBackgroundBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000B700 File Offset: 0x00009900
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._firstFrame)
			{
				this.UpdateBackgroundImage();
				this._firstFrame = false;
			}
			this.ParticleWidget1.IsVisible = this.IsParticleVisible;
			this.ParticleWidget2.IsVisible = this.IsParticleVisible;
			this.SmokeWidget1.IsVisible = this.IsSmokeVisible;
			this.SmokeWidget2.IsVisible = this.IsSmokeVisible;
			if (this.AnimEnabled)
			{
				if (this.IsParticleVisible)
				{
					this.ParticleWidget1.PositionXOffset = this._totalParticleXOffset;
					this.ParticleWidget2.PositionXOffset = this.ParticleWidget1.PositionXOffset + this.ParticleWidget1.SuggestedWidth;
					this._totalParticleXOffset -= dt * 10f * this.ParticleSpeedModifier;
					if (Math.Abs(this._totalParticleXOffset) >= this.ParticleWidget1.SuggestedWidth)
					{
						this._totalParticleXOffset = 0f;
					}
				}
				if (this.IsSmokeVisible)
				{
					this.SmokeWidget1.PositionXOffset = this._totalSmokeXOffset;
					this.SmokeWidget2.PositionXOffset = this.SmokeWidget1.PositionXOffset - this.SmokeWidget1.SuggestedWidth;
					if (Math.Abs(this._totalSmokeXOffset) >= this.SmokeWidget1.SuggestedWidth)
					{
						this._totalSmokeXOffset = 0f;
					}
					this._totalSmokeXOffset += dt * 10f * this.SmokeSpeedModifier;
				}
			}
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000B86C File Offset: 0x00009A6C
		private void UpdateBackgroundImage()
		{
			if (this.IsFullscreenImageEnabled)
			{
				int num = base.Context.UIRandom.Next(base.Brush.Styles.Count);
				StyleLayer[] layers = base.ReadOnlyBrush.Styles.ElementAt<Style>(num).GetLayers();
				if (layers.Length != 0)
				{
					base.Brush.Sprite = layers[0].Sprite;
					return;
				}
			}
			else
			{
				base.Brush.Sprite = null;
			}
		}

		// Token: 0x04000182 RID: 386
		private bool _firstFrame = true;

		// Token: 0x04000183 RID: 387
		private float _totalSmokeXOffset;

		// Token: 0x04000184 RID: 388
		private float _totalParticleXOffset;
	}
}
