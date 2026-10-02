using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.PlatformService;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200038A RID: 906
	public class MBProfileSelectionScreenBase : ScreenBase, IGameStateListener
	{
		// Token: 0x060033FD RID: 13309 RVA: 0x000D6436 File Offset: 0x000D4636
		public MBProfileSelectionScreenBase(ProfileSelectionState state)
		{
			this._state = state;
		}

		// Token: 0x060033FE RID: 13310 RVA: 0x000D6445 File Offset: 0x000D4645
		protected sealed override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			if (ScreenManager.TopScreen == this)
			{
				this.OnProfileSelectionTick(dt);
			}
		}

		// Token: 0x060033FF RID: 13311 RVA: 0x000D645D File Offset: 0x000D465D
		protected virtual void OnProfileSelectionTick(float dt)
		{
		}

		// Token: 0x06003400 RID: 13312 RVA: 0x000D645F File Offset: 0x000D465F
		protected void OnActivateProfileSelection()
		{
			PlatformServices.Instance.LoginUser();
		}

		// Token: 0x06003401 RID: 13313 RVA: 0x000D646B File Offset: 0x000D466B
		void IGameStateListener.OnActivate()
		{
		}

		// Token: 0x06003402 RID: 13314 RVA: 0x000D646D File Offset: 0x000D466D
		void IGameStateListener.OnDeactivate()
		{
		}

		// Token: 0x06003403 RID: 13315 RVA: 0x000D646F File Offset: 0x000D466F
		void IGameStateListener.OnFinalize()
		{
		}

		// Token: 0x06003404 RID: 13316 RVA: 0x000D6471 File Offset: 0x000D4671
		void IGameStateListener.OnInitialize()
		{
			Utilities.DisableGlobalLoadingWindow();
			LoadingWindow.DisableGlobalLoadingWindow();
		}

		// Token: 0x040015F0 RID: 5616
		private ProfileSelectionState _state;
	}
}
