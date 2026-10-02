using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions.Handlers;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200017C RID: 380
	public class TeamQuerySystem
	{
		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06001420 RID: 5152 RVA: 0x0004A821 File Offset: 0x00048A21
		public int MemberCount
		{
			get
			{
				return this._memberCount.Value;
			}
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06001421 RID: 5153 RVA: 0x0004A82E File Offset: 0x00048A2E
		public WorldPosition MedianPosition
		{
			get
			{
				return this._medianPosition.Value;
			}
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06001422 RID: 5154 RVA: 0x0004A83B File Offset: 0x00048A3B
		public Vec2 AveragePosition
		{
			get
			{
				return this._averagePosition.Value;
			}
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06001423 RID: 5155 RVA: 0x0004A848 File Offset: 0x00048A48
		public Vec2 AverageEnemyPosition
		{
			get
			{
				return this._averageEnemyPosition.Value;
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06001424 RID: 5156 RVA: 0x0004A855 File Offset: 0x00048A55
		public FormationQuerySystem MedianTargetFormation
		{
			get
			{
				return this._medianTargetFormation.Value;
			}
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06001425 RID: 5157 RVA: 0x0004A862 File Offset: 0x00048A62
		public WorldPosition MedianTargetFormationPosition
		{
			get
			{
				return this._medianTargetFormationPosition.Value;
			}
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06001426 RID: 5158 RVA: 0x0004A86F File Offset: 0x00048A6F
		public WorldPosition LeftFlankEdgePosition
		{
			get
			{
				return this._leftFlankEdgePosition.Value;
			}
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06001427 RID: 5159 RVA: 0x0004A87C File Offset: 0x00048A7C
		public WorldPosition RightFlankEdgePosition
		{
			get
			{
				return this._rightFlankEdgePosition.Value;
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06001428 RID: 5160 RVA: 0x0004A889 File Offset: 0x00048A89
		public float InfantryRatio
		{
			get
			{
				return this._infantryRatio.Value;
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06001429 RID: 5161 RVA: 0x0004A896 File Offset: 0x00048A96
		public float RangedRatio
		{
			get
			{
				return this._rangedRatio.Value;
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x0600142A RID: 5162 RVA: 0x0004A8A3 File Offset: 0x00048AA3
		public float CavalryRatio
		{
			get
			{
				return this._cavalryRatio.Value;
			}
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x0600142B RID: 5163 RVA: 0x0004A8B0 File Offset: 0x00048AB0
		public float RangedCavalryRatio
		{
			get
			{
				return this._rangedCavalryRatio.Value;
			}
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x0600142C RID: 5164 RVA: 0x0004A8BD File Offset: 0x00048ABD
		public int AllyUnitCount
		{
			get
			{
				return this._allyMemberCount.Value;
			}
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x0600142D RID: 5165 RVA: 0x0004A8CA File Offset: 0x00048ACA
		public int EnemyUnitCount
		{
			get
			{
				return this._enemyMemberCount.Value;
			}
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x0600142E RID: 5166 RVA: 0x0004A8D7 File Offset: 0x00048AD7
		public float AllyInfantryRatio
		{
			get
			{
				return this._allyInfantryRatio.Value;
			}
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x0600142F RID: 5167 RVA: 0x0004A8E4 File Offset: 0x00048AE4
		public float AllyRangedRatio
		{
			get
			{
				return this._allyRangedRatio.Value;
			}
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06001430 RID: 5168 RVA: 0x0004A8F1 File Offset: 0x00048AF1
		public float AllyCavalryRatio
		{
			get
			{
				return this._allyCavalryRatio.Value;
			}
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06001431 RID: 5169 RVA: 0x0004A8FE File Offset: 0x00048AFE
		public float AllyRangedCavalryRatio
		{
			get
			{
				return this._allyRangedCavalryRatio.Value;
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06001432 RID: 5170 RVA: 0x0004A90B File Offset: 0x00048B0B
		public float EnemyInfantryRatio
		{
			get
			{
				return this._enemyInfantryRatio.Value;
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06001433 RID: 5171 RVA: 0x0004A918 File Offset: 0x00048B18
		public float EnemyRangedRatio
		{
			get
			{
				return this._enemyRangedRatio.Value;
			}
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06001434 RID: 5172 RVA: 0x0004A925 File Offset: 0x00048B25
		public float EnemyCavalryRatio
		{
			get
			{
				return this._enemyCavalryRatio.Value;
			}
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06001435 RID: 5173 RVA: 0x0004A932 File Offset: 0x00048B32
		public float EnemyRangedCavalryRatio
		{
			get
			{
				return this._enemyRangedCavalryRatio.Value;
			}
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06001436 RID: 5174 RVA: 0x0004A93F File Offset: 0x00048B3F
		public float RemainingPowerRatio
		{
			get
			{
				return this._remainingPowerRatio.Value;
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06001437 RID: 5175 RVA: 0x0004A94C File Offset: 0x00048B4C
		public float TeamPower
		{
			get
			{
				return this._teamPower.Value;
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06001438 RID: 5176 RVA: 0x0004A959 File Offset: 0x00048B59
		public float TotalPowerRatio
		{
			get
			{
				return this._totalPowerRatio.Value;
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06001439 RID: 5177 RVA: 0x0004A966 File Offset: 0x00048B66
		public float InsideWallsRatio
		{
			get
			{
				return this._insideWallsRatio.Value;
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x0600143A RID: 5178 RVA: 0x0004A973 File Offset: 0x00048B73
		public IBattlePowerCalculationLogic BattlePowerLogic
		{
			get
			{
				if (this._battlePowerLogic == null)
				{
					this._battlePowerLogic = this._mission.GetMissionBehavior<IBattlePowerCalculationLogic>();
				}
				return this._battlePowerLogic;
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x0600143B RID: 5179 RVA: 0x0004A994 File Offset: 0x00048B94
		public CasualtyHandler CasualtyHandler
		{
			get
			{
				if (this._casualtyHandler == null)
				{
					this._casualtyHandler = this._mission.GetMissionBehavior<CasualtyHandler>();
				}
				return this._casualtyHandler;
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x0600143C RID: 5180 RVA: 0x0004A9B5 File Offset: 0x00048BB5
		public float MaxUnderRangedAttackRatio
		{
			get
			{
				return this._maxUnderRangedAttackRatio.Value;
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x0600143D RID: 5181 RVA: 0x0004A9C2 File Offset: 0x00048BC2
		// (set) Token: 0x0600143E RID: 5182 RVA: 0x0004A9CA File Offset: 0x00048BCA
		public int DeathCount { get; private set; }

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x0600143F RID: 5183 RVA: 0x0004A9D3 File Offset: 0x00048BD3
		// (set) Token: 0x06001440 RID: 5184 RVA: 0x0004A9DB File Offset: 0x00048BDB
		public int DeathByRangedCount { get; private set; }

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06001441 RID: 5185 RVA: 0x0004A9E4 File Offset: 0x00048BE4
		public int AllyRangedUnitCount
		{
			get
			{
				return (int)(this.AllyRangedRatio * (float)this.AllyUnitCount);
			}
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06001442 RID: 5186 RVA: 0x0004A9F5 File Offset: 0x00048BF5
		public int AllCavalryUnitCount
		{
			get
			{
				return (int)(this.AllyCavalryRatio * (float)this.AllyUnitCount);
			}
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06001443 RID: 5187 RVA: 0x0004AA06 File Offset: 0x00048C06
		public int EnemyRangedUnitCount
		{
			get
			{
				return (int)(this.EnemyRangedRatio * (float)this.EnemyUnitCount);
			}
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x0004AA18 File Offset: 0x00048C18
		public void Expire()
		{
			this._memberCount.Expire();
			this._medianPosition.Expire();
			this._averagePosition.Expire();
			this._averageEnemyPosition.Expire();
			this._medianTargetFormationPosition.Expire();
			this._leftFlankEdgePosition.Expire();
			this._rightFlankEdgePosition.Expire();
			this._infantryRatio.Expire();
			this._rangedRatio.Expire();
			this._cavalryRatio.Expire();
			this._rangedCavalryRatio.Expire();
			this._allyMemberCount.Expire();
			this._enemyMemberCount.Expire();
			this._allyInfantryRatio.Expire();
			this._allyRangedRatio.Expire();
			this._allyCavalryRatio.Expire();
			this._allyRangedCavalryRatio.Expire();
			this._enemyInfantryRatio.Expire();
			this._enemyRangedRatio.Expire();
			this._enemyCavalryRatio.Expire();
			this._enemyRangedCavalryRatio.Expire();
			this._remainingPowerRatio.Expire();
			this._teamPower.Expire();
			this._totalPowerRatio.Expire();
			this._insideWallsRatio.Expire();
			this._maxUnderRangedAttackRatio.Expire();
			foreach (Formation formation in this.Team.FormationsIncludingSpecialAndEmpty)
			{
				if (formation.CountOfUnits > 0)
				{
					formation.QuerySystem.Expire();
				}
			}
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x0004AB9C File Offset: 0x00048D9C
		public void ExpireAfterUnitAddRemove()
		{
			this._memberCount.Expire();
			this._medianPosition.Expire();
			this._averagePosition.Expire();
			this._leftFlankEdgePosition.Expire();
			this._rightFlankEdgePosition.Expire();
			this._infantryRatio.Expire();
			this._rangedRatio.Expire();
			this._cavalryRatio.Expire();
			this._rangedCavalryRatio.Expire();
			this._allyMemberCount.Expire();
			this._allyInfantryRatio.Expire();
			this._allyRangedRatio.Expire();
			this._allyCavalryRatio.Expire();
			this._allyRangedCavalryRatio.Expire();
			this._remainingPowerRatio.Expire();
			this._teamPower.Expire();
			this._totalPowerRatio.Expire();
			this._insideWallsRatio.Expire();
			this._maxUnderRangedAttackRatio.Expire();
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x0004AC7A File Offset: 0x00048E7A
		private void InitializeTelemetryScopeNames()
		{
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x0004AC7C File Offset: 0x00048E7C
		public TeamQuerySystem(Team team)
		{
			TeamQuerySystem <>4__this = this;
			this.Team = team;
			this._mission = Mission.Current;
			this._memberCount = new QueryData<int>(delegate
			{
				int num = 0;
				foreach (Formation formation in <>4__this.Team.FormationsIncludingSpecialAndEmpty)
				{
					num += formation.CountOfUnits;
				}
				return num;
			}, 2f);
			this._allyMemberCount = new QueryData<int>(delegate
			{
				int num2 = 0;
				foreach (Team team2 in <>4__this._mission.Teams)
				{
					if (team2.IsFriendOf(<>4__this.Team))
					{
						num2 += team2.QuerySystem.MemberCount;
					}
				}
				return num2;
			}, 2f);
			this._enemyMemberCount = new QueryData<int>(delegate
			{
				int num3 = 0;
				foreach (Team team3 in <>4__this._mission.Teams)
				{
					if (team3.IsEnemyOf(<>4__this.Team))
					{
						num3 += team3.QuerySystem.MemberCount;
					}
				}
				return num3;
			}, 2f);
			this._averagePosition = new QueryData<Vec2>(new Func<Vec2>(team.GetAveragePosition), 5f);
			this._medianPosition = new QueryData<WorldPosition>(() => team.GetMedianPosition(<>4__this.AveragePosition), 5f);
			this._averageEnemyPosition = new QueryData<Vec2>(delegate
			{
				Vec2 averagePositionOfEnemies = team.GetAveragePositionOfEnemies();
				if (averagePositionOfEnemies.IsValid)
				{
					return averagePositionOfEnemies;
				}
				if (team.Side == BattleSideEnum.Attacker)
				{
					SiegeDeploymentHandler missionBehavior = <>4__this._mission.GetMissionBehavior<SiegeDeploymentHandler>();
					if (missionBehavior != null)
					{
						return missionBehavior.GetEstimatedAverageDefenderPosition();
					}
				}
				if (!<>4__this.AveragePosition.IsValid)
				{
					return team.GetAveragePosition();
				}
				return <>4__this.AveragePosition;
			}, 5f);
			this._medianTargetFormation = new QueryData<FormationQuerySystem>(delegate
			{
				float num4 = float.MaxValue;
				Formation formation2 = null;
				foreach (Team team4 in <>4__this._mission.Teams)
				{
					if (team4.IsEnemyOf(<>4__this.Team))
					{
						foreach (Formation formation3 in team4.FormationsIncludingSpecialAndEmpty)
						{
							if (formation3.CountOfUnits > 0)
							{
								float num5 = formation3.CachedMedianPosition.AsVec2.DistanceSquared(<>4__this.AverageEnemyPosition);
								if (num4 > num5)
								{
									num4 = num5;
									formation2 = formation3;
								}
							}
						}
					}
				}
				if (formation2 != null)
				{
					return formation2.QuerySystem;
				}
				return null;
			}, 1f);
			this._medianTargetFormationPosition = new QueryData<WorldPosition>(delegate
			{
				if (<>4__this.MedianTargetFormation != null)
				{
					return <>4__this.MedianTargetFormation.Formation.CachedMedianPosition;
				}
				return <>4__this.MedianPosition;
			}, 1f);
			QueryData<WorldPosition>.SetupSyncGroup(new IQueryData[] { this._averageEnemyPosition, this._medianTargetFormationPosition });
			this._leftFlankEdgePosition = new QueryData<WorldPosition>(delegate
			{
				Vec2 vec = (<>4__this.MedianTargetFormationPosition.AsVec2 - <>4__this.AveragePosition).RightVec();
				vec.Normalize();
				WorldPosition medianTargetFormationPosition = <>4__this.MedianTargetFormationPosition;
				medianTargetFormationPosition.SetVec2(medianTargetFormationPosition.AsVec2 - vec * 50f);
				return medianTargetFormationPosition;
			}, 5f);
			this._rightFlankEdgePosition = new QueryData<WorldPosition>(delegate
			{
				Vec2 vec2 = (<>4__this.MedianTargetFormationPosition.AsVec2 - <>4__this.AveragePosition).RightVec();
				vec2.Normalize();
				WorldPosition medianTargetFormationPosition2 = <>4__this.MedianTargetFormationPosition;
				medianTargetFormationPosition2.SetVec2(medianTargetFormationPosition2.AsVec2 + vec2 * 50f);
				return medianTargetFormationPosition2;
			}, 5f);
			this._infantryRatio = new QueryData<float>(delegate
			{
				if (<>4__this.MemberCount != 0)
				{
					return (<>4__this.Team.FormationsIncludingSpecialAndEmpty.Sum<Formation>(delegate(Formation f)
					{
						if (f.CountOfUnits <= 0)
						{
							return 0f;
						}
						return f.QuerySystem.InfantryUnitRatio * (float)f.CountOfUnits;
					}) + (float)team.Heroes.Count<Agent>((Agent h) => QueryLibrary.IsInfantry(h))) / (float)<>4__this.MemberCount;
				}
				return 0f;
			}, 15f);
			this._rangedRatio = new QueryData<float>(delegate
			{
				if (<>4__this.MemberCount != 0)
				{
					return (<>4__this.Team.FormationsIncludingSpecialAndEmpty.Sum<Formation>(delegate(Formation f)
					{
						if (f.CountOfUnits <= 0)
						{
							return 0f;
						}
						return f.QuerySystem.RangedUnitRatio * (float)f.CountOfUnits;
					}) + (float)team.Heroes.Count<Agent>((Agent h) => QueryLibrary.IsRanged(h))) / (float)<>4__this.MemberCount;
				}
				return 0f;
			}, 15f);
			this._cavalryRatio = new QueryData<float>(delegate
			{
				if (<>4__this.MemberCount != 0)
				{
					return (<>4__this.Team.FormationsIncludingSpecialAndEmpty.Sum<Formation>(delegate(Formation f)
					{
						if (f.CountOfUnits <= 0)
						{
							return 0f;
						}
						return f.QuerySystem.CavalryUnitRatio * (float)f.CountOfUnits;
					}) + (float)team.Heroes.Count<Agent>((Agent h) => QueryLibrary.IsCavalry(h))) / (float)<>4__this.MemberCount;
				}
				return 0f;
			}, 15f);
			this._rangedCavalryRatio = new QueryData<float>(delegate
			{
				if (<>4__this.MemberCount != 0)
				{
					return (<>4__this.Team.FormationsIncludingSpecialAndEmpty.Sum<Formation>(delegate(Formation f)
					{
						if (f.CountOfUnits <= 0)
						{
							return 0f;
						}
						return f.QuerySystem.RangedCavalryUnitRatio * (float)f.CountOfUnits;
					}) + (float)team.Heroes.Count<Agent>((Agent h) => QueryLibrary.IsRangedCavalry(h))) / (float)<>4__this.MemberCount;
				}
				return 0f;
			}, 15f);
			QueryData<float>.SetupSyncGroup(new IQueryData[] { this._infantryRatio, this._rangedRatio, this._cavalryRatio, this._rangedCavalryRatio });
			this._allyInfantryRatio = new QueryData<float>(delegate
			{
				float num6 = 0f;
				int num7 = 0;
				foreach (Team team5 in <>4__this._mission.Teams)
				{
					if (team5.IsFriendOf(<>4__this.Team))
					{
						int memberCount = team5.QuerySystem.MemberCount;
						num6 += team5.QuerySystem.InfantryRatio * (float)memberCount;
						num7 += memberCount;
					}
				}
				if (num7 != 0)
				{
					return num6 / (float)num7;
				}
				return 0f;
			}, 15f);
			this._allyRangedRatio = new QueryData<float>(delegate
			{
				float num8 = 0f;
				int num9 = 0;
				foreach (Team team6 in <>4__this._mission.Teams)
				{
					if (team6.IsFriendOf(<>4__this.Team))
					{
						int memberCount2 = team6.QuerySystem.MemberCount;
						num8 += team6.QuerySystem.RangedRatio * (float)memberCount2;
						num9 += memberCount2;
					}
				}
				if (num9 != 0)
				{
					return num8 / (float)num9;
				}
				return 0f;
			}, 15f);
			this._allyCavalryRatio = new QueryData<float>(delegate
			{
				float num10 = 0f;
				int num11 = 0;
				foreach (Team team7 in <>4__this._mission.Teams)
				{
					if (team7.IsFriendOf(<>4__this.Team))
					{
						int memberCount3 = team7.QuerySystem.MemberCount;
						num10 += team7.QuerySystem.CavalryRatio * (float)memberCount3;
						num11 += memberCount3;
					}
				}
				if (num11 != 0)
				{
					return num10 / (float)num11;
				}
				return 0f;
			}, 15f);
			this._allyRangedCavalryRatio = new QueryData<float>(delegate
			{
				float num12 = 0f;
				int num13 = 0;
				foreach (Team team8 in <>4__this._mission.Teams)
				{
					if (team8.IsFriendOf(<>4__this.Team))
					{
						int memberCount4 = team8.QuerySystem.MemberCount;
						num12 += team8.QuerySystem.RangedCavalryRatio * (float)memberCount4;
						num13 += memberCount4;
					}
				}
				if (num13 != 0)
				{
					return num12 / (float)num13;
				}
				return 0f;
			}, 15f);
			QueryData<float>.SetupSyncGroup(new IQueryData[] { this._allyInfantryRatio, this._allyRangedRatio, this._allyCavalryRatio, this._allyRangedCavalryRatio });
			this._enemyInfantryRatio = new QueryData<float>(delegate
			{
				float num14 = 0f;
				int num15 = 0;
				foreach (Team team9 in <>4__this._mission.Teams)
				{
					if (team9.IsEnemyOf(<>4__this.Team))
					{
						int memberCount5 = team9.QuerySystem.MemberCount;
						num14 += team9.QuerySystem.InfantryRatio * (float)memberCount5;
						num15 += memberCount5;
					}
				}
				if (num15 != 0)
				{
					return num14 / (float)num15;
				}
				return 0f;
			}, 15f);
			this._enemyRangedRatio = new QueryData<float>(delegate
			{
				float num16 = 0f;
				int num17 = 0;
				foreach (Team team10 in <>4__this._mission.Teams)
				{
					if (team10.IsEnemyOf(<>4__this.Team))
					{
						int memberCount6 = team10.QuerySystem.MemberCount;
						num16 += team10.QuerySystem.RangedRatio * (float)memberCount6;
						num17 += memberCount6;
					}
				}
				if (num17 != 0)
				{
					return num16 / (float)num17;
				}
				return 0f;
			}, 15f);
			this._enemyCavalryRatio = new QueryData<float>(delegate
			{
				float num18 = 0f;
				int num19 = 0;
				foreach (Team team11 in <>4__this._mission.Teams)
				{
					if (team11.IsEnemyOf(<>4__this.Team))
					{
						int memberCount7 = team11.QuerySystem.MemberCount;
						num18 += team11.QuerySystem.CavalryRatio * (float)memberCount7;
						num19 += memberCount7;
					}
				}
				if (num19 != 0)
				{
					return num18 / (float)num19;
				}
				return 0f;
			}, 15f);
			this._enemyRangedCavalryRatio = new QueryData<float>(delegate
			{
				float num20 = 0f;
				int num21 = 0;
				foreach (Team team12 in <>4__this._mission.Teams)
				{
					if (team12.IsEnemyOf(<>4__this.Team))
					{
						int memberCount8 = team12.QuerySystem.MemberCount;
						num20 += team12.QuerySystem.RangedCavalryRatio * (float)memberCount8;
						num21 += memberCount8;
					}
				}
				if (num21 != 0)
				{
					return num20 / (float)num21;
				}
				return 0f;
			}, 15f);
			this._teamPower = new QueryData<float>(() => team.FormationsIncludingSpecialAndEmpty.Sum<Formation>(delegate(Formation f)
			{
				if (f.CountOfUnits <= 0)
				{
					return 0f;
				}
				return f.GetFormationPower();
			}), 5f);
			this._remainingPowerRatio = new QueryData<float>(delegate
			{
				IBattlePowerCalculationLogic battlePowerLogic = <>4__this.BattlePowerLogic;
				CasualtyHandler casualtyHandler = <>4__this.CasualtyHandler;
				float num22 = 0f;
				float num23 = 0f;
				foreach (Team team13 in <>4__this.Team.Mission.Teams)
				{
					if (team13.IsEnemyOf(<>4__this.Team))
					{
						num23 += battlePowerLogic.GetTotalTeamPower(team13);
						using (List<Formation>.Enumerator enumerator15 = team13.FormationsIncludingSpecialAndEmpty.GetEnumerator())
						{
							while (enumerator15.MoveNext())
							{
								Formation formation4 = enumerator15.Current;
								num23 -= casualtyHandler.GetCasualtyPowerLossOfFormation(formation4);
							}
							continue;
						}
					}
					num22 += battlePowerLogic.GetTotalTeamPower(team13);
					foreach (Formation formation5 in team13.FormationsIncludingSpecialAndEmpty)
					{
						num22 -= casualtyHandler.GetCasualtyPowerLossOfFormation(formation5);
					}
				}
				num22 = MathF.Max(0f, num22);
				num23 = MathF.Max(0f, num23);
				return (num22 + 1f) / (num23 + 1f);
			}, 5f);
			this._totalPowerRatio = new QueryData<float>(delegate
			{
				IBattlePowerCalculationLogic battlePowerLogic2 = <>4__this.BattlePowerLogic;
				float num24 = 0f;
				float num25 = 0f;
				foreach (Team team14 in <>4__this.Team.Mission.Teams)
				{
					if (team14.IsEnemyOf(<>4__this.Team))
					{
						num25 += battlePowerLogic2.GetTotalTeamPower(team14);
					}
					else
					{
						num24 += battlePowerLogic2.GetTotalTeamPower(team14);
					}
				}
				return (num24 + 1f) / (num25 + 1f);
			}, 10f);
			this._insideWallsRatio = new QueryData<float>(delegate
			{
				if (!(team.TeamAI is TeamAISiegeComponent))
				{
					return 1f;
				}
				if (<>4__this.AllyUnitCount == 0)
				{
					return 0f;
				}
				int num26 = 0;
				foreach (Team team15 in Mission.Current.Teams)
				{
					if (team15.IsFriendOf(team))
					{
						foreach (Formation formation6 in team15.FormationsIncludingSpecialAndEmpty)
						{
							if (formation6.CountOfUnits > 0)
							{
								num26 += formation6.CountUnitsOnNavMeshIDMod10(1, false);
							}
						}
					}
				}
				return (float)num26 / (float)<>4__this.AllyUnitCount;
			}, 10f);
			this._maxUnderRangedAttackRatio = new QueryData<float>(delegate
			{
				float num27;
				if (<>4__this.AllyUnitCount == 0)
				{
					num27 = 0f;
				}
				else
				{
					float currentTime = MBCommon.GetTotalMissionTime();
					int num28 = 0;
					Func<Agent, bool> <>9__35;
					foreach (Team team16 in <>4__this._mission.Teams)
					{
						if (team16.IsFriendOf(<>4__this.Team))
						{
							for (int i = 0; i < Math.Min(team16.FormationsIncludingSpecialAndEmpty.Count, 8); i++)
							{
								Formation formation7 = team16.FormationsIncludingSpecialAndEmpty[i];
								if (formation7.CountOfUnits > 0)
								{
									int num29 = num28;
									Formation formation8 = formation7;
									Func<Agent, bool> func;
									if ((func = <>9__35) == null)
									{
										func = (<>9__35 = (Agent agent) => currentTime - agent.LastRangedHitTime < 10f && !agent.Equipment.HasShield());
									}
									num28 = num29 + formation8.GetCountOfUnitsWithCondition(func);
								}
							}
						}
					}
					num27 = (float)num28 / (float)<>4__this.AllyUnitCount;
				}
				if (num27 <= <>4__this._maxUnderRangedAttackRatio.GetCachedValue())
				{
					return <>4__this._maxUnderRangedAttackRatio.GetCachedValue();
				}
				return num27;
			}, 3f);
			this.DeathCount = 0;
			this.DeathByRangedCount = 0;
			this.InitializeTelemetryScopeNames();
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x0004B044 File Offset: 0x00049244
		public void RegisterDeath()
		{
			int deathCount = this.DeathCount;
			this.DeathCount = deathCount + 1;
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x0004B064 File Offset: 0x00049264
		public void RegisterDeathByRanged()
		{
			int deathByRangedCount = this.DeathByRangedCount;
			this.DeathByRangedCount = deathByRangedCount + 1;
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x0004B084 File Offset: 0x00049284
		public float GetLocalAllyPower(Vec2 target)
		{
			return this.Team.FormationsIncludingSpecialAndEmpty.Sum<Formation>(delegate(Formation f)
			{
				if (f.CountOfUnits <= 0)
				{
					return 0f;
				}
				return f.QuerySystem.FormationPower / f.CachedAveragePosition.Distance(target);
			});
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x0004B0BC File Offset: 0x000492BC
		public float GetLocalEnemyPower(Vec2 target)
		{
			float num = 0f;
			foreach (Team team in Mission.Current.Teams)
			{
				if (this.Team.IsEnemyOf(team))
				{
					foreach (Formation formation in team.FormationsIncludingSpecialAndEmpty)
					{
						if (formation.CountOfUnits > 0)
						{
							num += formation.QuerySystem.FormationPower / formation.CachedAveragePosition.Distance(target);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x04000549 RID: 1353
		public readonly Team Team;

		// Token: 0x0400054A RID: 1354
		private readonly Mission _mission;

		// Token: 0x0400054B RID: 1355
		private readonly QueryData<int> _memberCount;

		// Token: 0x0400054C RID: 1356
		private readonly QueryData<WorldPosition> _medianPosition;

		// Token: 0x0400054D RID: 1357
		private readonly QueryData<Vec2> _averagePosition;

		// Token: 0x0400054E RID: 1358
		private readonly QueryData<Vec2> _averageEnemyPosition;

		// Token: 0x0400054F RID: 1359
		private readonly QueryData<FormationQuerySystem> _medianTargetFormation;

		// Token: 0x04000550 RID: 1360
		private readonly QueryData<WorldPosition> _medianTargetFormationPosition;

		// Token: 0x04000551 RID: 1361
		private readonly QueryData<WorldPosition> _leftFlankEdgePosition;

		// Token: 0x04000552 RID: 1362
		private readonly QueryData<WorldPosition> _rightFlankEdgePosition;

		// Token: 0x04000553 RID: 1363
		private readonly QueryData<float> _infantryRatio;

		// Token: 0x04000554 RID: 1364
		private readonly QueryData<float> _rangedRatio;

		// Token: 0x04000555 RID: 1365
		private readonly QueryData<float> _cavalryRatio;

		// Token: 0x04000556 RID: 1366
		private readonly QueryData<float> _rangedCavalryRatio;

		// Token: 0x04000557 RID: 1367
		private readonly QueryData<int> _allyMemberCount;

		// Token: 0x04000558 RID: 1368
		private readonly QueryData<int> _enemyMemberCount;

		// Token: 0x04000559 RID: 1369
		private readonly QueryData<float> _allyInfantryRatio;

		// Token: 0x0400055A RID: 1370
		private readonly QueryData<float> _allyRangedRatio;

		// Token: 0x0400055B RID: 1371
		private readonly QueryData<float> _allyCavalryRatio;

		// Token: 0x0400055C RID: 1372
		private readonly QueryData<float> _allyRangedCavalryRatio;

		// Token: 0x0400055D RID: 1373
		private readonly QueryData<float> _enemyInfantryRatio;

		// Token: 0x0400055E RID: 1374
		private readonly QueryData<float> _enemyRangedRatio;

		// Token: 0x0400055F RID: 1375
		private readonly QueryData<float> _enemyCavalryRatio;

		// Token: 0x04000560 RID: 1376
		private readonly QueryData<float> _enemyRangedCavalryRatio;

		// Token: 0x04000561 RID: 1377
		private readonly QueryData<float> _remainingPowerRatio;

		// Token: 0x04000562 RID: 1378
		private readonly QueryData<float> _teamPower;

		// Token: 0x04000563 RID: 1379
		private readonly QueryData<float> _totalPowerRatio;

		// Token: 0x04000564 RID: 1380
		private readonly QueryData<float> _insideWallsRatio;

		// Token: 0x04000565 RID: 1381
		private IBattlePowerCalculationLogic _battlePowerLogic;

		// Token: 0x04000566 RID: 1382
		private CasualtyHandler _casualtyHandler;

		// Token: 0x04000567 RID: 1383
		private readonly QueryData<float> _maxUnderRangedAttackRatio;
	}
}
