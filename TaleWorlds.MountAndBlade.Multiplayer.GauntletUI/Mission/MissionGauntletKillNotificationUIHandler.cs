using System;
using System.Collections.Generic;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.KillFeed;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI.Mission
{
	// Token: 0x02000011 RID: 17
	[OverrideView(typeof(MissionMultiplayerKillNotificationUIHandler))]
	public class MissionGauntletKillNotificationUIHandler : MissionView
	{
		// Token: 0x060000D0 RID: 208 RVA: 0x000058F0 File Offset: 0x00003AF0
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this.ViewOrderPriority = 2;
			this._isGeneralFeedEnabled = this._doesGameModeAllowGeneralFeed && BannerlordConfig.KillFeedVisualType < 2;
			this._isPersonalFeedEnabled = BannerlordConfig.ReportPersonalDamage;
			this._dataSource = new MPKillFeedVM();
			this._gauntletLayer = new GauntletLayer("MultiplayerKillFeed", this.ViewOrderPriority, false);
			this._gauntletLayer.LoadMovie("MultiplayerKillFeed", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
			CombatLogManager.OnGenerateCombatLog += this.OnCombatLogManagerOnPrintCombatLog;
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Combine(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnOptionChange));
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000059A9 File Offset: 0x00003BA9
		private void OnOptionChange(ManagedOptions.ManagedOptionsType changedManagedOptionsType)
		{
			if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.ReportCasualtiesType)
			{
				this._isGeneralFeedEnabled = this._doesGameModeAllowGeneralFeed && BannerlordConfig.KillFeedVisualType < 2;
				return;
			}
			if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.ReportPersonalDamage)
			{
				this._isPersonalFeedEnabled = BannerlordConfig.ReportPersonalDamage;
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000059DC File Offset: 0x00003BDC
		public override void AfterStart()
		{
			base.AfterStart();
			this._tdmClient = base.Mission.GetMissionBehavior<MissionMultiplayerTeamDeathmatchClient>();
			if (this._tdmClient != null)
			{
				this._tdmClient.OnGoldGainEvent += this.OnGoldGain;
			}
			this._siegeClient = base.Mission.GetMissionBehavior<MissionMultiplayerSiegeClient>();
			if (this._siegeClient != null)
			{
				this._siegeClient.OnGoldGainEvent += this.OnGoldGain;
			}
			this._flagDominationClient = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeFlagDominationClient>();
			if (this._flagDominationClient != null)
			{
				this._flagDominationClient.OnGoldGainEvent += this.OnGoldGain;
			}
			this._duelClient = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeDuelClient>();
			if (this._duelClient != null)
			{
				this._doesGameModeAllowGeneralFeed = false;
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00005AA0 File Offset: 0x00003CA0
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			CombatLogManager.OnGenerateCombatLog -= this.OnCombatLogManagerOnPrintCombatLog;
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Remove(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnOptionChange));
			if (this._tdmClient != null)
			{
				this._tdmClient.OnGoldGainEvent -= this.OnGoldGain;
			}
			if (this._siegeClient != null)
			{
				this._siegeClient.OnGoldGainEvent -= this.OnGoldGain;
			}
			if (this._flagDominationClient != null)
			{
				this._flagDominationClient.OnGoldGainEvent -= this.OnGoldGain;
			}
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00005B6C File Offset: 0x00003D6C
		private void OnGoldGain(GoldGain goldGainMessage)
		{
			if (this._isPersonalFeedEnabled)
			{
				foreach (KeyValuePair<ushort, int> keyValuePair in goldGainMessage.GoldChangeEventList)
				{
					this._dataSource.PersonalCasualty.OnGoldChange(keyValuePair.Value, (GoldGainFlags)keyValuePair.Key);
				}
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00005BE0 File Offset: 0x00003DE0
		private void OnCombatLogManagerOnPrintCombatLog(CombatLogData logData)
		{
			if (this._isPersonalFeedEnabled && (logData.IsAttackerAgentMine || logData.IsAttackerAgentRiderAgentMine) && logData.TotalDamage > 0 && !logData.IsVictimAgentSameAsAttackerAgent)
			{
				this._dataSource.OnPersonalDamage(logData.TotalDamage, logData.IsFatalDamage, logData.IsVictimAgentMount, logData.IsFriendlyFire, logData.BodyPartHit == BoneBodyPartType.Head, logData.VictimAgentName);
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00005C4C File Offset: 0x00003E4C
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, killingBlow);
			if (!this._isGeneralFeedEnabled || GameNetwork.IsDedicatedServer || affectorAgent == null || !affectedAgent.IsHuman || (agentState != AgentState.Killed && agentState != AgentState.Unconscious))
			{
				return;
			}
			this._dataSource.OnAgentRemoved(affectedAgent, affectorAgent, this._isPersonalFeedEnabled);
		}

		// Token: 0x04000053 RID: 83
		private MPKillFeedVM _dataSource;

		// Token: 0x04000054 RID: 84
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000055 RID: 85
		private MissionMultiplayerTeamDeathmatchClient _tdmClient;

		// Token: 0x04000056 RID: 86
		private MissionMultiplayerSiegeClient _siegeClient;

		// Token: 0x04000057 RID: 87
		private MissionMultiplayerGameModeDuelClient _duelClient;

		// Token: 0x04000058 RID: 88
		private MissionMultiplayerGameModeFlagDominationClient _flagDominationClient;

		// Token: 0x04000059 RID: 89
		private bool _isGeneralFeedEnabled;

		// Token: 0x0400005A RID: 90
		private bool _doesGameModeAllowGeneralFeed = true;

		// Token: 0x0400005B RID: 91
		private bool _isPersonalFeedEnabled;
	}
}
