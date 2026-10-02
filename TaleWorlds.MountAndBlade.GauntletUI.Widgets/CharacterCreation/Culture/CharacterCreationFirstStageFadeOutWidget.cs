using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterCreation.Culture
{
	// Token: 0x0200018C RID: 396
	public class CharacterCreationFirstStageFadeOutWidget : Widget
	{
		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06001482 RID: 5250 RVA: 0x00037CEF File Offset: 0x00035EEF
		// (set) Token: 0x06001483 RID: 5251 RVA: 0x00037CF7 File Offset: 0x00035EF7
		public float StayTime { get; set; } = 1.5f;

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06001484 RID: 5252 RVA: 0x00037D00 File Offset: 0x00035F00
		// (set) Token: 0x06001485 RID: 5253 RVA: 0x00037D08 File Offset: 0x00035F08
		public float FadeOutTime { get; set; } = 1.5f;

		// Token: 0x06001486 RID: 5254 RVA: 0x00037D11 File Offset: 0x00035F11
		public CharacterCreationFirstStageFadeOutWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x00037D30 File Offset: 0x00035F30
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._totalTime < this.StayTime)
			{
				this.SetGlobalAlphaRecursively(1f);
				base.IsEnabled = true;
			}
			else if (this._totalTime > this.StayTime && this._totalTime < this.StayTime + this.FadeOutTime)
			{
				float num = Mathf.Lerp(1f, 0f, (this._totalTime - this.StayTime) / this.FadeOutTime);
				this.SetGlobalAlphaRecursively(num);
				base.IsEnabled = num > 0.2f;
			}
			else
			{
				this.SetGlobalAlphaRecursively(0f);
				base.IsEnabled = false;
			}
			this._totalTime += dt;
		}

		// Token: 0x0400094F RID: 2383
		private float _totalTime;
	}
}
