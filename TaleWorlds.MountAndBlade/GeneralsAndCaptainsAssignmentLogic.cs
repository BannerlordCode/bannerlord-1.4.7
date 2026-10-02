using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000284 RID: 644
	public class GeneralsAndCaptainsAssignmentLogic : MissionLogic
	{
		// Token: 0x060023E6 RID: 9190 RVA: 0x00080D75 File Offset: 0x0007EF75
		public GeneralsAndCaptainsAssignmentLogic(TextObject attackerGeneralName, TextObject defenderGeneralName, TextObject attackerAllyGeneralName = null, TextObject defenderAllyGeneralName = null, bool createBodyguard = true)
		{
			this._attackerGeneralName = attackerGeneralName;
			this._defenderGeneralName = defenderGeneralName;
			this._attackerAllyGeneralName = attackerAllyGeneralName;
			this._defenderAllyGeneralName = defenderAllyGeneralName;
			this._createBodyguard = createBodyguard;
			this._isPlayerTeamGeneralFormationSet = false;
		}

		// Token: 0x060023E7 RID: 9191 RVA: 0x00080DB0 File Offset: 0x0007EFB0
		public override void AfterStart()
		{
			this._bannerLogic = base.Mission.GetMissionBehavior<BannerBearerLogic>();
		}

		// Token: 0x060023E8 RID: 9192 RVA: 0x00080DC4 File Offset: 0x0007EFC4
		public override void OnTeamDeployed(Team team)
		{
			this.SetGeneralAgentOfTeam(team);
			if (team.IsPlayerTeam)
			{
				if (!MissionGameModels.Current.BattleInitializationModel.CanPlayerSideDeployWithOrderOfBattle())
				{
					if (this.CanTeamHaveGeneralsFormation(team))
					{
						this.CreateGeneralFormationForTeam(team);
						this._isPlayerTeamGeneralFormationSet = true;
					}
					this.AssignBestCaptainsForTeam(team);
					return;
				}
			}
			else
			{
				if (this.CanTeamHaveGeneralsFormation(team))
				{
					this.CreateGeneralFormationForTeam(team);
				}
				this.AssignBestCaptainsForTeam(team);
			}
		}

		// Token: 0x060023E9 RID: 9193 RVA: 0x00080E2C File Offset: 0x0007F02C
		public override void OnDeploymentFinished()
		{
			Team playerTeam = base.Mission.PlayerTeam;
			if (!this._isPlayerTeamGeneralFormationSet && this.CanTeamHaveGeneralsFormation(playerTeam))
			{
				this.CreateGeneralFormationForTeam(playerTeam);
				this._isPlayerTeamGeneralFormationSet = true;
			}
			Agent initialPlayerAgent = base.Mission.InitialPlayerAgent;
			if (this._isPlayerTeamGeneralFormationSet && playerTeam.GeneralAgent != initialPlayerAgent && !base.Mission.IsNavalBattle)
			{
				initialPlayerAgent.SetCanLeadFormationsRemotely(true);
				Formation formation = playerTeam.GetFormation(FormationClass.NumberOfRegularFormations);
				initialPlayerAgent.Formation = formation;
				initialPlayerAgent.Team.TriggerOnFormationsChanged(formation);
				formation.QuerySystem.Expire();
			}
		}

		// Token: 0x060023EA RID: 9194 RVA: 0x00080EBC File Offset: 0x0007F0BC
		protected virtual void SortCaptainsByPriority(Team team, ref List<Agent> captains)
		{
			captains = captains.OrderByDescending<Agent, float>(delegate(Agent captain)
			{
				if (team.GeneralAgent != captain)
				{
					return captain.Character.GetPower();
				}
				return float.MaxValue;
			}).ToList<Agent>();
		}

		// Token: 0x060023EB RID: 9195 RVA: 0x00080EF0 File Offset: 0x0007F0F0
		protected virtual Formation PickBestRegularFormationToLead(Agent agent, List<Formation> candidateFormations)
		{
			Formation formation = null;
			int num = 0;
			foreach (Formation formation2 in candidateFormations)
			{
				if (!(agent.HasMount ^ formation2.CalculateHasSignificantNumberOfMounted))
				{
					int countOfUnits = formation2.CountOfUnits;
					if (countOfUnits > num)
					{
						num = countOfUnits;
						formation = formation2;
					}
				}
			}
			return formation;
		}

		// Token: 0x060023EC RID: 9196 RVA: 0x00080F60 File Offset: 0x0007F160
		private bool CanTeamHaveGeneralsFormation(Team team)
		{
			if (base.Mission.IsNavalBattle)
			{
				return false;
			}
			Agent generalAgent = team.GeneralAgent;
			return generalAgent != null && (generalAgent == base.Mission.InitialPlayerAgent || team.QuerySystem.MemberCount >= 50);
		}

		// Token: 0x060023ED RID: 9197 RVA: 0x00080FAC File Offset: 0x0007F1AC
		private void AssignBestCaptainsForTeam(Team team)
		{
			List<Agent> list = team.ActiveAgents.Where<Agent>((Agent agent) => agent.IsHero).ToList<Agent>();
			this.SortCaptainsByPriority(team, ref list);
			int numRegularFormations = 8;
			List<Formation> list2 = team.FormationsIncludingEmpty.WhereQ<Formation>((Formation f) => f.CountOfUnits > 0 && f.FormationIndex < (FormationClass)numRegularFormations).ToList<Formation>();
			List<Agent> list3 = new List<Agent>();
			foreach (Agent agent3 in list)
			{
				Formation formation = null;
				BattleBannerBearersModel battleBannerBearersModel = MissionGameModels.Current.BattleBannerBearersModel;
				if (agent3 == team.GeneralAgent && team.BodyGuardFormation != null && team.BodyGuardFormation.CountOfUnits > 0)
				{
					formation = team.BodyGuardFormation;
				}
				if (formation == null)
				{
					formation = this.PickBestRegularFormationToLead(agent3, list2);
					if (formation != null)
					{
						list2.Remove(formation);
					}
				}
				if (formation != null)
				{
					list3.Add(agent3);
					this.OnCaptainAssignedToFormation(agent3, formation);
				}
			}
			foreach (Agent agent2 in list3)
			{
				list.Remove(agent2);
			}
			using (List<Agent>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Agent candidate = enumerator.Current;
					if (list2.IsEmpty<Formation>())
					{
						break;
					}
					Formation formation2 = list2.FirstOrDefault<Formation>((Formation f) => f.CalculateHasSignificantNumberOfMounted == candidate.HasMount);
					if (formation2 != null)
					{
						this.OnCaptainAssignedToFormation(candidate, formation2);
						list2.Remove(formation2);
					}
				}
			}
		}

		// Token: 0x060023EE RID: 9198 RVA: 0x00081188 File Offset: 0x0007F388
		private void SetGeneralAgentOfTeam(Team team)
		{
			Agent agent = null;
			if (team.IsPlayerTeam && team.IsPlayerGeneral)
			{
				agent = base.Mission.InitialPlayerAgent;
			}
			else
			{
				List<IFormationUnit> list = team.FormationsIncludingEmpty.SelectMany<Formation, IFormationUnit>((Formation f) => f.UnitsWithoutLooseDetachedOnes).ToList<IFormationUnit>();
				TextObject generalName = ((team == base.Mission.AttackerTeam) ? this._attackerGeneralName : ((team == base.Mission.DefenderTeam) ? this._defenderGeneralName : ((team == base.Mission.AttackerAllyTeam) ? this._attackerAllyGeneralName : ((team == base.Mission.DefenderAllyTeam) ? this._defenderAllyGeneralName : null))));
				if (generalName != null && list.Count<IFormationUnit>((IFormationUnit ta) => ((Agent)ta).Character != null && ((Agent)ta).Character.GetName().Equals(generalName)) == 1)
				{
					agent = (Agent)list.First<IFormationUnit>((IFormationUnit ta) => ((Agent)ta).Character != null && ((Agent)ta).Character.GetName().Equals(generalName));
				}
				else if (list.Any<IFormationUnit>((IFormationUnit u) => !((Agent)u).IsMainAgent && ((Agent)u).IsHero))
				{
					agent = (Agent)list.Where<IFormationUnit>((IFormationUnit u) => !((Agent)u).IsMainAgent && ((Agent)u).IsHero).MaxBy<IFormationUnit, float>((IFormationUnit u) => ((Agent)u).CharacterPowerCached);
				}
			}
			if (agent != null && !base.Mission.IsNavalBattle)
			{
				agent.SetCanLeadFormationsRemotely(true);
			}
			team.GeneralAgent = agent;
		}

		// Token: 0x060023EF RID: 9199 RVA: 0x00081320 File Offset: 0x0007F520
		private void CreateGeneralFormationForTeam(Team team)
		{
			Agent generalAgent = team.GeneralAgent;
			Formation formation = team.GetFormation(FormationClass.NumberOfRegularFormations);
			base.Mission.SetFormationPositioningFromDeploymentPlan(formation);
			WorldPosition worldPosition = formation.CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache.GroundVec3);
			formation.SetMovementOrder(MovementOrder.MovementOrderMove(worldPosition));
			formation.SetControlledByAI(true, false);
			team.GeneralsFormation = formation;
			generalAgent.Formation = formation;
			generalAgent.Team.TriggerOnFormationsChanged(formation);
			formation.QuerySystem.Expire();
			TacticComponent.SetDefaultBehaviorWeights(formation);
			formation.AI.SetBehaviorWeight<BehaviorGeneral>(1f);
			formation.PlayerOwner = null;
			if (this._createBodyguard && generalAgent != base.Mission.InitialPlayerAgent)
			{
				List<IFormationUnit> list = team.FormationsIncludingEmpty.SelectMany<Formation, IFormationUnit>((Formation f) => f.UnitsWithoutLooseDetachedOnes).ToList<IFormationUnit>();
				list.Remove(generalAgent);
				List<IFormationUnit> list2 = list.Where<IFormationUnit>(delegate(IFormationUnit u)
				{
					Agent agent;
					if ((agent = u as Agent) == null || (agent.Character != null && agent.Character.IsHero) || agent.Banner != null)
					{
						return false;
					}
					if (generalAgent.MountAgent == null)
					{
						return !agent.HasMount;
					}
					return agent.HasMount;
				}).ToList<IFormationUnit>();
				int num = MathF.Min((int)((float)list2.Count / 10f), 20);
				if (num != 0)
				{
					Formation formation2 = team.GetFormation(FormationClass.Bodyguard);
					formation2.SetMovementOrder(MovementOrder.MovementOrderMove(worldPosition));
					formation2.SetControlledByAI(true, false);
					List<IFormationUnit> list3 = list2.OrderByDescending<IFormationUnit, float>((IFormationUnit u) => ((Agent)u).CharacterPowerCached).Take<IFormationUnit>(num).ToList<IFormationUnit>();
					IEnumerable<Formation> enumerable = list3.Select<IFormationUnit, Formation>((IFormationUnit bu) => ((Agent)bu).Formation).Distinct<Formation>();
					foreach (IFormationUnit formationUnit in list3)
					{
						((Agent)formationUnit).Formation = formation2;
					}
					foreach (Formation formation3 in enumerable)
					{
						team.TriggerOnFormationsChanged(formation3);
						formation3.QuerySystem.Expire();
					}
					TacticComponent.SetDefaultBehaviorWeights(formation2);
					formation2.AI.SetBehaviorWeight<BehaviorProtectGeneral>(1f);
					formation2.PlayerOwner = null;
					formation2.QuerySystem.Expire();
					team.BodyGuardFormation = formation2;
					team.TriggerOnFormationsChanged(formation2);
				}
			}
		}

		// Token: 0x060023F0 RID: 9200 RVA: 0x00081598 File Offset: 0x0007F798
		private void OnCaptainAssignedToFormation(Agent captain, Formation formation)
		{
			if (captain.Formation != formation && captain != formation.Team.GeneralAgent)
			{
				captain.Formation = formation;
				formation.Team.TriggerOnFormationsChanged(formation);
				formation.QuerySystem.Expire();
			}
			formation.Captain = captain;
			if (this._bannerLogic != null && captain.FormationBanner != null)
			{
				this._bannerLogic.SetFormationBanner(formation, captain.FormationBanner);
			}
		}

		// Token: 0x04000DC4 RID: 3524
		public int MinimumAgentCountToLeadGeneralFormation = 3;

		// Token: 0x04000DC5 RID: 3525
		private BannerBearerLogic _bannerLogic;

		// Token: 0x04000DC6 RID: 3526
		private readonly TextObject _attackerGeneralName;

		// Token: 0x04000DC7 RID: 3527
		private readonly TextObject _defenderGeneralName;

		// Token: 0x04000DC8 RID: 3528
		private readonly TextObject _attackerAllyGeneralName;

		// Token: 0x04000DC9 RID: 3529
		private readonly TextObject _defenderAllyGeneralName;

		// Token: 0x04000DCA RID: 3530
		private readonly bool _createBodyguard;

		// Token: 0x04000DCB RID: 3531
		private bool _isPlayerTeamGeneralFormationSet;
	}
}
