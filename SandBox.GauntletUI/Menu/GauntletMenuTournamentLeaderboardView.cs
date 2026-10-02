using System;
using SandBox.View.Menu;
using TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TournamentLeaderboard;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Menu
{
	// Token: 0x02000028 RID: 40
	[OverrideView(typeof(MenuTournamentLeaderboardView))]
	public class GauntletMenuTournamentLeaderboardView : MenuView
	{
		// Token: 0x06000209 RID: 521 RVA: 0x0000CEDC File Offset: 0x0000B0DC
		protected override void OnInitialize()
		{
			base.OnInitialize();
			this._dataSource = new TournamentLeaderboardVM
			{
				IsEnabled = true
			};
			base.Layer = new GauntletLayer("MapTournamentLeaderboard", 206, false);
			this._layerAsGauntletLayer = base.Layer as GauntletLayer;
			base.Layer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			base.Layer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			this._dataSource.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			this._movie = this._layerAsGauntletLayer.LoadMovie("GameMenuTournamentLeaderboard", this._dataSource);
			base.Layer.IsFocusLayer = true;
			ScreenManager.TrySetFocus(base.Layer);
			base.MenuViewContext.AddLayer(base.Layer);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000CFB8 File Offset: 0x0000B1B8
		protected override void OnFinalize()
		{
			base.Layer.IsFocusLayer = false;
			ScreenManager.TryLoseFocus(base.Layer);
			this._dataSource.OnFinalize();
			this._dataSource = null;
			this._layerAsGauntletLayer.ReleaseMovie(this._movie);
			base.MenuViewContext.RemoveLayer(base.Layer);
			this._movie = null;
			base.Layer = null;
			base.OnFinalize();
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000D024 File Offset: 0x0000B224
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			if (base.Layer.Input.IsHotKeyReleased("Exit") || base.Layer.Input.IsHotKeyReleased("Confirm"))
			{
				UISoundsHelper.PlayUISound("event:/ui/default");
				this._dataSource.IsEnabled = false;
			}
			if (!this._dataSource.IsEnabled)
			{
				base.MenuViewContext.CloseTournamentLeaderboard();
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000D094 File Offset: 0x0000B294
		protected override void OnMapConversationActivated()
		{
			base.OnMapConversationActivated();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, true);
			}
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000D0B0 File Offset: 0x0000B2B0
		protected override void OnMapConversationDeactivated()
		{
			base.OnMapConversationDeactivated();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, false);
			}
		}

		// Token: 0x040000A9 RID: 169
		private GauntletLayer _layerAsGauntletLayer;

		// Token: 0x040000AA RID: 170
		private TournamentLeaderboardVM _dataSource;

		// Token: 0x040000AB RID: 171
		private GauntletMovieIdentifier _movie;
	}
}
