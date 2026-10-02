using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Options
{
	// Token: 0x02000076 RID: 118
	public class OptionsBrightnessImageSliderWidget : SliderWidget
	{
		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000652 RID: 1618 RVA: 0x00012A0C File Offset: 0x00010C0C
		// (set) Token: 0x06000653 RID: 1619 RVA: 0x00012A14 File Offset: 0x00010C14
		public bool IsMax { get; set; }

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x00012A1D File Offset: 0x00010C1D
		// (set) Token: 0x06000655 RID: 1621 RVA: 0x00012A25 File Offset: 0x00010C25
		public Widget ImageWidget { get; set; }

		// Token: 0x06000656 RID: 1622 RVA: 0x00012A2E File Offset: 0x00010C2E
		public OptionsBrightnessImageSliderWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00012A38 File Offset: 0x00010C38
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._isInitialized)
			{
				float num;
				if (this.IsMax)
				{
					num = (float)(base.ValueInt - 1) * 0.003f + 1f;
				}
				else
				{
					num = (float)(base.ValueInt + 1) * 0.003f;
				}
				this.SetColorOfImage(MBMath.ClampFloat(num, 0f, 1f));
				this._isInitialized = true;
			}
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00012AA8 File Offset: 0x00010CA8
		protected override void OnValueFloatChanged(float value)
		{
			base.OnValueFloatChanged(value);
			float num;
			if (this.IsMax)
			{
				num = (value - 1f) * 0.003f + 1f;
			}
			else
			{
				num = (value + 1f) * 0.003f;
			}
			this.SetColorOfImage(MBMath.ClampFloat(num, 0f, 1f));
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x00012B04 File Offset: 0x00010D04
		private void SetColorOfImage(float value)
		{
			this.ImageWidget.Color = new Color(value, value, value, 1f);
		}

		// Token: 0x040002B8 RID: 696
		private bool _isInitialized;
	}
}
