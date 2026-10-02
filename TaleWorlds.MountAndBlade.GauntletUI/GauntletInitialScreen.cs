using System;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Options;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;
using TaleWorlds.MountAndBlade.ViewModelCollection.InitialMenu;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI
{
	// Token: 0x02000014 RID: 20
	[GameStateScreen(typeof(InitialState))]
	public class GauntletInitialScreen : MBInitialScreenBase, IChatLogHandlerScreen
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x00005578 File Offset: 0x00003778
		public GauntletInitialScreen(InitialState initialState)
			: base(initialState)
		{
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00005584 File Offset: 0x00003784
		protected override void OnInitialize()
		{
			base.OnInitialize();
			this._dataSource = new InitialMenuVM(base._state);
			this._gauntletLayer = new GauntletLayer("MainMenu", 1, false);
			this._gauntletLayer.LoadMovie("InitialScreen", this._dataSource);
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.Mouse);
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			base.AddLayer(this._gauntletLayer);
			this._gauntletLayer.IsFocusLayer = true;
			ScreenManager.TrySetFocus(this._gauntletLayer);
			if (NativeOptions.GetConfig(NativeOptions.NativeOptionsType.BrightnessCalibrated) < 4f)
			{
				this._brightnessOptionDataSource = new BrightnessOptionVM(new Action<bool>(this.OnCloseBrightness))
				{
					Visible = true
				};
				this._gauntletBrightnessLayer = new GauntletLayer("MainMenuBrightness", 2, false);
				this._gauntletBrightnessLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.Mouse);
				this._brightnessOptionMovie = this._gauntletBrightnessLayer.LoadMovie("BrightnessOption", this._brightnessOptionDataSource);
				base.AddLayer(this._gauntletBrightnessLayer);
			}
			GauntletFullScreenNoticeView.Initialize();
			GauntletGameNotification.Initialize();
			GauntletChatLogView gauntletChatLogView = GauntletChatLogView.Current;
			if (gauntletChatLogView != null)
			{
				gauntletChatLogView.LoadMovie(false);
			}
			InformationManager.ClearAllMessages();
			base._state.OnGameContentUpdated += this.OnGameContentUpdated;
			this.SetGainNavigationAfterFrames(3);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000056D8 File Offset: 0x000038D8
		protected override void OnInitialScreenTick(float dt)
		{
			base.OnInitialScreenTick(dt);
			if (ScreenManager.IsMouseCursorHidden())
			{
				MouseManager.ShowCursor(false);
				MouseManager.ShowCursor(true);
			}
			InitialMenuVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.Tick();
			}
			if (this._gauntletLayer.Input.IsHotKeyReleased("Exit"))
			{
				BrightnessOptionVM brightnessOptionDataSource = this._brightnessOptionDataSource;
				if (brightnessOptionDataSource != null && brightnessOptionDataSource.Visible)
				{
					UISoundsHelper.PlayUISound("event:/ui/default");
					this._brightnessOptionDataSource.ExecuteCancel();
					return;
				}
				ExposureOptionVM exposureOptionDataSource = this._exposureOptionDataSource;
				if (exposureOptionDataSource != null && exposureOptionDataSource.Visible)
				{
					UISoundsHelper.PlayUISound("event:/ui/default");
					this._exposureOptionDataSource.ExecuteCancel();
					return;
				}
			}
			else if (this._gauntletLayer.Input.IsHotKeyReleased("Confirm"))
			{
				BrightnessOptionVM brightnessOptionDataSource2 = this._brightnessOptionDataSource;
				if (brightnessOptionDataSource2 != null && brightnessOptionDataSource2.Visible)
				{
					UISoundsHelper.PlayUISound("event:/ui/default");
					this._brightnessOptionDataSource.ExecuteConfirm();
					return;
				}
				ExposureOptionVM exposureOptionDataSource2 = this._exposureOptionDataSource;
				if (exposureOptionDataSource2 != null && exposureOptionDataSource2.Visible)
				{
					UISoundsHelper.PlayUISound("event:/ui/default");
					this._exposureOptionDataSource.ExecuteConfirm();
				}
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000057E8 File Offset: 0x000039E8
		protected override void OnActivate()
		{
			base.OnActivate();
			InitialMenuVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.RefreshMenuOptions();
			}
			this.SetGainNavigationAfterFrames(3);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00005808 File Offset: 0x00003A08
		private void SetGainNavigationAfterFrames(int frameCount)
		{
			this._gauntletLayer.UIContext.GamepadNavigation.GainNavigationAfterFrames(frameCount, delegate
			{
				BrightnessOptionVM brightnessOptionDataSource = this._brightnessOptionDataSource;
				if (brightnessOptionDataSource == null || !brightnessOptionDataSource.Visible)
				{
					ExposureOptionVM exposureOptionDataSource = this._exposureOptionDataSource;
					return exposureOptionDataSource == null || !exposureOptionDataSource.Visible;
				}
				return false;
			});
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000582C File Offset: 0x00003A2C
		private void OnGameContentUpdated()
		{
			InitialMenuVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.RefreshMenuOptions();
		}

		// Token: 0x060000AC RID: 172 RVA: 0x0000583E File Offset: 0x00003A3E
		private void OnCloseBrightness(bool isConfirm)
		{
			this._gauntletBrightnessLayer.ReleaseMovie(this._brightnessOptionMovie);
			base.RemoveLayer(this._gauntletBrightnessLayer);
			this._brightnessOptionDataSource = null;
			this._gauntletBrightnessLayer = null;
			NativeOptions.SaveConfig();
			this.OpenExposureControl();
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00005878 File Offset: 0x00003A78
		private void OpenExposureControl()
		{
			this._exposureOptionDataSource = new ExposureOptionVM(new Action<bool>(this.OnCloseExposureControl))
			{
				Visible = true
			};
			this._gauntletExposureLayer = new GauntletLayer("MainMenuExposure", 2, false);
			this._gauntletExposureLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.Mouse);
			this._exposureOptionMovie = this._gauntletExposureLayer.LoadMovie("ExposureOption", this._exposureOptionDataSource);
			base.AddLayer(this._gauntletExposureLayer);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000058EF File Offset: 0x00003AEF
		private void OnCloseExposureControl(bool isConfirm)
		{
			this._gauntletExposureLayer.ReleaseMovie(this._exposureOptionMovie);
			base.RemoveLayer(this._gauntletExposureLayer);
			this._exposureOptionDataSource = null;
			this._gauntletExposureLayer = null;
			NativeOptions.SaveConfig();
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00005924 File Offset: 0x00003B24
		protected override void OnFinalize()
		{
			base.OnFinalize();
			if (base._state != null)
			{
				base._state.OnGameContentUpdated -= this.OnGameContentUpdated;
			}
			if (this._gauntletLayer != null)
			{
				base.RemoveLayer(this._gauntletLayer);
			}
			InitialMenuVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnFinalize();
			}
			this._dataSource = null;
			this._gauntletLayer = null;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00005989 File Offset: 0x00003B89
		public void TryUpdateChatLogLayerParameters(ref bool isTeamChatAvailable, ref bool inputEnabled, ref bool isToggleChatHintAvailable, ref bool isMouseVisible, ref InputContext inputContext)
		{
			inputEnabled = false;
			inputContext = null;
		}

		// Token: 0x0400006A RID: 106
		private GauntletLayer _gauntletLayer;

		// Token: 0x0400006B RID: 107
		private GauntletLayer _gauntletBrightnessLayer;

		// Token: 0x0400006C RID: 108
		private GauntletLayer _gauntletExposureLayer;

		// Token: 0x0400006D RID: 109
		private InitialMenuVM _dataSource;

		// Token: 0x0400006E RID: 110
		private BrightnessOptionVM _brightnessOptionDataSource;

		// Token: 0x0400006F RID: 111
		private ExposureOptionVM _exposureOptionDataSource;

		// Token: 0x04000070 RID: 112
		private GauntletMovieIdentifier _brightnessOptionMovie;

		// Token: 0x04000071 RID: 113
		private GauntletMovieIdentifier _exposureOptionMovie;
	}
}
