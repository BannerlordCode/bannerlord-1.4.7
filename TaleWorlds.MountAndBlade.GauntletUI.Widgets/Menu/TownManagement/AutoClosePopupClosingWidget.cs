using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.TownManagement
{
	// Token: 0x02000105 RID: 261
	public class AutoClosePopupClosingWidget : Widget
	{
		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06000DED RID: 3565 RVA: 0x000262AB File Offset: 0x000244AB
		// (set) Token: 0x06000DEE RID: 3566 RVA: 0x000262B3 File Offset: 0x000244B3
		public Widget Target { get; set; }

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06000DEF RID: 3567 RVA: 0x000262BC File Offset: 0x000244BC
		// (set) Token: 0x06000DF0 RID: 3568 RVA: 0x000262C4 File Offset: 0x000244C4
		public bool IncludeChildren { get; set; }

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06000DF1 RID: 3569 RVA: 0x000262CD File Offset: 0x000244CD
		// (set) Token: 0x06000DF2 RID: 3570 RVA: 0x000262D5 File Offset: 0x000244D5
		public bool IncludeTarget { get; set; }

		// Token: 0x06000DF3 RID: 3571 RVA: 0x000262DE File Offset: 0x000244DE
		public AutoClosePopupClosingWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x000262E8 File Offset: 0x000244E8
		public bool ShouldClosePopup()
		{
			if (this.IncludeTarget && base.EventManager.LatestMouseUpWidget == this.Target)
			{
				return true;
			}
			if (this.IncludeChildren)
			{
				Widget target = this.Target;
				return target != null && target.CheckIsMyChildRecursive(base.EventManager.LatestMouseUpWidget);
			}
			return false;
		}
	}
}
