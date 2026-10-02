using System;
using System.Collections.Generic;
using SandBox.ViewModelCollection.Map.Cheat;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Missions
{
	// Token: 0x0200001C RID: 28
	[OverrideView(typeof(MissionCheatView))]
	public class MissionGauntletCheatView : MissionCheatView
	{
		// Token: 0x0600019E RID: 414 RVA: 0x0000B09A File Offset: 0x0000929A
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			this.FinalizeScreen();
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000B0A8 File Offset: 0x000092A8
		public override bool GetIsCheatsAvailable()
		{
			return true;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000B0AC File Offset: 0x000092AC
		public override void InitializeScreen()
		{
			if (this._isActive)
			{
				return;
			}
			this._isActive = true;
			IEnumerable<GameplayCheatBase> missionCheatList = GameplayCheatsManager.GetMissionCheatList();
			this._dataSource = new GameplayCheatsVM(new Action(this.FinalizeScreen), missionCheatList);
			this.InitializeKeyVisuals();
			this._gauntletLayer = new GauntletLayer("MissionCheat", 4500, false);
			this._gauntletLayer.LoadMovie("MapCheats", this._dataSource);
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			this._gauntletLayer.IsFocusLayer = true;
			ScreenManager.TrySetFocus(this._gauntletLayer);
			base.MissionScreen.AddLayer(this._gauntletLayer);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000B170 File Offset: 0x00009370
		public override void FinalizeScreen()
		{
			if (!this._isActive)
			{
				return;
			}
			this._isActive = false;
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			GameplayCheatsVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnFinalize();
			}
			this._gauntletLayer = null;
			this._dataSource = null;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000B1BD File Offset: 0x000093BD
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (this._isActive)
			{
				this.HandleInput();
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000B1D4 File Offset: 0x000093D4
		private void HandleInput()
		{
			if (this._gauntletLayer.Input.IsHotKeyReleased("Exit"))
			{
				UISoundsHelper.PlayUISound("event:/ui/default");
				GameplayCheatsVM dataSource = this._dataSource;
				if (dataSource == null)
				{
					return;
				}
				dataSource.ExecuteClose();
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000B207 File Offset: 0x00009407
		private void InitializeKeyVisuals()
		{
			this._dataSource.SetCloseInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
		}

		// Token: 0x04000081 RID: 129
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000082 RID: 130
		private GameplayCheatsVM _dataSource;

		// Token: 0x04000083 RID: 131
		private bool _isActive;
	}
}
