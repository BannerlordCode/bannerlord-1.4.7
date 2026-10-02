using System;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.TeamSelection;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI.Mission
{
	// Token: 0x0200000C RID: 12
	[OverrideView(typeof(MultiplayerCultureSelectUIHandler))]
	public class MissionGauntletCultureSelection : MissionView
	{
		// Token: 0x060000A7 RID: 167 RVA: 0x00004EF8 File Offset: 0x000030F8
		public MissionGauntletCultureSelection()
		{
			this.ViewOrderPriority = 22;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00004F08 File Offset: 0x00003108
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._missionLobbyComponent = base.Mission.GetMissionBehavior<MissionLobbyComponent>();
			this._missionLobbyComponent.OnCultureSelectionRequested += this.OnCultureSelectionRequested;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004F38 File Offset: 0x00003138
		public override void OnMissionScreenFinalize()
		{
			this._missionLobbyComponent.OnCultureSelectionRequested -= this.OnCultureSelectionRequested;
			base.OnMissionScreenFinalize();
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00004F57 File Offset: 0x00003157
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (this._toOpen && base.MissionScreen.SetDisplayDialog(true))
			{
				this._toOpen = false;
				this.OnOpen();
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004F84 File Offset: 0x00003184
		private void OnOpen()
		{
			this._dataSource = new MultiplayerCultureSelectVM(new Action<BasicCultureObject>(this.OnCultureSelected), new Action(this.OnClose));
			this._gauntletLayer = new GauntletLayer("MultiplayerCultureSelection", this.ViewOrderPriority, false);
			this._gauntletLayer.LoadMovie("MultiplayerCultureSelection", this._dataSource);
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			base.MissionScreen.AddLayer(this._gauntletLayer);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00005008 File Offset: 0x00003208
		private void OnClose()
		{
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			base.MissionScreen.SetDisplayDialog(false);
			this._gauntletLayer.InputRestrictions.ResetInputRestrictions();
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x0000505C File Offset: 0x0000325C
		private void OnCultureSelectionRequested()
		{
			this._toOpen = true;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00005065 File Offset: 0x00003265
		private void OnCultureSelected(BasicCultureObject culture)
		{
			this._missionLobbyComponent.OnCultureSelected(culture);
			this.OnClose();
		}

		// Token: 0x0400003D RID: 61
		private GauntletLayer _gauntletLayer;

		// Token: 0x0400003E RID: 62
		private MultiplayerCultureSelectVM _dataSource;

		// Token: 0x0400003F RID: 63
		private MissionLobbyComponent _missionLobbyComponent;

		// Token: 0x04000040 RID: 64
		private bool _toOpen;
	}
}
