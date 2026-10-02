using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.MountAndBlade.Missions.Handlers;
using TaleWorlds.MountAndBlade.Source.Missions;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000276 RID: 630
	public class BattleEndLogic : MissionLogic, IBattleEndLogic
	{
		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06002341 RID: 9025 RVA: 0x0007D052 File Offset: 0x0007B252
		public bool PlayerVictory
		{
			get
			{
				return (this._isEnemySideRetreating || this._isEnemySideDepleted) && !this._isEnemyDefenderPulledBack;
			}
		}

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06002342 RID: 9026 RVA: 0x0007D06F File Offset: 0x0007B26F
		public bool EnemyVictory
		{
			get
			{
				return this._isPlayerSideRetreating || this._isPlayerSideDepleted;
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06002343 RID: 9027 RVA: 0x0007D081 File Offset: 0x0007B281
		public bool IsEnemySideRetreating
		{
			get
			{
				return this._isEnemySideRetreating;
			}
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06002344 RID: 9028 RVA: 0x0007D089 File Offset: 0x0007B289
		// (set) Token: 0x06002345 RID: 9029 RVA: 0x0007D091 File Offset: 0x0007B291
		private bool _notificationsDisabled { get; set; }

		// Token: 0x06002346 RID: 9030 RVA: 0x0007D09A File Offset: 0x0007B29A
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._checkRetreatingTimer = new BasicMissionTimer();
			this._missionAgentSpawnLogic = base.Mission.GetMissionBehavior<IMissionAgentSpawnLogic>();
		}

		// Token: 0x06002347 RID: 9031 RVA: 0x0007D0C0 File Offset: 0x0007B2C0
		public override void OnMissionTick(float dt)
		{
			if (base.Mission.IsMissionEnding)
			{
				if (this._notificationsDisabled)
				{
					this._scoreBoardOpenedOnceOnMissionEnd = true;
				}
				if (this._missionEndedMessageShown && !this._scoreBoardOpenedOnceOnMissionEnd)
				{
					if (this._checkRetreatingTimer.ElapsedTime > 7f)
					{
						this.CheckIsEnemySideRetreatingOrOneSideDepleted();
						this._checkRetreatingTimer.Reset();
						if (base.Mission.MissionResult != null && base.Mission.MissionResult.PlayerDefeated)
						{
							GameTexts.SetVariable("leave_key", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("Generic", 4), 1f));
							MBInformationManager.AddQuickInformation(GameTexts.FindText("str_battle_lost_press_tab_to_view_results", null), 0, null, null, "");
						}
						else if (base.Mission.MissionResult != null && base.Mission.MissionResult.PlayerVictory)
						{
							if (this._isEnemySideDepleted)
							{
								GameTexts.SetVariable("leave_key", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("Generic", 4), 1f));
								MBInformationManager.AddQuickInformation(GameTexts.FindText("str_battle_won_press_tab_to_view_results", null), 0, null, null, "");
							}
						}
						else
						{
							GameTexts.SetVariable("leave_key", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("Generic", 4), 1f));
							MBInformationManager.AddQuickInformation(GameTexts.FindText("str_battle_finished_press_tab_to_view_results", null), 0, null, null, "");
						}
					}
				}
				else if (this._checkRetreatingTimer.ElapsedTime > 3f && !this._scoreBoardOpenedOnceOnMissionEnd)
				{
					if (base.Mission.MissionResult != null && base.Mission.MissionResult.PlayerDefeated)
					{
						if (this._isPlayerSideDepleted)
						{
							MBInformationManager.AddQuickInformation(GameTexts.FindText("str_battle_lost", null), 0, null, null, "");
						}
						else if (this._isPlayerSideRetreating)
						{
							MBInformationManager.AddQuickInformation(GameTexts.FindText("str_friendlies_are_fleeing_you_lost", null), 0, null, null, "");
						}
					}
					else if (base.Mission.MissionResult != null && base.Mission.MissionResult.PlayerVictory)
					{
						if (this._isEnemySideDepleted)
						{
							MBInformationManager.AddQuickInformation(GameTexts.FindText("str_battle_won", null), 0, null, null, "");
						}
						else if (this._isEnemySideRetreating)
						{
							MBInformationManager.AddQuickInformation(GameTexts.FindText("str_enemies_are_fleeing_you_won", null), 0, null, null, "");
						}
					}
					else
					{
						MBInformationManager.AddQuickInformation(GameTexts.FindText("str_battle_finished", null), 0, null, null, "");
					}
					this._missionEndedMessageShown = true;
					this._checkRetreatingTimer.Reset();
				}
				if (!this._victoryReactionsActivated)
				{
					AgentVictoryLogic missionBehavior = base.Mission.GetMissionBehavior<AgentVictoryLogic>();
					if (missionBehavior != null)
					{
						this.CheckIsEnemySideRetreatingOrOneSideDepleted();
						if (this._isEnemySideDepleted)
						{
							missionBehavior.SetTimersOfVictoryReactionsOnBattleEnd(base.Mission.PlayerTeam.Side);
							this._victoryReactionsActivated = true;
							return;
						}
						if (this._isPlayerSideDepleted)
						{
							missionBehavior.SetTimersOfVictoryReactionsOnBattleEnd(base.Mission.PlayerEnemyTeam.Side);
							this._victoryReactionsActivated = true;
							return;
						}
						if (this._isEnemySideRetreating && !this._victoryReactionsActivatedForRetreating)
						{
							missionBehavior.SetTimersOfVictoryReactionsOnRetreat(base.Mission.PlayerTeam.Side);
							this._victoryReactionsActivatedForRetreating = true;
							return;
						}
						if (this._isPlayerSideRetreating && !this._victoryReactionsActivatedForRetreating)
						{
							missionBehavior.SetTimersOfVictoryReactionsOnRetreat(base.Mission.PlayerEnemyTeam.Side);
							this._victoryReactionsActivatedForRetreating = true;
							return;
						}
					}
				}
			}
			else if (this._checkRetreatingTimer.ElapsedTime > 1f)
			{
				this.CheckIsEnemySideRetreatingOrOneSideDepleted();
				this._checkRetreatingTimer.Reset();
			}
		}

		// Token: 0x06002348 RID: 9032 RVA: 0x0007D42C File Offset: 0x0007B62C
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			if (this._enemyDefenderPullbackEnabled && this._troopNumberNeededForEnemyDefenderPullBack > 0 && affectedAgent.IsHuman && agentState == AgentState.Routed && affectedAgent.Team != null && affectedAgent.Team.Side == BattleSideEnum.Defender && affectedAgent.Team.Side != base.Mission.PlayerTeam.Side)
			{
				this._troopNumberNeededForEnemyDefenderPullBack--;
				this._isEnemyDefenderPulledBack = this._troopNumberNeededForEnemyDefenderPullBack <= 0;
			}
		}

		// Token: 0x06002349 RID: 9033 RVA: 0x0007D4A8 File Offset: 0x0007B6A8
		public override bool MissionEnded(ref MissionResult missionResult)
		{
			bool flag = false;
			if (this._isEnemySideDepleted && this._isEnemyDefenderPulledBack)
			{
				missionResult = MissionResult.CreateDefenderPushedBack();
				flag = true;
			}
			else if (this._isEnemySideRetreating || this._isEnemySideDepleted)
			{
				missionResult = MissionResult.CreateSuccessful(base.Mission, this._isEnemySideRetreating);
				flag = true;
			}
			else if (this._isPlayerSideRetreating || this._isPlayerSideDepleted)
			{
				missionResult = MissionResult.CreateDefeated(base.Mission);
				flag = true;
			}
			if (flag)
			{
				this._missionAgentSpawnLogic.StopSpawner(BattleSideEnum.Attacker);
				this._missionAgentSpawnLogic.StopSpawner(BattleSideEnum.Defender);
			}
			return flag;
		}

		// Token: 0x0600234A RID: 9034 RVA: 0x0007D534 File Offset: 0x0007B734
		protected override void OnEndMission()
		{
			if (this._isEnemySideRetreating)
			{
				foreach (Agent agent in base.Mission.PlayerEnemyTeam.ActiveAgents)
				{
					bool flag = agent.GetMorale() < 0.01f;
					IAgentOriginBase origin = agent.Origin;
					if (origin != null)
					{
						origin.SetRouted(!flag);
					}
				}
			}
		}

		// Token: 0x0600234B RID: 9035 RVA: 0x0007D5B4 File Offset: 0x0007B7B4
		public void ChangeCanCheckForEndCondition(bool canCheckForEndCondition)
		{
			this._canCheckForEndCondition = canCheckForEndCondition;
		}

		// Token: 0x0600234C RID: 9036 RVA: 0x0007D5C0 File Offset: 0x0007B7C0
		public BattleEndLogic.ExitResult TryExit()
		{
			if (GameNetwork.IsClientOrReplay)
			{
				return BattleEndLogic.ExitResult.False;
			}
			if (base.Mission.MissionEnded || (!this.PlayerVictory && !this.EnemyVictory))
			{
				Agent mainAgent = base.Mission.MainAgent;
				if (mainAgent == null || !mainAgent.IsActive() || !base.Mission.IsPlayerCloseToAnEnemy(5f))
				{
					if (base.Mission.MissionEnded || this._isEnemySideRetreating)
					{
						base.Mission.EndMission();
						return BattleEndLogic.ExitResult.True;
					}
					if (Mission.Current.IsSiegeBattle && base.Mission.PlayerTeam.IsDefender)
					{
						return BattleEndLogic.ExitResult.SurrenderSiege;
					}
					return BattleEndLogic.ExitResult.NeedsPlayerConfirmation;
				}
			}
			return BattleEndLogic.ExitResult.False;
		}

		// Token: 0x0600234D RID: 9037 RVA: 0x0007D663 File Offset: 0x0007B863
		public void EnableEnemyDefenderPullBack(int neededTroopNumber)
		{
			this._enemyDefenderPullbackEnabled = true;
			this._troopNumberNeededForEnemyDefenderPullBack = neededTroopNumber;
		}

		// Token: 0x0600234E RID: 9038 RVA: 0x0007D673 File Offset: 0x0007B873
		public void SetNotificationDisabled(bool value)
		{
			this._notificationsDisabled = value;
		}

		// Token: 0x0600234F RID: 9039 RVA: 0x0007D67C File Offset: 0x0007B87C
		private void CheckIsEnemySideRetreatingOrOneSideDepleted()
		{
			if (!this._canCheckForEndConditionSiege)
			{
				this._canCheckForEndConditionSiege = base.Mission.GetMissionBehavior<BattleDeploymentHandler>() == null;
				return;
			}
			if (this._canCheckForEndCondition)
			{
				BattleSideEnum side = base.Mission.PlayerTeam.Side;
				BattleSideEnum oppositeSide = side.GetOppositeSide();
				this._isPlayerSideDepleted = this._missionAgentSpawnLogic.IsSideDepleted(side);
				this._isEnemySideDepleted = this._missionAgentSpawnLogic.IsSideDepleted(oppositeSide);
				if (!this._isEnemySideDepleted && !this._isPlayerSideDepleted && base.Mission.GetMissionBehavior<HideoutPhasedMissionController>() == null)
				{
					float num = this._missionAgentSpawnLogic.GetReinforcementInterval(side) + 3f;
					if (base.Mission.MainAgent != null && base.Mission.MainAgent.IsPlayerControlled && base.Mission.MainAgent.IsActive())
					{
						this._playerSideNotYetRetreatingTime = MissionTime.Now;
					}
					else
					{
						bool flag = true;
						foreach (Team team in base.Mission.Teams)
						{
							if (team.IsFriendOf(base.Mission.PlayerTeam))
							{
								using (List<Agent>.Enumerator enumerator2 = team.ActiveAgents.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										if (!enumerator2.Current.IsRunningAway)
										{
											flag = false;
											break;
										}
									}
								}
							}
						}
						if (!flag)
						{
							this._playerSideNotYetRetreatingTime = MissionTime.Now;
						}
					}
					if (this._playerSideNotYetRetreatingTime.ElapsedSeconds > num)
					{
						this._isPlayerSideRetreating = true;
					}
					if (oppositeSide != BattleSideEnum.Defender || !this._enemyDefenderPullbackEnabled)
					{
						float num2 = this._missionAgentSpawnLogic.GetReinforcementInterval(oppositeSide) + 3f;
						bool flag2 = true;
						foreach (Team team2 in base.Mission.Teams)
						{
							if (team2.IsEnemyOf(base.Mission.PlayerTeam))
							{
								using (List<Agent>.Enumerator enumerator2 = team2.ActiveAgents.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										if (!enumerator2.Current.IsRunningAway)
										{
											flag2 = false;
											break;
										}
									}
								}
							}
						}
						if (!flag2)
						{
							this._enemySideNotYetRetreatingTime = MissionTime.Now;
						}
						if (this._enemySideNotYetRetreatingTime.ElapsedSeconds > num2)
						{
							this._isEnemySideRetreating = true;
						}
					}
				}
			}
		}

		// Token: 0x04000D84 RID: 3460
		private IMissionAgentSpawnLogic _missionAgentSpawnLogic;

		// Token: 0x04000D85 RID: 3461
		private MissionTime _enemySideNotYetRetreatingTime;

		// Token: 0x04000D86 RID: 3462
		private MissionTime _playerSideNotYetRetreatingTime;

		// Token: 0x04000D87 RID: 3463
		private BasicMissionTimer _checkRetreatingTimer;

		// Token: 0x04000D88 RID: 3464
		private bool _isEnemySideRetreating;

		// Token: 0x04000D89 RID: 3465
		private bool _isPlayerSideRetreating;

		// Token: 0x04000D8A RID: 3466
		private bool _isEnemySideDepleted;

		// Token: 0x04000D8B RID: 3467
		private bool _isPlayerSideDepleted;

		// Token: 0x04000D8C RID: 3468
		private bool _isEnemyDefenderPulledBack;

		// Token: 0x04000D8D RID: 3469
		private bool _canCheckForEndCondition = true;

		// Token: 0x04000D8E RID: 3470
		private bool _canCheckForEndConditionSiege;

		// Token: 0x04000D8F RID: 3471
		private bool _enemyDefenderPullbackEnabled;

		// Token: 0x04000D90 RID: 3472
		private int _troopNumberNeededForEnemyDefenderPullBack;

		// Token: 0x04000D91 RID: 3473
		private bool _missionEndedMessageShown;

		// Token: 0x04000D92 RID: 3474
		private bool _victoryReactionsActivated;

		// Token: 0x04000D93 RID: 3475
		private bool _victoryReactionsActivatedForRetreating;

		// Token: 0x04000D94 RID: 3476
		private bool _scoreBoardOpenedOnceOnMissionEnd;

		// Token: 0x0200054E RID: 1358
		public enum ExitResult
		{
			// Token: 0x04001DD5 RID: 7637
			False,
			// Token: 0x04001DD6 RID: 7638
			NeedsPlayerConfirmation,
			// Token: 0x04001DD7 RID: 7639
			SurrenderSiege,
			// Token: 0x04001DD8 RID: 7640
			True
		}
	}
}
