using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200029F RID: 671
	public static class MissionReinforcementsHelper
	{
		// Token: 0x06002513 RID: 9491 RVA: 0x00086B68 File Offset: 0x00084D68
		public static void OnMissionStart()
		{
			Mission mission = Mission.Current;
			MissionReinforcementsHelper._reinforcementFormationsData = new MissionReinforcementsHelper.ReinforcementFormationData[mission.Teams.Count, 8];
			foreach (Team team in mission.Teams)
			{
				for (int i = 0; i < 8; i++)
				{
					MissionReinforcementsHelper._reinforcementFormationsData[team.TeamIndex, i] = new MissionReinforcementsHelper.ReinforcementFormationData();
				}
			}
			MissionReinforcementsHelper._localInitTime = 0U;
		}

		// Token: 0x06002514 RID: 9492 RVA: 0x00086BF8 File Offset: 0x00084DF8
		[return: TupleElementNames(new string[] { "origin", "formationIndex" })]
		public unsafe static List<ValueTuple<IAgentOriginBase, int>> GetReinforcementAssignments(BattleSideEnum battleSide, List<IAgentOriginBase> troopOrigins)
		{
			Mission mission = Mission.Current;
			MissionReinforcementsHelper._localInitTime += 1U;
			List<ValueTuple<IAgentOriginBase, int>> list = new List<ValueTuple<IAgentOriginBase, int>>();
			PriorityQueue<MissionReinforcementsHelper.ReinforcementFormationPriority, Formation> priorityQueue = new PriorityQueue<MissionReinforcementsHelper.ReinforcementFormationPriority, Formation>(new MissionReinforcementsHelper.ReinforcementFormationPreferenceComparer());
			foreach (IAgentOriginBase agentOriginBase in troopOrigins)
			{
				priorityQueue.Clear();
				FormationClass agentTroopClass = Mission.Current.GetAgentTroopClass(battleSide, agentOriginBase.Troop);
				bool flag = Mission.Current.PlayerTeam.Side == battleSide;
				Team agentTeam = Mission.GetAgentTeam(agentOriginBase, flag);
				foreach (Formation formation in agentTeam.FormationsIncludingEmpty)
				{
					int formationIndex = (int)formation.FormationIndex;
					if (formation.GetReadonlyMovementOrderReference()->OrderEnum != MovementOrder.MovementOrderEnum.Retreat)
					{
						MissionReinforcementsHelper.ReinforcementFormationData reinforcementFormationData = MissionReinforcementsHelper._reinforcementFormationsData[agentTeam.TeamIndex, formationIndex];
						if (!reinforcementFormationData.IsInitialized(MissionReinforcementsHelper._localInitTime))
						{
							reinforcementFormationData.Initialize(formation, MissionReinforcementsHelper._localInitTime);
						}
						MissionReinforcementsHelper.ReinforcementFormationPriority priority = reinforcementFormationData.GetPriority(agentTroopClass);
						if (priorityQueue.IsEmpty<KeyValuePair<MissionReinforcementsHelper.ReinforcementFormationPriority, Formation>>() || priority >= priorityQueue.Peek().Key)
						{
							priorityQueue.Enqueue(priority, formation);
						}
					}
				}
				Formation formation2 = MissionReinforcementsHelper.FindBestFormationAmong(priorityQueue);
				if (formation2 == null)
				{
					formation2 = agentTeam.GetFormation(agentTroopClass);
				}
				int formationIndex2 = (int)formation2.FormationIndex;
				MissionReinforcementsHelper._reinforcementFormationsData[formation2.Team.TeamIndex, formationIndex2].AddProspectiveTroop(agentTroopClass);
				ValueTuple<IAgentOriginBase, int> valueTuple = new ValueTuple<IAgentOriginBase, int>(agentOriginBase, formationIndex2);
				list.Add(valueTuple);
			}
			return list;
		}

		// Token: 0x06002515 RID: 9493 RVA: 0x00086DCC File Offset: 0x00084FCC
		public static void OnMissionEnd()
		{
			MissionReinforcementsHelper._reinforcementFormationsData = null;
		}

		// Token: 0x06002516 RID: 9494 RVA: 0x00086DD4 File Offset: 0x00084FD4
		private static Formation FindBestFormationAmong(PriorityQueue<MissionReinforcementsHelper.ReinforcementFormationPriority, Formation> matchingFormations)
		{
			Formation formation = null;
			float num = float.MinValue;
			if (!matchingFormations.IsEmpty<KeyValuePair<MissionReinforcementsHelper.ReinforcementFormationPriority, Formation>>())
			{
				int key = (int)matchingFormations.Peek().Key;
				foreach (KeyValuePair<MissionReinforcementsHelper.ReinforcementFormationPriority, Formation> keyValuePair in matchingFormations)
				{
					int key2 = (int)keyValuePair.Key;
					if (key2 < key)
					{
						break;
					}
					Formation value = keyValuePair.Value;
					if (key2 == 3 || key2 == 4)
					{
						if (formation == null || value.FormationIndex < formation.FormationIndex)
						{
							formation = value;
						}
					}
					else
					{
						float formationReinforcementScore = MissionReinforcementsHelper.GetFormationReinforcementScore(value);
						if (formationReinforcementScore > num)
						{
							num = formationReinforcementScore;
							formation = value;
						}
					}
				}
			}
			return formation;
		}

		// Token: 0x06002517 RID: 9495 RVA: 0x00086E8C File Offset: 0x0008508C
		private static float GetFormationReinforcementScore(Formation formation)
		{
			Mission mission = Mission.Current;
			float num = (float)formation.CountOfUnits / (float)Math.Max(1, formation.Team.ActiveAgents.Count);
			float num2 = MathF.Max(0f, 1f - num);
			float num3 = 0f;
			Team team = formation.Team;
			DefaultMissionDeploymentPlan defaultMissionDeploymentPlan;
			if (mission.GetDeploymentPlan<DefaultMissionDeploymentPlan>(out defaultMissionDeploymentPlan) && formation.HasBeenPositioned && defaultMissionDeploymentPlan.IsReinforcementPlanMade(team))
			{
				Vec2 asVec = defaultMissionDeploymentPlan.GetMeanPosition(team, false).AsVec2;
				float num4 = formation.CurrentPosition.DistanceSquared(asVec);
				float num5 = MathF.Min(1f, num4 / 62500f);
				num3 = MathF.Max(0f, 1f - num5);
			}
			return 0.6f * num2 + 0.4f * num3;
		}

		// Token: 0x04000E55 RID: 3669
		private const float DominantClassThreshold = 0.5f;

		// Token: 0x04000E56 RID: 3670
		private const float CommonClassThreshold = 0.25f;

		// Token: 0x04000E57 RID: 3671
		private static uint _localInitTime;

		// Token: 0x04000E58 RID: 3672
		private static MissionReinforcementsHelper.ReinforcementFormationData[,] _reinforcementFormationsData;

		// Token: 0x02000571 RID: 1393
		public enum ReinforcementFormationPriority
		{
			// Token: 0x04001E3D RID: 7741
			Dominant = 6,
			// Token: 0x04001E3E RID: 7742
			Common = 5,
			// Token: 0x04001E3F RID: 7743
			EmptyRepresentativeMatch = 4,
			// Token: 0x04001E40 RID: 7744
			EmptyNoMatch = 3,
			// Token: 0x04001E41 RID: 7745
			AlternativeDominant = 2,
			// Token: 0x04001E42 RID: 7746
			AlternativeCommon = 1,
			// Token: 0x04001E43 RID: 7747
			Default = 0
		}

		// Token: 0x02000572 RID: 1394
		public class ReinforcementFormationPreferenceComparer : IComparer<MissionReinforcementsHelper.ReinforcementFormationPriority>
		{
			// Token: 0x06003D29 RID: 15657 RVA: 0x000F2AC0 File Offset: 0x000F0CC0
			public int Compare(MissionReinforcementsHelper.ReinforcementFormationPriority left, MissionReinforcementsHelper.ReinforcementFormationPriority right)
			{
				if (right < left)
				{
					return 1;
				}
				if (right > left)
				{
					return -1;
				}
				return 0;
			}
		}

		// Token: 0x02000573 RID: 1395
		public class ReinforcementFormationData
		{
			// Token: 0x06003D2B RID: 15659 RVA: 0x000F2AE6 File Offset: 0x000F0CE6
			public ReinforcementFormationData()
			{
				this._initTime = 0U;
				this._expectedTroopCountPerClass = new int[4];
				this._expectedTotalTroopCount = 0;
				this._isClassified = false;
				this._representativeClass = FormationClass.NumberOfAllFormations;
				this._troopClasses = new bool[4];
			}

			// Token: 0x06003D2C RID: 15660 RVA: 0x000F2B24 File Offset: 0x000F0D24
			public void Initialize(Formation formation, uint initTime)
			{
				int countOfUnits = formation.CountOfUnits;
				this._expectedTroopCountPerClass[0] = (int)(formation.QuerySystem.InfantryUnitRatio * (float)countOfUnits);
				this._expectedTroopCountPerClass[1] = (int)(formation.QuerySystem.RangedUnitRatio * (float)countOfUnits);
				this._expectedTroopCountPerClass[2] = (int)(formation.QuerySystem.CavalryUnitRatio * (float)countOfUnits);
				this._expectedTroopCountPerClass[3] = (int)(formation.QuerySystem.RangedCavalryUnitRatio * (float)countOfUnits);
				this._expectedTotalTroopCount = countOfUnits;
				this._isClassified = false;
				this._representativeClass = formation.RepresentativeClass;
				this._initTime = initTime;
			}

			// Token: 0x06003D2D RID: 15661 RVA: 0x000F2BB8 File Offset: 0x000F0DB8
			public void AddProspectiveTroop(FormationClass troopClass)
			{
				this._expectedTroopCountPerClass[(int)troopClass]++;
				this._expectedTotalTroopCount++;
				this._isClassified = false;
			}

			// Token: 0x06003D2E RID: 15662 RVA: 0x000F2BED File Offset: 0x000F0DED
			public bool IsInitialized(uint initTime)
			{
				return initTime == this._initTime;
			}

			// Token: 0x06003D2F RID: 15663 RVA: 0x000F2BF8 File Offset: 0x000F0DF8
			public MissionReinforcementsHelper.ReinforcementFormationPriority GetPriority(FormationClass troopClass)
			{
				if (this._expectedTotalTroopCount == 0)
				{
					if (this._representativeClass == troopClass)
					{
						return MissionReinforcementsHelper.ReinforcementFormationPriority.EmptyRepresentativeMatch;
					}
					return MissionReinforcementsHelper.ReinforcementFormationPriority.EmptyNoMatch;
				}
				else
				{
					if (!this._isClassified)
					{
						this.Classify();
					}
					bool flag;
					if (this.HasTroopClass(troopClass, out flag))
					{
						if (!flag)
						{
							return MissionReinforcementsHelper.ReinforcementFormationPriority.Common;
						}
						return MissionReinforcementsHelper.ReinforcementFormationPriority.Dominant;
					}
					else
					{
						FormationClass formationClass = troopClass.AlternativeClass();
						if (!this.HasTroopClass(formationClass, out flag))
						{
							return MissionReinforcementsHelper.ReinforcementFormationPriority.Default;
						}
						if (!flag)
						{
							return MissionReinforcementsHelper.ReinforcementFormationPriority.AlternativeCommon;
						}
						return MissionReinforcementsHelper.ReinforcementFormationPriority.AlternativeDominant;
					}
				}
			}

			// Token: 0x06003D30 RID: 15664 RVA: 0x000F2C58 File Offset: 0x000F0E58
			private void Classify()
			{
				if (this._expectedTotalTroopCount > 0)
				{
					int num = -1;
					int num2 = 4;
					for (int i = 0; i < num2; i++)
					{
						float num3 = (float)this._expectedTroopCountPerClass[i] / (float)this._expectedTotalTroopCount;
						this._troopClasses[i] = num3 >= 0.25f;
						if (num3 > 0.5f)
						{
							num = i;
							break;
						}
					}
					if (num >= 0)
					{
						this.ResetClassAssignments();
						this._troopClasses[num] = true;
					}
				}
				else
				{
					this.ResetClassAssignments();
				}
				this._isClassified = true;
			}

			// Token: 0x06003D31 RID: 15665 RVA: 0x000F2CD4 File Offset: 0x000F0ED4
			private bool HasTroopClass(FormationClass troopClass, out bool isDominant)
			{
				int num = 0;
				for (int i = 0; i < 4; i++)
				{
					if (i == (int)troopClass && this._troopClasses[i])
					{
						num++;
					}
				}
				isDominant = num == 1;
				return num >= 1;
			}

			// Token: 0x06003D32 RID: 15666 RVA: 0x000F2D10 File Offset: 0x000F0F10
			private void ResetClassAssignments()
			{
				int num = 4;
				for (int i = 0; i < num; i++)
				{
					this._troopClasses[i] = false;
				}
			}

			// Token: 0x04001E44 RID: 7748
			private uint _initTime;

			// Token: 0x04001E45 RID: 7749
			private bool _isClassified;

			// Token: 0x04001E46 RID: 7750
			private int[] _expectedTroopCountPerClass;

			// Token: 0x04001E47 RID: 7751
			private int _expectedTotalTroopCount;

			// Token: 0x04001E48 RID: 7752
			private bool[] _troopClasses;

			// Token: 0x04001E49 RID: 7753
			private FormationClass _representativeClass;
		}
	}
}
