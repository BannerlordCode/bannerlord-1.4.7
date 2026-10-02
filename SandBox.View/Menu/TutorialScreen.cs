using System;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.ScreenSystem;

namespace SandBox.View.Menu
{
	// Token: 0x0200003C RID: 60
	[GameStateScreen(typeof(TutorialState))]
	public class TutorialScreen : ScreenBase, IGameStateListener
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x0001397E File Offset: 0x00011B7E
		public MenuViewContext MenuViewContext { get; }

		// Token: 0x060001F5 RID: 501 RVA: 0x00013986 File Offset: 0x00011B86
		public TutorialScreen(TutorialState tutorialState)
		{
			this.MenuViewContext = new MenuViewContext(this, tutorialState.MenuContext);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x000139A0 File Offset: 0x00011BA0
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			this.MenuViewContext.OnFrameTick(dt);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x000139B5 File Offset: 0x00011BB5
		protected override void OnActivate()
		{
			base.OnActivate();
			this.MenuViewContext.OnActivate();
			LoadingWindow.DisableGlobalLoadingWindow();
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x000139CD File Offset: 0x00011BCD
		protected override void OnDeactivate()
		{
			this.MenuViewContext.OnDeactivate();
			base.OnDeactivate();
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x000139E0 File Offset: 0x00011BE0
		protected override void OnInitialize()
		{
			base.OnInitialize();
			this.MenuViewContext.OnInitialize();
		}

		// Token: 0x060001FA RID: 506 RVA: 0x000139F3 File Offset: 0x00011BF3
		protected override void OnFinalize()
		{
			this.MenuViewContext.OnFinalize();
			base.OnFinalize();
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00013A06 File Offset: 0x00011C06
		void IGameStateListener.OnActivate()
		{
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00013A08 File Offset: 0x00011C08
		void IGameStateListener.OnDeactivate()
		{
			this.MenuViewContext.OnGameStateDeactivate();
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00013A15 File Offset: 0x00011C15
		void IGameStateListener.OnInitialize()
		{
			this.MenuViewContext.OnGameStateInitialize();
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00013A22 File Offset: 0x00011C22
		void IGameStateListener.OnFinalize()
		{
			this.MenuViewContext.OnGameStateFinalize();
		}
	}
}
