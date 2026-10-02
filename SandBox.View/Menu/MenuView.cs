using System;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;

namespace SandBox.View.Menu
{
	// Token: 0x0200003A RID: 58
	public abstract class MenuView : SandboxView
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x00013004 File Offset: 0x00011204
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x0001300C File Offset: 0x0001120C
		internal bool Removed { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x00013015 File Offset: 0x00011215
		public virtual bool ShouldUpdateMenuAfterRemoved
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00013018 File Offset: 0x00011218
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x00013020 File Offset: 0x00011220
		public MenuViewContext MenuViewContext { get; internal set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00013029 File Offset: 0x00011229
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x00013031 File Offset: 0x00011231
		public MenuContext MenuContext { get; internal set; }

		// Token: 0x060001B7 RID: 439 RVA: 0x0001303A File Offset: 0x0001123A
		protected internal virtual void OnMenuContextUpdated(MenuContext newMenuContext)
		{
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0001303C File Offset: 0x0001123C
		protected internal virtual void OnMenuContextRefreshed()
		{
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0001303E File Offset: 0x0001123E
		protected internal virtual void OnOverlayTypeChange(GameMenu.MenuOverlayType newType)
		{
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00013040 File Offset: 0x00011240
		protected internal virtual void OnCharacterDeveloperOpened()
		{
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00013042 File Offset: 0x00011242
		protected internal virtual void OnCharacterDeveloperClosed()
		{
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00013044 File Offset: 0x00011244
		protected internal virtual void OnBackgroundMeshNameSet(string name)
		{
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00013046 File Offset: 0x00011246
		protected internal virtual void OnHourlyTick()
		{
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00013048 File Offset: 0x00011248
		protected internal virtual void OnResume()
		{
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0001304A File Offset: 0x0001124A
		protected internal virtual void OnMapConversationActivated()
		{
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0001304C File Offset: 0x0001124C
		protected internal virtual void OnMapConversationDeactivated()
		{
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0001304E File Offset: 0x0001124E
		protected internal virtual TutorialContexts GetTutorialContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x040000F8 RID: 248
		protected const float ContextAlphaModifier = 8.5f;
	}
}
