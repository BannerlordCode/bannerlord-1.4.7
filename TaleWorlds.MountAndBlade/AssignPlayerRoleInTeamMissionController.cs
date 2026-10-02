using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.LinQuick;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000272 RID: 626
	public class AssignPlayerRoleInTeamMissionController : MissionLogic
	{
		// Token: 0x14000034 RID: 52
		// (add) Token: 0x06002302 RID: 8962 RVA: 0x0007BF40 File Offset: 0x0007A140
		// (remove) Token: 0x06002303 RID: 8963 RVA: 0x0007BF78 File Offset: 0x0007A178
		public event PlayerTurnToChooseFormationToLeadEvent OnPlayerTurnToChooseFormationToLead;

		// Token: 0x14000035 RID: 53
		// (add) Token: 0x06002304 RID: 8964 RVA: 0x0007BFB0 File Offset: 0x0007A1B0
		// (remove) Token: 0x06002305 RID: 8965 RVA: 0x0007BFE8 File Offset: 0x0007A1E8
		public event AllFormationsAssignedSergeantsEvent OnAllFormationsAssignedSergeants;

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06002306 RID: 8966 RVA: 0x0007C01D File Offset: 0x0007A21D
		public bool IsPlayerInArmy { get; }

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06002307 RID: 8967 RVA: 0x0007C025 File Offset: 0x0007A225
		public bool IsPlayerGeneral { get; }

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06002308 RID: 8968 RVA: 0x0007C02D File Offset: 0x0007A22D
		public bool IsPlayerSergeant { get; }

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06002309 RID: 8969 RVA: 0x0007C035 File Offset: 0x0007A235
		// (set) Token: 0x0600230A RID: 8970 RVA: 0x0007C03D File Offset: 0x0007A23D
		public int PlayerChosenIndex { get; protected set; }

		// Token: 0x0600230B RID: 8971 RVA: 0x0007C046 File Offset: 0x0007A246
		public AssignPlayerRoleInTeamMissionController(bool isPlayerGeneral, bool isPlayerSergeant, bool isPlayerInArmy, List<string> charactersInPlayerSideByPriority = null)
		{
			this.IsPlayerGeneral = isPlayerGeneral;
			this.IsPlayerSergeant = isPlayerSergeant;
			this.IsPlayerInArmy = isPlayerInArmy;
			this.PlayerChosenIndex = -1;
			this.CharactersInPlayerSideByPriority = charactersInPlayerSideByPriority;
		}

		// Token: 0x0600230C RID: 8972 RVA: 0x0007C072 File Offset: 0x0007A272
		public override void AfterStart()
		{
			Mission.Current.PlayerTeam.SetPlayerRole(this.IsPlayerGeneral, this.IsPlayerSergeant);
		}

		// Token: 0x0600230D RID: 8973 RVA: 0x0007C090 File Offset: 0x0007A290
		public override void OnTeamDeployed(Team team)
		{
			base.OnTeamDeployed(team);
			if (team == base.Mission.PlayerTeam)
			{
				team.PlayerOrderController.Owner = base.Mission.InitialPlayerAgent;
				if (team.IsPlayerGeneral)
				{
					foreach (Formation formation in team.FormationsIncludingEmpty)
					{
						formation.PlayerOwner = base.Mission.InitialPlayerAgent;
					}
				}
				team.PlayerOrderController.SelectAllFormations(false);
			}
		}

		// Token: 0x0600230E RID: 8974 RVA: 0x0007C12C File Offset: 0x0007A32C
		public virtual void OnPlayerTeamDeployed()
		{
			if (MissionGameModels.Current.BattleInitializationModel.CanPlayerSideDeployWithOrderOfBattle())
			{
				Team playerTeam = Mission.Current.PlayerTeam;
				this.FormationsLockedWithSergeants = new Dictionary<int, Agent>();
				this.FormationsWithLooselyChosenSergeants = new Dictionary<int, Agent>();
				if (playerTeam.IsPlayerGeneral)
				{
					this.CharacterNamesInPlayerSideByPriorityQueue = new Queue<string>();
					this.RemainingFormationsToAssignSergeantsTo = new List<Formation>();
				}
				else
				{
					this.CharacterNamesInPlayerSideByPriorityQueue = ((this.CharactersInPlayerSideByPriority != null) ? new Queue<string>(this.CharactersInPlayerSideByPriority) : new Queue<string>());
					this.RemainingFormationsToAssignSergeantsTo = playerTeam.FormationsIncludingSpecialAndEmpty.WhereQ<Formation>((Formation f) => f.CountOfUnits > 0).ToList<Formation>();
					while (this.RemainingFormationsToAssignSergeantsTo.Count > 0 && this.CharacterNamesInPlayerSideByPriorityQueue.Count > 0)
					{
						string nextAgentNameToProcess = this.CharacterNamesInPlayerSideByPriorityQueue.Dequeue();
						Agent agent = playerTeam.ActiveAgents.FirstOrDefault<Agent>((Agent aa) => aa.Character.StringId.Equals(nextAgentNameToProcess));
						if (agent != null)
						{
							if (agent == base.Mission.InitialPlayerAgent)
							{
								break;
							}
							Formation formation = this.ChooseFormationToLead(this.RemainingFormationsToAssignSergeantsTo, agent);
							if (formation != null)
							{
								this.FormationsLockedWithSergeants.Add(formation.Index, agent);
								this.RemainingFormationsToAssignSergeantsTo.Remove(formation);
							}
						}
					}
				}
				PlayerTurnToChooseFormationToLeadEvent onPlayerTurnToChooseFormationToLead = this.OnPlayerTurnToChooseFormationToLead;
				if (onPlayerTurnToChooseFormationToLead == null)
				{
					return;
				}
				onPlayerTurnToChooseFormationToLead(this.FormationsLockedWithSergeants, this.RemainingFormationsToAssignSergeantsTo.Select<Formation, int>((Formation ftcsf) => ftcsf.Index).ToList<int>());
			}
		}

		// Token: 0x0600230F RID: 8975 RVA: 0x0007C2C4 File Offset: 0x0007A4C4
		public virtual void OnPlayerChoiceMade(int chosenIndex)
		{
			if (this.PlayerChosenIndex != chosenIndex)
			{
				this.PlayerChosenIndex = chosenIndex;
				this.FormationsWithLooselyChosenSergeants.Clear();
				List<Formation> list = base.Mission.PlayerTeam.FormationsIncludingEmpty.WhereQ<Formation>((Formation f) => f.CountOfUnits > 0 && !this.FormationsLockedWithSergeants.ContainsKey(f.Index)).ToList<Formation>();
				if (chosenIndex != -1)
				{
					Formation formation = list.FirstOrDefault<Formation>((Formation fr) => fr.Index == chosenIndex);
					this.FormationsWithLooselyChosenSergeants.Add(chosenIndex, base.Mission.PlayerTeam.PlayerOrderController.Owner);
					list.Remove(formation);
				}
				Queue<string> queue = new Queue<string>(this.CharacterNamesInPlayerSideByPriorityQueue);
				while (list.Count > 0 && queue.Count > 0)
				{
					string nextAgentNameToProcess = queue.Dequeue();
					Agent agent = base.Mission.PlayerTeam.ActiveAgents.FirstOrDefault<Agent>((Agent aa) => aa.Character.StringId.Equals(nextAgentNameToProcess));
					if (agent != null)
					{
						Formation formation2 = this.ChooseFormationToLead(list, agent);
						if (formation2 != null)
						{
							this.FormationsWithLooselyChosenSergeants.Add(formation2.Index, agent);
							list.Remove(formation2);
						}
					}
				}
				if (this.OnAllFormationsAssignedSergeants != null)
				{
					this.OnAllFormationsAssignedSergeants(this.FormationsWithLooselyChosenSergeants);
				}
			}
		}

		// Token: 0x06002310 RID: 8976 RVA: 0x0007C420 File Offset: 0x0007A620
		public void OnPlayerChoiceFinalized()
		{
			foreach (KeyValuePair<int, Agent> keyValuePair in this.FormationsLockedWithSergeants)
			{
				this.AssignSergeant(keyValuePair.Value.Team.GetFormation((FormationClass)keyValuePair.Key), keyValuePair.Value);
			}
			foreach (KeyValuePair<int, Agent> keyValuePair2 in this.FormationsWithLooselyChosenSergeants)
			{
				this.AssignSergeant(keyValuePair2.Value.Team.GetFormation((FormationClass)keyValuePair2.Key), keyValuePair2.Value);
			}
		}

		// Token: 0x06002311 RID: 8977 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		protected virtual void AssignSergeant(Formation formationToLead, Agent sergeant)
		{
			sergeant.Formation = formationToLead;
			if (!sergeant.IsAIControlled || sergeant == base.Mission.InitialPlayerAgent)
			{
				formationToLead.PlayerOwner = sergeant;
			}
			formationToLead.Captain = sergeant;
		}

		// Token: 0x06002312 RID: 8978 RVA: 0x0007C524 File Offset: 0x0007A724
		private Formation ChooseFormationToLead(IEnumerable<Formation> formationsToChooseFrom, Agent agent)
		{
			bool hasMount = agent.HasMount;
			bool flag = agent.HasRangedWeapon(false);
			List<Formation> list = formationsToChooseFrom.ToList<Formation>();
			while (list.Count > 0)
			{
				Formation formation = list.MaxBy<Formation, float>((Formation ftcf) => ftcf.QuerySystem.FormationPower);
				list.Remove(formation);
				if ((flag || (!formation.QuerySystem.IsRangedFormation && !formation.QuerySystem.IsRangedCavalryFormation)) && (hasMount || (!formation.QuerySystem.IsCavalryFormation && !formation.QuerySystem.IsRangedCavalryFormation)))
				{
					return formation;
				}
			}
			return null;
		}

		// Token: 0x04000D6F RID: 3439
		protected readonly List<string> CharactersInPlayerSideByPriority;

		// Token: 0x04000D70 RID: 3440
		protected Queue<string> CharacterNamesInPlayerSideByPriorityQueue;

		// Token: 0x04000D71 RID: 3441
		protected List<Formation> RemainingFormationsToAssignSergeantsTo;

		// Token: 0x04000D72 RID: 3442
		protected Dictionary<int, Agent> FormationsLockedWithSergeants;

		// Token: 0x04000D73 RID: 3443
		protected Dictionary<int, Agent> FormationsWithLooselyChosenSergeants;
	}
}
