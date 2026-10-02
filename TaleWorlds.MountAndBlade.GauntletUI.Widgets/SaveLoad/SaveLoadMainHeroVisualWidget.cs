using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.SaveLoad
{
	// Token: 0x0200005C RID: 92
	public class SaveLoadMainHeroVisualWidget : Widget
	{
		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x0000F758 File Offset: 0x0000D958
		// (set) Token: 0x060004FF RID: 1279 RVA: 0x0000F760 File Offset: 0x0000D960
		public Widget DefaultVisualWidget { get; set; }

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x0000F769 File Offset: 0x0000D969
		// (set) Token: 0x06000501 RID: 1281 RVA: 0x0000F771 File Offset: 0x0000D971
		public SaveLoadHeroTableauWidget SaveLoadHeroTableau { get; set; }

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x0000F77A File Offset: 0x0000D97A
		// (set) Token: 0x06000503 RID: 1283 RVA: 0x0000F782 File Offset: 0x0000D982
		public bool IsVisualDisabledForMemoryPurposes { get; set; }

		// Token: 0x06000504 RID: 1284 RVA: 0x0000F78B File Offset: 0x0000D98B
		public SaveLoadMainHeroVisualWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0000F794 File Offset: 0x0000D994
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.DefaultVisualWidget != null)
			{
				if (this.IsVisualDisabledForMemoryPurposes)
				{
					this.DefaultVisualWidget.IsVisible = true;
					this.SaveLoadHeroTableau.IsVisible = false;
					return;
				}
				this.DefaultVisualWidget.IsVisible = string.IsNullOrEmpty(this.SaveLoadHeroTableau.HeroVisualCode) || !this.SaveLoadHeroTableau.IsVersionCompatible;
				this.SaveLoadHeroTableau.IsVisible = !this.DefaultVisualWidget.IsVisible;
			}
		}
	}
}
