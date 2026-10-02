using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.GameOver
{
	// Token: 0x0200014F RID: 335
	public class GameOverCategoryButtonWidget : ButtonWidget
	{
		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x060011C7 RID: 4551 RVA: 0x000316E5 File Offset: 0x0002F8E5
		// (set) Token: 0x060011C8 RID: 4552 RVA: 0x000316ED File Offset: 0x0002F8ED
		public string CategoryID { get; set; }

		// Token: 0x060011C9 RID: 4553 RVA: 0x000316F6 File Offset: 0x0002F8F6
		public GameOverCategoryButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x000316FF File Offset: 0x0002F8FF
		protected override void HandleClick()
		{
			this.HandleSoundEvent();
			base.HandleClick();
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x0003170D File Offset: 0x0002F90D
		private void HandleSoundEvent()
		{
			base.EventFired(this.CategoryID, Array.Empty<object>());
		}
	}
}
