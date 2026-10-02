using System;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.HUDExtensions;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI.Mission
{
	// Token: 0x02000014 RID: 20
	[OverrideView(typeof(MissionMultiplayerHUDExtensionUIHandler))]
	public class MissionGauntletMultiplayerHUDExtension : MissionView
	{
		// Token: 0x060000E7 RID: 231 RVA: 0x000060D7 File Offset: 0x000042D7
		public MissionGauntletMultiplayerHUDExtension()
		{
			this.ViewOrderPriority = 2;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000060E8 File Offset: 0x000042E8
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._mpMissionCategory = UIResourceManager.LoadSpriteCategory("ui_mpmission");
			this._dataSource = new MissionMultiplayerHUDExtensionVM(base.Mission);
			this._gauntletLayer = new GauntletLayer("HUDExtension", this.ViewOrderPriority, false);
			this._gauntletLayer.LoadMovie("HUDExtension", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
			base.MissionScreen.OnSpectateAgentFocusIn += this._dataSource.OnSpectatedAgentFocusIn;
			base.MissionScreen.OnSpectateAgentFocusOut += this._dataSource.OnSpectatedAgentFocusOut;
			Game.Current.EventManager.RegisterEvent<MissionPlayerToggledOrderViewEvent>(new Action<MissionPlayerToggledOrderViewEvent>(this.OnMissionPlayerToggledOrderViewEvent));
			this._lobbyComponent = base.Mission.GetMissionBehavior<MissionLobbyComponent>();
			this._lobbyComponent.OnPostMatchEnded += this.OnPostMatchEnded;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000061D8 File Offset: 0x000043D8
		public override void OnMissionScreenFinalize()
		{
			this._lobbyComponent.OnPostMatchEnded -= this.OnPostMatchEnded;
			base.MissionScreen.OnSpectateAgentFocusIn -= this._dataSource.OnSpectatedAgentFocusIn;
			base.MissionScreen.OnSpectateAgentFocusOut -= this._dataSource.OnSpectatedAgentFocusOut;
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			SpriteCategory mpMissionCategory = this._mpMissionCategory;
			if (mpMissionCategory != null)
			{
				mpMissionCategory.Unload();
			}
			this._dataSource.OnFinalize();
			this._dataSource = null;
			this._gauntletLayer = null;
			Game.Current.EventManager.UnregisterEvent<MissionPlayerToggledOrderViewEvent>(new Action<MissionPlayerToggledOrderViewEvent>(this.OnMissionPlayerToggledOrderViewEvent));
			base.OnMissionScreenFinalize();
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00006290 File Offset: 0x00004490
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			this._dataSource.Tick(dt);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000062A5 File Offset: 0x000044A5
		private void OnMissionPlayerToggledOrderViewEvent(MissionPlayerToggledOrderViewEvent eventObj)
		{
			this._dataSource.IsOrderActive = eventObj.IsOrderEnabled;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000062B8 File Offset: 0x000044B8
		private void OnPostMatchEnded()
		{
			this._dataSource.ShowHud = false;
		}

		// Token: 0x04000064 RID: 100
		private MissionMultiplayerHUDExtensionVM _dataSource;

		// Token: 0x04000065 RID: 101
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000066 RID: 102
		private SpriteCategory _mpMissionCategory;

		// Token: 0x04000067 RID: 103
		private MissionLobbyComponent _lobbyComponent;
	}
}
