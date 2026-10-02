using System;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD.KillFeed;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission.Singleplayer
{
	// Token: 0x02000039 RID: 57
	[OverrideView(typeof(MissionSingleplayerKillNotificationUIHandler))]
	public class MissionGauntletKillNotificationSingleplayerUIHandler : MissionBattleUIBaseView
	{
		// Token: 0x06000293 RID: 659 RVA: 0x0000F290 File Offset: 0x0000D490
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this.ViewOrderPriority = 17;
			this._isGeneralFeedEnabled = BannerlordConfig.KillFeedVisualType < 2;
			this._isPersonalFeedEnabled = BannerlordConfig.ReportPersonalDamage;
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Combine(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnOptionChange));
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000F2E4 File Offset: 0x0000D4E4
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Remove(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnOptionChange));
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000F30C File Offset: 0x0000D50C
		protected override void OnCreateView()
		{
			this._dataSource = new SPKillFeedVM();
			this._gauntletLayer = new GauntletLayer("MissionSPKillFeed", this.ViewOrderPriority, false);
			this._gauntletLayer.LoadMovie("SingleplayerKillfeed", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
			CombatLogManager.OnGenerateCombatLog += this.OnCombatLogManagerOnPrintCombatLog;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000F374 File Offset: 0x0000D574
		protected override void OnDestroyView()
		{
			CombatLogManager.OnGenerateCombatLog -= this.OnCombatLogManagerOnPrintCombatLog;
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000F3B1 File Offset: 0x0000D5B1
		protected override void OnSuspendView()
		{
			if (this._gauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._gauntletLayer, true);
			}
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000F3C7 File Offset: 0x0000D5C7
		protected override void OnResumeView()
		{
			if (this._gauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._gauntletLayer, false);
			}
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000F3E0 File Offset: 0x0000D5E0
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (this._dataSource != null)
			{
				bool isPaused = MBCommon.IsPaused;
				for (int i = 0; i < this._dataSource.GeneralCasualty.NotificationList.Count; i++)
				{
					this._dataSource.GeneralCasualty.NotificationList[i].IsPaused = isPaused;
				}
				for (int j = 0; j < this._dataSource.PersonalFeed.NotificationList.Count; j++)
				{
					this._dataSource.PersonalFeed.NotificationList[j].IsPaused = isPaused;
				}
			}
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000F47A File Offset: 0x0000D67A
		private void OnOptionChange(ManagedOptions.ManagedOptionsType changedManagedOptionsType)
		{
			if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.ReportCasualtiesType)
			{
				this._isGeneralFeedEnabled = BannerlordConfig.KillFeedVisualType < 2;
				return;
			}
			if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.ReportPersonalDamage)
			{
				this._isPersonalFeedEnabled = BannerlordConfig.ReportPersonalDamage;
			}
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000F4A0 File Offset: 0x0000D6A0
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, killingBlow);
			if (!base.IsViewCreated || affectorAgent == null || (agentState != AgentState.Killed && agentState != AgentState.Unconscious))
			{
				return;
			}
			bool flag = killingBlow.IsHeadShot();
			if (this._isPersonalFeedEnabled && affectorAgent == Agent.Main && (affectedAgent.IsHuman || affectedAgent.IsMount))
			{
				bool flag2 = affectedAgent.Team == affectorAgent.Team || affectedAgent.IsFriendOf(affectorAgent);
				SPKillFeedVM dataSource = this._dataSource;
				int inflictedDamage = killingBlow.InflictedDamage;
				bool isMount = affectedAgent.IsMount;
				bool flag3 = flag2;
				bool flag4 = flag;
				BasicCharacterObject character = affectedAgent.Character;
				dataSource.OnPersonalKill(inflictedDamage, isMount, flag3, flag4, (character != null) ? character.Name.ToString() : null, agentState == AgentState.Unconscious);
			}
			if (this._isGeneralFeedEnabled && affectedAgent.IsHuman)
			{
				this._dataSource.OnAgentRemoved(affectedAgent, affectorAgent, flag, affectedAgent == affectorAgent, affectedAgent == affectorAgent && affectedAgent.IsInWater());
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000F574 File Offset: 0x0000D774
		private void OnCombatLogManagerOnPrintCombatLog(CombatLogData logData)
		{
			if (this._isPersonalFeedEnabled && (logData.IsAttackerAgentMine || logData.IsAttackerAgentRiderAgentMine) && logData.TotalDamage > 0 && (!logData.IsFatalDamage || (logData.IsEntityToEntityCollisionDamage && logData.IsSpecialDamage)))
			{
				this._dataSource.OnPersonalDamage(logData.TotalDamage, logData.IsVictimAgentMount, logData.IsFriendlyFire || logData.IsVictimAgentMine, logData.VictimAgentName);
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000F5EA File Offset: 0x0000D7EA
		public override void OnPhotoModeActivated()
		{
			base.OnPhotoModeActivated();
			if (base.IsViewCreated)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 0f;
			}
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000F60F File Offset: 0x0000D80F
		public override void OnPhotoModeDeactivated()
		{
			base.OnPhotoModeDeactivated();
			if (base.IsViewCreated)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 1f;
			}
		}

		// Token: 0x0400014F RID: 335
		protected SPKillFeedVM _dataSource;

		// Token: 0x04000150 RID: 336
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000151 RID: 337
		protected bool _isGeneralFeedEnabled = true;

		// Token: 0x04000152 RID: 338
		protected bool _isPersonalFeedEnabled = true;
	}
}
