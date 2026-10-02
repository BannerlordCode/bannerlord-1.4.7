using System;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.ViewModelCollection;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission
{
	// Token: 0x0200002D RID: 45
	[OverrideView(typeof(MissionBoundaryCrossingView))]
	public class MissionGauntletBoundaryCrossingView : MissionBattleUIBaseView
	{
		// Token: 0x060001D4 RID: 468 RVA: 0x0000AC98 File Offset: 0x00008E98
		protected override void OnCreateView()
		{
			this._dataSource = new BoundaryCrossingVM(base.Mission, new Action<bool>(this.OnEscapeMenuToggled));
			this._gauntletLayer = new GauntletLayer("BoundaryCrossing", 47, false);
			this._gauntletLayer.LoadMovie("BoundaryCrossing", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000ACFD File Offset: 0x00008EFD
		protected override void OnDestroyView()
		{
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000AD18 File Offset: 0x00008F18
		protected override void OnSuspendView()
		{
			if (this._gauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._gauntletLayer, true);
			}
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000AD2E File Offset: 0x00008F2E
		protected override void OnResumeView()
		{
			if (this._gauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._gauntletLayer, false);
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000AD44 File Offset: 0x00008F44
		private void OnEscapeMenuToggled(bool isOpened)
		{
			if (base.IsViewCreated)
			{
				ScreenManager.SetSuspendLayer(this._gauntletLayer, !isOpened);
			}
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000AD5D File Offset: 0x00008F5D
		public override void OnPhotoModeActivated()
		{
			base.OnPhotoModeActivated();
			if (base.IsViewCreated)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 0f;
			}
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000AD82 File Offset: 0x00008F82
		public override void OnPhotoModeDeactivated()
		{
			base.OnPhotoModeDeactivated();
			if (base.IsViewCreated)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 1f;
			}
		}

		// Token: 0x040000EF RID: 239
		private GauntletLayer _gauntletLayer;

		// Token: 0x040000F0 RID: 240
		private BoundaryCrossingVM _dataSource;
	}
}
