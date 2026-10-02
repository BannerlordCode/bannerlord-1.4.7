using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenu;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission
{
	// Token: 0x02000030 RID: 48
	public abstract class MissionGauntletEscapeMenuBase : MissionEscapeMenuView
	{
		// Token: 0x060001F7 RID: 503 RVA: 0x0000B8E2 File Offset: 0x00009AE2
		protected MissionGauntletEscapeMenuBase(string viewFile)
		{
			base.OnMissionScreenInitialize();
			this._viewFile = viewFile;
			this.ViewOrderPriority = 50;
			Game.Current.EventManager.RegisterEvent<TutorialContextChangedEvent>(new Action<TutorialContextChangedEvent>(this.OnTutorialContextChanged));
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000B91A File Offset: 0x00009B1A
		protected virtual List<EscapeMenuItemVM> GetEscapeMenuItems()
		{
			return null;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000B920 File Offset: 0x00009B20
		public override void OnMissionScreenFinalize()
		{
			Game.Current.EventManager.UnregisterEvent<TutorialContextChangedEvent>(new Action<TutorialContextChangedEvent>(this.OnTutorialContextChanged));
			this.DataSource.OnFinalize();
			this.DataSource = null;
			this._gauntletLayer = null;
			this._movie = null;
			base.OnMissionScreenFinalize();
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000B96E File Offset: 0x00009B6E
		public override bool OnEscape()
		{
			if (!this._isRenderingStarted)
			{
				return false;
			}
			if (!base.IsActive)
			{
				this.DataSource.RefreshItems(this.GetEscapeMenuItems());
			}
			return this.OnEscapeMenuToggled(!base.IsActive);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000B9A4 File Offset: 0x00009BA4
		protected bool OnEscapeMenuToggled(bool isOpened)
		{
			if (base.IsActive == isOpened)
			{
				return false;
			}
			base.IsActive = isOpened;
			if (isOpened)
			{
				Game.Current.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(TutorialContexts.EscapeMenu));
				this.DataSource.RefreshValues();
				if (!GameNetwork.IsMultiplayer)
				{
					MBCommon.PauseGameEngine();
					Game.Current.GameStateManager.RegisterActiveStateDisableRequest(this);
				}
				this._gauntletLayer = new GauntletLayer("MissionEscapeMenu", this.ViewOrderPriority, false);
				this._gauntletLayer.IsFocusLayer = true;
				this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
				this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
				this._movie = this._gauntletLayer.LoadMovie(this._viewFile, this.DataSource);
				base.MissionScreen.AddLayer(this._gauntletLayer);
				ScreenManager.TrySetFocus(this._gauntletLayer);
			}
			else
			{
				Game.Current.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(this._escapeMenuPrevTutorialContext));
				if (!GameNetwork.IsMultiplayer)
				{
					MBCommon.UnPauseGameEngine();
					Game.Current.GameStateManager.UnregisterActiveStateDisableRequest(this);
				}
				this._gauntletLayer.InputRestrictions.ResetInputRestrictions();
				base.MissionScreen.RemoveLayer(this._gauntletLayer);
				this._movie = null;
				this._gauntletLayer = null;
			}
			return true;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000BAF8 File Offset: 0x00009CF8
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (base.IsActive && (this._gauntletLayer.Input.IsHotKeyReleased("ToggleEscapeMenu") || this._gauntletLayer.Input.IsHotKeyReleased("Exit")))
			{
				this.OnEscapeMenuToggled(false);
			}
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000BB4A File Offset: 0x00009D4A
		public override void OnSceneRenderingStarted()
		{
			base.OnSceneRenderingStarted();
			this._isRenderingStarted = true;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000BB59 File Offset: 0x00009D59
		private void OnTutorialContextChanged(TutorialContextChangedEvent obj)
		{
			if (obj.NewContext != TutorialContexts.EscapeMenu)
			{
				this._escapeMenuPrevTutorialContext = obj.NewContext;
			}
		}

		// Token: 0x040000F9 RID: 249
		protected EscapeMenuVM DataSource;

		// Token: 0x040000FA RID: 250
		private GauntletLayer _gauntletLayer;

		// Token: 0x040000FB RID: 251
		private GauntletMovieIdentifier _movie;

		// Token: 0x040000FC RID: 252
		private string _viewFile;

		// Token: 0x040000FD RID: 253
		private bool _isRenderingStarted;

		// Token: 0x040000FE RID: 254
		private TutorialContexts _escapeMenuPrevTutorialContext;
	}
}
