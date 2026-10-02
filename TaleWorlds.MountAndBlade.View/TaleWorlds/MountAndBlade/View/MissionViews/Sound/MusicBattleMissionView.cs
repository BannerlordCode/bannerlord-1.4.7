using System;
using System.Collections.Generic;
using System.Linq;
using psai.net;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.MissionViews.Sound
{
	// Token: 0x02000086 RID: 134
	public class MusicBattleMissionView : MissionView, IMusicHandler
	{
		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600050F RID: 1295 RVA: 0x0002582C File Offset: 0x00023A2C
		bool IMusicHandler.IsPausable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000510 RID: 1296 RVA: 0x0002582F File Offset: 0x00023A2F
		private BattleSideEnum PlayerSide
		{
			get
			{
				Team playerTeam = Mission.Current.PlayerTeam;
				if (playerTeam == null)
				{
					return BattleSideEnum.None;
				}
				return playerTeam.Side;
			}
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00025846 File Offset: 0x00023A46
		public MusicBattleMissionView(bool isSiegeBattle)
		{
			this._isSiegeBattle = isSiegeBattle;
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00025855 File Offset: 0x00023A55
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._missionAgentSpawnLogic = Mission.Current.GetMissionBehavior<DefaultBattleMissionAgentSpawnLogic>();
			MBMusicManager.Current.DeactivateCurrentMode();
			MBMusicManager.Current.ActivateBattleMode();
			MBMusicManager.Current.OnBattleMusicHandlerInit(this);
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0002588C File Offset: 0x00023A8C
		public override void OnMissionScreenFinalize()
		{
			MBMusicManager.Current.DeactivateBattleMode();
			MBMusicManager.Current.OnBattleMusicHandlerFinalize();
			base.Mission.PlayerTeam.PlayerOrderController.OnOrderIssued -= new OnOrderIssuedDelegate(this.PlayerOrderControllerOnOrderIssued);
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x000258C3 File Offset: 0x00023AC3
		public override void AfterStart()
		{
			this._nextPossibleTimeToIncreaseIntensityForChargeOrder = MissionTime.Now;
			base.Mission.PlayerTeam.PlayerOrderController.OnOrderIssued += new OnOrderIssuedDelegate(this.PlayerOrderControllerOnOrderIssued);
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x000258F4 File Offset: 0x00023AF4
		private void PlayerOrderControllerOnOrderIssued(OrderType orderType, IEnumerable<Formation> appliedFormations, OrderController orderController, object[] parameters)
		{
			if ((orderType == OrderType.Charge || orderType == OrderType.ChargeWithTarget) && this._nextPossibleTimeToIncreaseIntensityForChargeOrder.IsPast)
			{
				float currentIntensity = PsaiCore.Instance.GetCurrentIntensity();
				float num = currentIntensity * MusicParameters.PlayerChargeEffectMultiplierOnIntensity - currentIntensity;
				MBMusicManager.Current.ChangeCurrentThemeIntensity(num);
				this._nextPossibleTimeToIncreaseIntensityForChargeOrder = MissionTime.Now + MissionTime.Seconds(60f);
			}
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00025950 File Offset: 0x00023B50
		private void CheckIntensityFall()
		{
			PsaiInfo psaiInfo = PsaiCore.Instance.GetPsaiInfo();
			if (psaiInfo.effectiveThemeId >= 0)
			{
				if (float.IsNaN(psaiInfo.currentIntensity))
				{
					MBMusicManager.Current.ChangeCurrentThemeIntensity(MusicParameters.MinIntensity);
					return;
				}
				if (psaiInfo.currentIntensity < MusicParameters.MinIntensity)
				{
					MBMusicManager.Current.ChangeCurrentThemeIntensity(MusicParameters.MinIntensity - psaiInfo.currentIntensity);
				}
			}
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x000259B8 File Offset: 0x00023BB8
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (this._battleState != MusicBattleMissionView.BattleState.Starting)
			{
				bool flag = affectedAgent.IsMine || (affectedAgent.RiderAgent != null && affectedAgent.RiderAgent.IsMine);
				Team team = affectedAgent.Team;
				BattleSideEnum battleSideEnum = ((team != null) ? team.Side : BattleSideEnum.None);
				bool flag2;
				if (!flag)
				{
					if (battleSideEnum != BattleSideEnum.None)
					{
						Team playerTeam = Mission.Current.PlayerTeam;
						flag2 = ((playerTeam != null) ? playerTeam.Side : BattleSideEnum.None) == battleSideEnum;
					}
					else
					{
						flag2 = false;
					}
				}
				else
				{
					flag2 = true;
				}
				bool flag3 = flag2;
				if (!this._isSiegeBattle && affectedAgent.IsHuman && battleSideEnum != BattleSideEnum.None && this._battleState == MusicBattleMissionView.BattleState.Started && this._startingTroopCounts.Sum() >= MusicParameters.SmallBattleTreshold && MissionTime.Now.ToSeconds > (double)MusicParameters.BattleTurnsOneSideCooldown && this._missionAgentSpawnLogic.NumberOfRemainingTroops == 0)
				{
					int[] array = new int[]
					{
						this._missionAgentSpawnLogic.NumberOfActiveDefenderTroops,
						this._missionAgentSpawnLogic.NumberOfActiveAttackerTroops
					};
					array[(int)battleSideEnum]--;
					MusicTheme musicTheme = MusicTheme.None;
					if (array[0] > 0 && array[1] > 0)
					{
						float num = (float)array[0] / (float)array[1];
						if (num < this._startingBattleRatio * MusicParameters.BattleRatioTresholdOnIntensity)
						{
							musicTheme = MBMusicManager.Current.GetBattleTurnsOneSideTheme(base.Mission.MusicCulture, this.PlayerSide > BattleSideEnum.Defender, this._isPaganBattle);
						}
						else if (num > this._startingBattleRatio / MusicParameters.BattleRatioTresholdOnIntensity)
						{
							musicTheme = MBMusicManager.Current.GetBattleTurnsOneSideTheme(base.Mission.MusicCulture, this.PlayerSide == BattleSideEnum.Defender, this._isPaganBattle);
						}
					}
					if (musicTheme != MusicTheme.None)
					{
						MBMusicManager.Current.StartTheme(musicTheme, PsaiCore.Instance.GetCurrentIntensity(), false);
						this._battleState = MusicBattleMissionView.BattleState.TurnedOneSide;
					}
				}
				if ((affectedAgent.IsHuman && affectedAgent.State != AgentState.Routed) || flag)
				{
					float num2 = (flag3 ? MusicParameters.FriendlyTroopDeadEffectOnIntensity : MusicParameters.EnemyTroopDeadEffectOnIntensity);
					if (flag)
					{
						num2 *= MusicParameters.PlayerTroopDeadEffectMultiplierOnIntensity;
					}
					MBMusicManager.Current.ChangeCurrentThemeIntensity(num2);
				}
			}
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00025BBC File Offset: 0x00023DBC
		private void CheckForStarting()
		{
			if (this._startingTroopCounts == null)
			{
				this._startingTroopCounts = new int[]
				{
					this._missionAgentSpawnLogic.GetTotalNumberOfTroopsForSide(BattleSideEnum.Defender),
					this._missionAgentSpawnLogic.GetTotalNumberOfTroopsForSide(BattleSideEnum.Attacker)
				};
				this._startingBattleRatio = (float)this._startingTroopCounts[0] / (float)this._startingTroopCounts[1];
			}
			Agent main = Agent.Main;
			Vec2 vec = ((main != null) ? main.Position.AsVec2 : Vec2.Invalid);
			Team playerTeam = Mission.Current.PlayerTeam;
			bool flag;
			if (playerTeam == null)
			{
				flag = false;
			}
			else
			{
				flag = playerTeam.FormationsIncludingEmpty.Any<Formation>((Formation f) => f.CountOfUnits > 0);
			}
			bool flag2 = flag;
			float num = float.MaxValue;
			if (flag2 || vec.IsValid)
			{
				foreach (Formation formation in Mission.Current.PlayerEnemyTeam.FormationsIncludingEmpty)
				{
					if (formation.CountOfUnits > 0)
					{
						float num2 = float.MaxValue;
						if (!flag2 && vec.IsValid)
						{
							num2 = vec.DistanceSquared(formation.CurrentPosition);
						}
						else if (flag2)
						{
							foreach (Formation formation2 in Mission.Current.PlayerTeam.FormationsIncludingEmpty)
							{
								if (formation2.CountOfUnits > 0)
								{
									float num3 = formation2.CurrentPosition.DistanceSquared(formation.CurrentPosition);
									if (num2 > num3)
									{
										num2 = num3;
									}
								}
							}
						}
						if (num > num2)
						{
							num = num2;
						}
					}
				}
			}
			int num4 = this._startingTroopCounts.Sum();
			bool flag3 = false;
			if (num4 < MusicParameters.SmallBattleTreshold)
			{
				if (num < MusicParameters.SmallBattleDistanceTreshold * MusicParameters.SmallBattleDistanceTreshold)
				{
					flag3 = true;
				}
			}
			else if (num4 < MusicParameters.MediumBattleTreshold)
			{
				if (num < MusicParameters.MediumBattleDistanceTreshold * MusicParameters.MediumBattleDistanceTreshold)
				{
					flag3 = true;
				}
			}
			else if (num4 < MusicParameters.LargeBattleTreshold)
			{
				if (num < MusicParameters.LargeBattleDistanceTreshold * MusicParameters.LargeBattleDistanceTreshold)
				{
					flag3 = true;
				}
			}
			else if (num < MusicParameters.MaxBattleDistanceTreshold * MusicParameters.MaxBattleDistanceTreshold)
			{
				flag3 = true;
			}
			if (flag3)
			{
				float num5 = (float)num4 / 1000f;
				float num6 = MusicParameters.DefaultStartIntensity + num5 * MusicParameters.BattleSizeEffectOnStartIntensity + (MBRandom.RandomFloat - 0.5f) * (MusicParameters.RandomEffectMultiplierOnStartIntensity * 2f);
				MusicTheme musicTheme = (this._isSiegeBattle ? MBMusicManager.Current.GetSiegeTheme(base.Mission.MusicCulture) : MBMusicManager.Current.GetBattleTheme(base.Mission.MusicCulture, num4, out this._isPaganBattle));
				MBMusicManager.Current.StartTheme(musicTheme, num6, false);
				this._battleState = MusicBattleMissionView.BattleState.Started;
			}
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00025E80 File Offset: 0x00024080
		private void CheckForEnding()
		{
			if (Mission.Current.IsMissionEnding)
			{
				if (Mission.Current.MissionResult != null)
				{
					MusicTheme battleEndTheme = MBMusicManager.Current.GetBattleEndTheme(base.Mission.MusicCulture, Mission.Current.MissionResult.PlayerVictory);
					MBMusicManager.Current.StartTheme(battleEndTheme, PsaiCore.Instance.GetPsaiInfo().currentIntensity, true);
					this._battleState = MusicBattleMissionView.BattleState.Ending;
					return;
				}
				MBMusicManager.Current.StartTheme(MusicTheme.BattleDefeat, PsaiCore.Instance.GetPsaiInfo().currentIntensity, true);
				this._battleState = MusicBattleMissionView.BattleState.Ending;
			}
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00025F18 File Offset: 0x00024118
		void IMusicHandler.OnUpdated(float dt)
		{
			if (this._battleState == MusicBattleMissionView.BattleState.Starting)
			{
				if (base.Mission.MusicCulture == null && Mission.Current.GetMissionBehavior<DeploymentHandler>() == null && this._missionAgentSpawnLogic.IsDeploymentOver)
				{
					KeyValuePair<BasicCultureObject, int> keyValuePair = new KeyValuePair<BasicCultureObject, int>(null, -1);
					Dictionary<BasicCultureObject, int> dictionary = new Dictionary<BasicCultureObject, int>();
					foreach (Team team in base.Mission.Teams)
					{
						foreach (Agent agent in team.ActiveAgents)
						{
							BasicCultureObject culture = agent.Character.Culture;
							if (culture != null && culture.IsMainCulture)
							{
								if (!dictionary.ContainsKey(agent.Character.Culture))
								{
									dictionary.Add(agent.Character.Culture, 0);
								}
								Dictionary<BasicCultureObject, int> dictionary2 = dictionary;
								BasicCultureObject culture2 = agent.Character.Culture;
								int num = dictionary2[culture2];
								dictionary2[culture2] = num + 1;
								if (dictionary[agent.Character.Culture] > keyValuePair.Value)
								{
									keyValuePair = new KeyValuePair<BasicCultureObject, int>(agent.Character.Culture, dictionary[agent.Character.Culture]);
								}
							}
						}
					}
					if (keyValuePair.Key != null)
					{
						base.Mission.MusicCulture = keyValuePair.Key;
					}
					else
					{
						base.Mission.MusicCulture = Game.Current.PlayerTroop.Culture;
					}
				}
				if (base.Mission.MusicCulture != null)
				{
					this.CheckForStarting();
				}
			}
			if (this._battleState == MusicBattleMissionView.BattleState.Started || this._battleState == MusicBattleMissionView.BattleState.TurnedOneSide)
			{
				this.CheckForEnding();
			}
			this.CheckIntensityFall();
		}

		// Token: 0x040002D9 RID: 729
		private const float ChargeOrderIntensityIncreaseCooldownInSeconds = 60f;

		// Token: 0x040002DA RID: 730
		private MusicBattleMissionView.BattleState _battleState;

		// Token: 0x040002DB RID: 731
		private DefaultBattleMissionAgentSpawnLogic _missionAgentSpawnLogic;

		// Token: 0x040002DC RID: 732
		private int[] _startingTroopCounts;

		// Token: 0x040002DD RID: 733
		private float _startingBattleRatio;

		// Token: 0x040002DE RID: 734
		private bool _isSiegeBattle;

		// Token: 0x040002DF RID: 735
		private bool _isPaganBattle;

		// Token: 0x040002E0 RID: 736
		private MissionTime _nextPossibleTimeToIncreaseIntensityForChargeOrder;

		// Token: 0x020000E5 RID: 229
		private enum BattleState
		{
			// Token: 0x040003EB RID: 1003
			Starting,
			// Token: 0x040003EC RID: 1004
			Started,
			// Token: 0x040003ED RID: 1005
			TurnedOneSide,
			// Token: 0x040003EE RID: 1006
			Ending
		}
	}
}
