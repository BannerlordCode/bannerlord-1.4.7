using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000E1 RID: 225
	public class MissionLeaveBarSliderWidget : SliderWidget
	{
		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06000B9D RID: 2973 RVA: 0x000205C9 File Offset: 0x0001E7C9
		private float CurrentAlpha
		{
			get
			{
				return base.ReadOnlyBrush.GlobalAlphaFactor;
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06000B9E RID: 2974 RVA: 0x000205D6 File Offset: 0x0001E7D6
		// (set) Token: 0x06000B9F RID: 2975 RVA: 0x000205DE File Offset: 0x0001E7DE
		public float FadeInMultiplier { get; set; } = 1f;

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06000BA0 RID: 2976 RVA: 0x000205E7 File Offset: 0x0001E7E7
		// (set) Token: 0x06000BA1 RID: 2977 RVA: 0x000205EF File Offset: 0x0001E7EF
		public float FadeOutMultiplier { get; set; } = 1f;

		// Token: 0x06000BA2 RID: 2978 RVA: 0x000205F8 File Offset: 0x0001E7F8
		public MissionLeaveBarSliderWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x00020618 File Offset: 0x0001E818
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				this.SetGlobalAlphaRecursively(0f);
				this._initialized = true;
			}
			float num = ((base.ValueFloat > 0f) ? this.FadeInMultiplier : this.FadeOutMultiplier);
			float num2 = (float)((base.ValueFloat > 0f) ? 1 : 0);
			float num3 = Mathf.Clamp(Mathf.Lerp(this.CurrentAlpha, num2, num * 0.2f), 0f, 1f);
			this.SetGlobalAlphaRecursively(num3);
		}

		// Token: 0x0400053D RID: 1341
		private bool _initialized;
	}
}
