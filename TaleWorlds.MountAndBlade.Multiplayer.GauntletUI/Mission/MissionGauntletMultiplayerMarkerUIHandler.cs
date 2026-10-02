using System;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.FlagMarker;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI.Mission
{
	// Token: 0x02000015 RID: 21
	[OverrideView(typeof(MissionMultiplayerMarkerUIHandler))]
	public class MissionGauntletMultiplayerMarkerUIHandler : MissionView
	{
		// Token: 0x060000EE RID: 238 RVA: 0x000062D0 File Offset: 0x000044D0
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._dataSource = new MultiplayerMissionMarkerVM(base.MissionScreen.CombatCamera);
			this._gauntletLayer = new GauntletLayer("MPMissionMarkers", 1, false);
			this._gauntletLayer.LoadMovie("MPMissionMarkers", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00006333 File Offset: 0x00004533
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00006365 File Offset: 0x00004565
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (base.Input.IsGameKeyDown(5))
			{
				this._dataSource.IsEnabled = true;
			}
			else
			{
				this._dataSource.IsEnabled = false;
			}
			this._dataSource.Tick(dt);
		}

		// Token: 0x04000068 RID: 104
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000069 RID: 105
		private MultiplayerMissionMarkerVM _dataSource;
	}
}
