using System;
using System.Collections.Generic;
using TaleWorlds.AchievementSystem;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000018 RID: 24
	public class MultiplayerAchievementComponent : MissionLogic
	{
		// Token: 0x06000174 RID: 372 RVA: 0x00006780 File Offset: 0x00004980
		public override void OnBehaviorInitialize()
		{
			this._missionLobbyComponent = Mission.Current.GetMissionBehavior<MissionLobbyComponent>();
			this._multiplayerRoundComponent = Mission.Current.GetMissionBehavior<MultiplayerRoundComponent>();
			this.CacheAndInitializeAchievementVariables();
		}

		// Token: 0x06000175 RID: 373 RVA: 0x000067A8 File Offset: 0x000049A8
		public override void EarlyStart()
		{
			if (this._multiplayerRoundComponent != null)
			{
				this._multiplayerRoundComponent.OnRoundStarted += this.OnRoundStarted;
			}
			if (this._recentBoulderKills == null)
			{
				this._recentBoulderKills = new Queue<MultiplayerAchievementComponent.BoulderKillRecord>();
				return;
			}
			this._recentBoulderKills.Clear();
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000067E8 File Offset: 0x000049E8
		protected override void OnEndMission()
		{
			if (this._multiplayerRoundComponent != null)
			{
				this._multiplayerRoundComponent.OnRoundStarted -= this.OnRoundStarted;
			}
			Queue<MultiplayerAchievementComponent.BoulderKillRecord> recentBoulderKills = this._recentBoulderKills;
			if (recentBoulderKills == null)
			{
				return;
			}
			recentBoulderKills.Clear();
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0000681C File Offset: 0x00004A1C
		public override void OnMissionTick(float dt)
		{
			if (this._recentBoulderKills != null)
			{
				while (this._recentBoulderKills.Count > 0)
				{
					MultiplayerAchievementComponent.BoulderKillRecord boulderKillRecord = this._recentBoulderKills.Peek();
					if (base.Mission.CurrentTime - boulderKillRecord.Time < 4f)
					{
						break;
					}
					this._recentBoulderKills.Dequeue();
				}
			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00006872 File Offset: 0x00004A72
		private void OnRoundStarted()
		{
			this._singleRoundKillsWithMeleeOnFoot = 0;
			this._singleRoundKillsWithMeleeMounted = 0;
			this._singleRoundKillsWithRangedOnFoot = 0;
			this._singleRoundKillsWithRangedMounted = 0;
			this._singleRoundKillsWithCouchedLance = 0;
			this._killsWithAStolenHorse = 0;
			this._hasStolenMount = false;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x000068A8 File Offset: 0x00004AA8
		public override void OnAgentMount(Agent agent)
		{
			if (agent.IsMine && agent.SpawnEquipment.Horse.IsEmpty)
			{
				this._hasStolenMount = true;
				this._killsWithAStolenHorse = 0;
			}
		}

		// Token: 0x0600017A RID: 378 RVA: 0x000068E0 File Offset: 0x00004AE0
		public override void OnAgentDismount(Agent agent)
		{
			if (agent.IsMine)
			{
				this._hasStolenMount = false;
				this._killsWithAStolenHorse = 0;
			}
		}

		// Token: 0x0600017B RID: 379 RVA: 0x000068F8 File Offset: 0x00004AF8
		public override void OnAgentBuild(Agent agent, Banner banner)
		{
			if (agent.IsMine)
			{
				this._hasStolenMount = false;
				this._killsWithAStolenHorse = 0;
			}
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00006910 File Offset: 0x00004B10
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (affectedAgent != null && !affectedAgent.IsMount)
			{
				if (agentState == AgentState.Killed)
				{
					if (affectorAgent != null && affectorAgent.IsMine && affectorAgent != affectedAgent && !affectedAgent.IsFriendOf(affectorAgent))
					{
						WeaponClass weaponClass = (WeaponClass)blow.WeaponClass;
						int num = (int)weaponClass;
						bool flag = num >= 1 && num <= 11;
						bool isMissile = blow.IsMissile;
						if (weaponClass == WeaponClass.Boulder || weaponClass == WeaponClass.BallistaBoulder)
						{
							this._recentBoulderKills.Enqueue(new MultiplayerAchievementComponent.BoulderKillRecord(base.Mission.CurrentTime));
							if (this._recentBoulderKills.Count > 1 && this._recentBoulderKills.Count > this._cachedMaxMultiKillsWithSingleMangonelShot)
							{
								this._cachedMaxMultiKillsWithSingleMangonelShot = this._recentBoulderKills.Count;
								this.SetStatInternal("MaxMultiKillsWithSingleMangonelShot", this._cachedMaxMultiKillsWithSingleMangonelShot);
							}
							this._cachedKillsWithBoulder++;
							this.SetStatInternal("KillsWithBoulder", this._cachedKillsWithBoulder);
						}
						if (blow.AttackType == AgentAttackType.Kick && blow.OverrideKillInfo == Agent.KillInfo.Gravity)
						{
							this.SetStatInternal("PushedSomeoneOffLedge", 1);
						}
						if (isMissile && blow.IsHeadShot())
						{
							this._cachedKillsWithRangedHeadShots++;
							this.SetStatInternal("KillsWithRangedHeadshots", this._cachedKillsWithRangedHeadShots);
						}
						if (affectorAgent.IsReleasingChainAttackInMultiplayer())
						{
							this._cachedKillsWithChainAttack++;
							this.SetStatInternal("KillsWithChainAttack", this._cachedKillsWithChainAttack);
						}
						if (affectorAgent.HasMount)
						{
							if (affectorAgent.IsDoingPassiveAttack)
							{
								this._singleRoundKillsWithCouchedLance++;
								this._cachedKillsWithCouchedLance++;
								this.SetStatInternal("KillsWithCouchedLance", this._cachedKillsWithCouchedLance);
							}
							if (isMissile)
							{
								this._singleRoundKillsWithRangedMounted++;
								this._cachedKillsWithRangedMounted++;
								this.SetStatInternal("KillsWithRangedMounted", this._cachedKillsWithRangedMounted);
							}
							if (flag)
							{
								this._singleRoundKillsWithMeleeMounted++;
							}
							if (!flag && !isMissile)
							{
								this._cachedKillsWithHorseCharge++;
								this.SetStatInternal("KillsWithHorseCharge", this._cachedKillsWithHorseCharge);
							}
							if (this._hasStolenMount)
							{
								this._killsWithAStolenHorse++;
								if (this._killsWithAStolenHorse > this._cachedKillsWithStolenHorse)
								{
									this._cachedKillsWithStolenHorse = this._killsWithAStolenHorse;
									this.SetStatInternal("KillsWithStolenHorse", this._cachedKillsWithStolenHorse);
								}
							}
						}
						else
						{
							if (isMissile)
							{
								this._singleRoundKillsWithRangedOnFoot++;
							}
							if (flag)
							{
								this._singleRoundKillsWithMeleeOnFoot++;
							}
						}
						if (this._missionLobbyComponent.MissionType == MultiplayerGameType.Skirmish && this._singleRoundKillsWithMeleeOnFoot > 0 && this._singleRoundKillsWithMeleeMounted > 0 && this._singleRoundKillsWithRangedOnFoot > 0 && this._singleRoundKillsWithRangedMounted > 0 && this._singleRoundKillsWithCouchedLance > 0)
						{
							this.SetStatInternal("SatisfiedJackOfAllTrades", 1);
						}
					}
					NetworkCommunicator myPeer = GameNetwork.MyPeer;
					MissionPeer missionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
					if (missionPeer != null && this._missionLobbyComponent.MissionType == MultiplayerGameType.Captain && (((affectorAgent != null) ? affectorAgent.MissionPeer : null) == missionPeer || ((affectorAgent != null) ? affectorAgent.OwningAgentMissionPeer : null) == missionPeer))
					{
						this._cachedKillCountCaptain++;
						this.SetStatInternal("KillCountCaptain", this._cachedKillCountCaptain);
					}
				}
				if (affectedAgent.IsMine)
				{
					this._hasStolenMount = false;
					this._killsWithAStolenHorse = 0;
				}
			}
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00006C44 File Offset: 0x00004E44
		private async void CacheAndInitializeAchievementVariables()
		{
			int[] array = await AchievementManager.GetStats(new string[] { "MaxMultiKillsWithSingleMangonelShot", "KillsWithBoulder", "KillsWithChainAttack", "KillsWithRangedHeadshots", "KillsWithRangedMounted", "KillsWithCouchedLance", "KillsWithHorseCharge", "KillCountCaptain", "KillsWithStolenHorse" });
			if (array != null)
			{
				int num = 0;
				int cachedMaxMultiKillsWithSingleMangonelShot = this._cachedMaxMultiKillsWithSingleMangonelShot;
				int[] array2 = array;
				num++;
				this._cachedMaxMultiKillsWithSingleMangonelShot = cachedMaxMultiKillsWithSingleMangonelShot + array2[num];
				int cachedKillsWithBoulder = this._cachedKillsWithBoulder;
				int[] array3 = array;
				num++;
				this._cachedKillsWithBoulder = cachedKillsWithBoulder + array3[num];
				int cachedKillsWithChainAttack = this._cachedKillsWithChainAttack;
				int[] array4 = array;
				num++;
				this._cachedKillsWithChainAttack = cachedKillsWithChainAttack + array4[num];
				int cachedKillsWithRangedHeadShots = this._cachedKillsWithRangedHeadShots;
				int[] array5 = array;
				num++;
				this._cachedKillsWithRangedHeadShots = cachedKillsWithRangedHeadShots + array5[num];
				int cachedKillsWithRangedMounted = this._cachedKillsWithRangedMounted;
				int[] array6 = array;
				num++;
				this._cachedKillsWithRangedMounted = cachedKillsWithRangedMounted + array6[num];
				int cachedKillsWithCouchedLance = this._cachedKillsWithCouchedLance;
				int[] array7 = array;
				num++;
				this._cachedKillsWithCouchedLance = cachedKillsWithCouchedLance + array7[num];
				int cachedKillsWithHorseCharge = this._cachedKillsWithHorseCharge;
				int[] array8 = array;
				num++;
				this._cachedKillsWithHorseCharge = cachedKillsWithHorseCharge + array8[num];
				int cachedKillCountCaptain = this._cachedKillCountCaptain;
				int[] array9 = array;
				num++;
				this._cachedKillCountCaptain = cachedKillCountCaptain + array9[num];
				int cachedKillsWithStolenHorse = this._cachedKillsWithStolenHorse;
				int[] array10 = array;
				num++;
				this._cachedKillsWithStolenHorse = cachedKillsWithStolenHorse + array10[num];
			}
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00006C7D File Offset: 0x00004E7D
		private void SetStatInternal(string statId, int value)
		{
			AchievementManager.SetStat(statId, value);
		}

		// Token: 0x0400003E RID: 62
		private const float SingleMangonelShotTimeout = 4f;

		// Token: 0x0400003F RID: 63
		private const string MaxMultiKillsWithSingleMangonelShotStatID = "MaxMultiKillsWithSingleMangonelShot";

		// Token: 0x04000040 RID: 64
		private const string KillsWithBoulderStatID = "KillsWithBoulder";

		// Token: 0x04000041 RID: 65
		private const string KillsWithChainAttackStatID = "KillsWithChainAttack";

		// Token: 0x04000042 RID: 66
		private const string KillsWithRangedHeadShotsStatID = "KillsWithRangedHeadshots";

		// Token: 0x04000043 RID: 67
		private const string KillsWithRangedMountedStatID = "KillsWithRangedMounted";

		// Token: 0x04000044 RID: 68
		private const string KillsWithCouchedLanceStatID = "KillsWithCouchedLance";

		// Token: 0x04000045 RID: 69
		private const string KillsWithHorseChargeStatID = "KillsWithHorseCharge";

		// Token: 0x04000046 RID: 70
		private const string KillCountCaptainStatID = "KillCountCaptain";

		// Token: 0x04000047 RID: 71
		private const string KillsWithStolenHorse = "KillsWithStolenHorse";

		// Token: 0x04000048 RID: 72
		private const string SatisfiedJackOfAllTradesStatID = "SatisfiedJackOfAllTrades";

		// Token: 0x04000049 RID: 73
		private const string PushedSomeoneOffLedgeStatID = "PushedSomeoneOffLedge";

		// Token: 0x0400004A RID: 74
		private int _cachedMaxMultiKillsWithSingleMangonelShot;

		// Token: 0x0400004B RID: 75
		private int _cachedKillsWithBoulder;

		// Token: 0x0400004C RID: 76
		private int _cachedKillsWithChainAttack;

		// Token: 0x0400004D RID: 77
		private int _cachedKillsWithRangedHeadShots;

		// Token: 0x0400004E RID: 78
		private int _cachedKillsWithRangedMounted;

		// Token: 0x0400004F RID: 79
		private int _cachedKillsWithCouchedLance;

		// Token: 0x04000050 RID: 80
		private int _cachedKillsWithHorseCharge;

		// Token: 0x04000051 RID: 81
		private int _cachedKillCountCaptain;

		// Token: 0x04000052 RID: 82
		private int _cachedKillsWithStolenHorse;

		// Token: 0x04000053 RID: 83
		private int _singleRoundKillsWithMeleeOnFoot;

		// Token: 0x04000054 RID: 84
		private int _singleRoundKillsWithMeleeMounted;

		// Token: 0x04000055 RID: 85
		private int _singleRoundKillsWithRangedOnFoot;

		// Token: 0x04000056 RID: 86
		private int _singleRoundKillsWithRangedMounted;

		// Token: 0x04000057 RID: 87
		private int _singleRoundKillsWithCouchedLance;

		// Token: 0x04000058 RID: 88
		private int _killsWithAStolenHorse;

		// Token: 0x04000059 RID: 89
		private bool _hasStolenMount;

		// Token: 0x0400005A RID: 90
		private MissionLobbyComponent _missionLobbyComponent;

		// Token: 0x0400005B RID: 91
		private MultiplayerRoundComponent _multiplayerRoundComponent;

		// Token: 0x0400005C RID: 92
		private Queue<MultiplayerAchievementComponent.BoulderKillRecord> _recentBoulderKills;

		// Token: 0x0200009D RID: 157
		private struct BoulderKillRecord
		{
			// Token: 0x06000417 RID: 1047 RVA: 0x00012312 File Offset: 0x00010512
			public BoulderKillRecord(float time)
			{
				this.Time = time;
			}

			// Token: 0x04000193 RID: 403
			public readonly float Time;
		}
	}
}
