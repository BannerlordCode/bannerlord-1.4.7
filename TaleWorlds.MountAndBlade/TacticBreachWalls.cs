using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000166 RID: 358
	public class TacticBreachWalls : TacticComponent
	{
		// Token: 0x060012B2 RID: 4786 RVA: 0x0003BA04 File Offset: 0x00039C04
		public TacticBreachWalls(Team team)
			: base(team)
		{
			Mission mission = Mission.Current;
			this._teamAISiegeAttacker = team.TeamAI as TeamAISiegeAttacker;
			this._meleeFormations = new List<Formation>();
			this._rangedFormations = new List<Formation>();
			this._cachedUsedSiegeLanes = new List<SiegeLane>();
			this._cachedUsedArcherPositions = new List<ArcherPosition>();
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x0003BA5C File Offset: 0x00039C5C
		private void BalanceAssaultLanes(List<Formation> attackerFormations)
		{
			if (attackerFormations.Count < 2)
			{
				return;
			}
			int num = attackerFormations.Sum<Formation>((Formation f) => f.CountOfUnitsWithoutDetachedOnes);
			int idealCount = num / attackerFormations.Count;
			int num2 = MathF.Max((int)((float)num * 0.2f), 1);
			Func<Formation, int> <>9__2;
			Func<Formation, bool> <>9__1;
			foreach (Formation formation in attackerFormations)
			{
				int num3 = 0;
				while (idealCount - formation.CountOfUnitsWithoutDetachedOnes > num2)
				{
					Func<Formation, bool> func;
					if ((func = <>9__1) == null)
					{
						func = (<>9__1 = (Formation af) => af.CountOfUnitsWithoutDetachedOnes > idealCount);
					}
					if (!attackerFormations.Any<Formation>(func) || num3 >= attackerFormations.Count)
					{
						break;
					}
					int num4 = idealCount - formation.CountOfUnitsWithoutDetachedOnes;
					Func<Formation, int> func2;
					if ((func2 = <>9__2) == null)
					{
						func2 = (<>9__2 = (Formation df) => df.CountOfUnitsWithoutDetachedOnes - idealCount);
					}
					Formation formation2 = attackerFormations.MaxBy<Formation, int>(func2);
					num4 = MathF.Min(num4, formation2.CountOfUnitsWithoutDetachedOnes - idealCount);
					formation2.TransferUnits(formation, num4);
					num3++;
				}
			}
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x0003BBAC File Offset: 0x00039DAC
		private bool ShouldRetreat(List<SiegeLane> lanes, int insideFormationCount)
		{
			if (this._indicators != null)
			{
				float num = base.Team.QuerySystem.RemainingPowerRatio / this._indicators.StartingPowerRatio;
				float retreatThresholdRatio = this._indicators.GetRetreatThresholdRatio(lanes, insideFormationCount);
				return num < retreatThresholdRatio;
			}
			return false;
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x0003BBF0 File Offset: 0x00039DF0
		private void AssignMeleeFormationsToLanes(List<Formation> meleeFormationsSource, List<SiegeLane> currentLanes)
		{
			List<Formation> list = new List<Formation>(meleeFormationsSource.Count);
			list.AddRange(meleeFormationsSource);
			List<SiegeLane> list2 = currentLanes.ToList<SiegeLane>();
			for (int i = 0; i < currentLanes.Count; i++)
			{
				SiegeLane siegeLane = currentLanes[i];
				Formation lastAssignedFormation = currentLanes[i].GetLastAssignedFormation(base.Team.TeamIndex);
				if (lastAssignedFormation != null && list.Contains(lastAssignedFormation))
				{
					lastAssignedFormation.AI.Side = siegeLane.LaneSide;
					lastAssignedFormation.AI.ResetBehaviorWeights();
					TacticComponent.SetDefaultBehaviorWeights(lastAssignedFormation);
					lastAssignedFormation.AI.SetBehaviorWeight<BehaviorAssaultWalls>(1f);
					lastAssignedFormation.AI.SetBehaviorWeight<BehaviorUseSiegeMachines>(1f);
					lastAssignedFormation.AI.SetBehaviorWeight<BehaviorWaitForLadders>(1f);
					list2.Remove(siegeLane);
					list.Remove(lastAssignedFormation);
				}
			}
			while (list.Count > 0 && list2.Count > 0)
			{
				Formation largestFormation = list.MaxBy<Formation, int>((Formation mf) => mf.CountOfUnitsWithoutLooseDetachedOnes);
				SiegeLane siegeLane2 = list2.MinBy<SiegeLane, float>(delegate(SiegeLane l)
				{
					WorldPosition currentAttackerPosition = l.GetCurrentAttackerPosition();
					Vec3 navMeshVec = largestFormation.CachedMedianPosition.GetNavMeshVec3();
					return currentAttackerPosition.DistanceSquaredWithLimit(in navMeshVec, 10000f);
				});
				largestFormation.AI.Side = siegeLane2.LaneSide;
				largestFormation.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(largestFormation);
				largestFormation.AI.SetBehaviorWeight<BehaviorAssaultWalls>(1f);
				largestFormation.AI.SetBehaviorWeight<BehaviorUseSiegeMachines>(1f);
				largestFormation.AI.SetBehaviorWeight<BehaviorWaitForLadders>(1f);
				siegeLane2.SetLastAssignedFormation(base.Team.TeamIndex, largestFormation);
				list.Remove(largestFormation);
				list2.Remove(siegeLane2);
			}
			bool flag = true;
			while (list.Count > 0)
			{
				if (list2.IsEmpty<SiegeLane>())
				{
					list2.AddRange(currentLanes);
					flag = false;
				}
				Formation nextBiggest = list.MaxBy<Formation, int>((Formation mf) => mf.CountOfUnitsWithoutLooseDetachedOnes);
				SiegeLane siegeLane3 = list2.MinBy<SiegeLane, float>(delegate(SiegeLane l)
				{
					WorldPosition currentAttackerPosition2 = l.GetCurrentAttackerPosition();
					Vec3 navMeshVec2 = nextBiggest.CachedMedianPosition.GetNavMeshVec3();
					return currentAttackerPosition2.DistanceSquaredWithLimit(in navMeshVec2, 10000f);
				});
				nextBiggest.AI.Side = siegeLane3.LaneSide;
				nextBiggest.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(nextBiggest);
				nextBiggest.AI.SetBehaviorWeight<BehaviorAssaultWalls>(1f);
				nextBiggest.AI.SetBehaviorWeight<BehaviorUseSiegeMachines>(1f);
				nextBiggest.AI.SetBehaviorWeight<BehaviorWaitForLadders>(1f);
				if (flag)
				{
					siegeLane3.SetLastAssignedFormation(base.Team.TeamIndex, nextBiggest);
				}
				list.Remove(nextBiggest);
				list2.Remove(siegeLane3);
			}
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x0003BEF4 File Offset: 0x0003A0F4
		private void WellRoundedAssault(ref List<SiegeLane> currentLanes, ref List<ArcherPosition> archerPositions)
		{
			if (currentLanes.Count == 0)
			{
				Debug.Print("TeamAISiegeComponent.SiegeLanes.Count" + TeamAISiegeComponent.SiegeLanes.Count, 0, Debug.DebugColor.White, 17592186044416UL);
				for (int i = 0; i < TeamAISiegeComponent.SiegeLanes.Count; i++)
				{
					SiegeLane siegeLane = TeamAISiegeComponent.SiegeLanes[i];
					Debug.Print(string.Concat(new object[]
					{
						"lane ",
						i,
						" is breach ",
						siegeLane.IsBreach.ToString(),
						" is unusable ",
						siegeLane.CalculateIsLaneUnusable().ToString(),
						" has gate ",
						siegeLane.HasGate.ToString()
					}), 0, Debug.DebugColor.White, 17592186044416UL);
				}
				Debug.Print("_teamAISiegeAttacker.PrimarySiegeWeapons.Count " + this._teamAISiegeAttacker.PrimarySiegeWeapons.Count, 0, Debug.DebugColor.White, 17592186044416UL);
				List<SiegeLadder> list = Mission.Current.ActiveMissionObjects.FindAllWithType<SiegeLadder>().ToList<SiegeLadder>();
				Debug.Print("ladders.Count = " + list.Count, 0, Debug.DebugColor.White, 17592186044416UL);
				List<SiegeTower> list2 = Mission.Current.ActiveMissionObjects.FindAllWithType<SiegeTower>().ToList<SiegeTower>();
				Debug.Print("towers.Count = " + list2.Count, 0, Debug.DebugColor.White, 17592186044416UL);
				BatteringRam batteringRam = Mission.Current.ActiveMissionObjects.FindAllWithType<BatteringRam>().FirstOrDefault<BatteringRam>();
				Debug.Print("ram = " + batteringRam, 0, Debug.DebugColor.White, 17592186044416UL);
			}
			this.AssignMeleeFormationsToLanes(this._meleeFormations, currentLanes);
			foreach (Formation formation in this._rangedFormations)
			{
				formation.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(formation);
				formation.AI.SetBehaviorWeight<BehaviorSkirmish>(1f);
			}
			if (archerPositions.Count > 0)
			{
				using (IEnumerator<Formation> enumerator2 = this._rangedFormations.OrderByDescending<Formation, int>((Formation rf) => rf.CountOfUnits).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Formation rangedFormation = enumerator2.Current;
						if (archerPositions.IsEmpty<ArcherPosition>())
						{
							archerPositions.AddRange(this._teamAISiegeAttacker.ArcherPositions);
						}
						ArcherPosition archerPosition = null;
						if (rangedFormation.AI.ActiveBehavior is BehaviorSparseSkirmish)
						{
							archerPosition = archerPositions.FirstOrDefault<ArcherPosition>((ArcherPosition ap) => ap.Entity == (rangedFormation.AI.ActiveBehavior as BehaviorSparseSkirmish).ArcherPosition);
						}
						if (archerPosition != null)
						{
							rangedFormation.AI.SetBehaviorWeight<BehaviorSparseSkirmish>(1f);
							archerPositions.Remove(archerPosition);
						}
						else
						{
							ArcherPosition archerPosition2 = archerPositions.MinBy<ArcherPosition, float>((ArcherPosition ap) => ap.Entity.GlobalPosition.AsVec2.DistanceSquared(rangedFormation.CachedAveragePosition));
							rangedFormation.AI.SetBehaviorWeight<BehaviorSparseSkirmish>(1f);
							rangedFormation.AI.GetBehavior<BehaviorSparseSkirmish>().ArcherPosition = archerPosition2.Entity;
							archerPosition2.SetLastAssignedFormation(base.Team.TeamIndex, rangedFormation);
							archerPositions.Remove(archerPosition2);
						}
					}
				}
			}
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x0003C298 File Offset: 0x0003A498
		private void AllInAssault()
		{
			List<Formation> list = base.FormationsIncludingEmpty.Where<Formation>((Formation f) => f.CountOfUnits > 0).ToList<Formation>();
			List<SiegeLane> list2 = this.DetermineCurrentLanes();
			this.AssignMeleeFormationsToLanes(list, list2);
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x0003C2E4 File Offset: 0x0003A4E4
		private void StartTacticalRetreat()
		{
			this.StopUsingAllMachines();
			foreach (Formation formation in base.FormationsIncludingSpecialAndEmpty)
			{
				if (formation.CountOfUnits > 0)
				{
					formation.AI.ResetBehaviorWeights();
					TacticComponent.SetDefaultBehaviorWeights(formation);
					formation.AI.SetBehaviorWeight<BehaviorRetreatToKeep>(1f);
				}
			}
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x0003C364 File Offset: 0x0003A564
		protected override bool CheckAndSetAvailableFormationsChanged()
		{
			bool flag = false;
			int count = this.DetermineCurrentLanes().Count;
			if (this._laneCount != count)
			{
				this._laneCount = count;
				flag = true;
			}
			int aicontrolledFormationCount = base.Team.GetAIControlledFormationCount();
			bool flag2 = aicontrolledFormationCount != this._AIControlledFormationCount;
			if (flag2)
			{
				this._AIControlledFormationCount = aicontrolledFormationCount;
				this.IsTacticReapplyNeeded = true;
			}
			bool flag3 = false;
			bool flag4 = false;
			if (this._tacticState == TacticBreachWalls.TacticState.AssaultUnderRangedCover)
			{
				int num = 0;
				int num2 = 0;
				foreach (Formation formation in base.Team.FormationsIncludingEmpty)
				{
					if (formation.CountOfUnitsWithoutDetachedOnes > 0)
					{
						if (formation.QuerySystem.IsInfantryFormation)
						{
							num++;
						}
						if (formation.QuerySystem.IsRangedFormation)
						{
							num2++;
						}
					}
				}
				if (this._meleeFormations.Count == num && this._rangedFormations.Count == num2)
				{
					goto IL_01AC;
				}
				flag3 = true;
				this._meleeFormations.Clear();
				this._rangedFormations.Clear();
				using (List<Formation>.Enumerator enumerator = base.Team.FormationsIncludingEmpty.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Formation formation2 = enumerator.Current;
						if (formation2.CountOfUnitsWithoutDetachedOnes > 0)
						{
							if (formation2.QuerySystem.IsInfantryFormation)
							{
								this._meleeFormations.Add(formation2);
							}
							if (formation2.QuerySystem.IsRangedFormation)
							{
								this._rangedFormations.Add(formation2);
							}
						}
					}
					goto IL_01AC;
				}
			}
			if (this._tacticState == TacticBreachWalls.TacticState.TotalAttack)
			{
				int formationCount = base.Team.GetFormationCount();
				if ((formationCount < count && aicontrolledFormationCount > 0) || (formationCount > count && (formationCount - aicontrolledFormationCount < count || aicontrolledFormationCount > 1)))
				{
					flag4 = true;
				}
			}
			IL_01AC:
			return flag || flag2 || flag3 || flag4;
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x0003C544 File Offset: 0x0003A744
		private void MergeFormationsIfLanesBecameUnavailable(ref List<SiegeLane> currentLanes)
		{
			int count = currentLanes.Count;
			if (this._laneCount > count)
			{
				List<Formation> list = new List<Formation>();
				int num = 0;
				List<Formation> list2 = new List<Formation>();
				int num2 = 0;
				for (int i = 0; i < this._cachedUsedSiegeLanes.Count; i++)
				{
					bool flag = false;
					SiegeLane siegeLane = this._cachedUsedSiegeLanes[i];
					for (int j = 0; j < currentLanes.Count; j++)
					{
						if (siegeLane == currentLanes[j])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						Formation formation = siegeLane.GetLastAssignedFormation(base.Team.TeamIndex);
						if (formation != null && formation.IsSplittableByAI)
						{
							num += formation.CountOfUnits;
							list.Add(formation);
						}
					}
					else
					{
						Formation formation = siegeLane.GetLastAssignedFormation(base.Team.TeamIndex);
						if (formation != null)
						{
							num2 += formation.CountOfUnits;
							list2.Add(formation);
						}
					}
				}
				int num3 = MathF.Ceiling((float)(num + num2) / (float)list2.Count);
				for (int k = 0; k < list.Count; k++)
				{
					Formation formation2 = list[k];
					int num4 = formation2.CountOfUnits;
					for (int l = 0; l < list2.Count; l++)
					{
						Formation formation3 = list2[l];
						int num5 = num3 - formation3.CountOfUnits;
						if (num5 > 0)
						{
							int num6 = MathF.Min(num4, num5);
							num4 -= num6;
							formation2.TransferUnits(formation3, num6);
						}
					}
				}
				this._AIControlledFormationCount -= num;
			}
			this._cachedUsedSiegeLanes = currentLanes;
			this._laneCount = currentLanes.Count;
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x0003C6E4 File Offset: 0x0003A8E4
		private void MergeFormationsIfArcherPositionsBecameUnavailable(ref List<ArcherPosition> currentArcherPositions)
		{
			int count = currentArcherPositions.Count;
			if (this._cachedUsedArcherPositions.Count > count)
			{
				List<Formation> list = new List<Formation>();
				int num = 0;
				List<Formation> list2 = new List<Formation>();
				int num2 = 0;
				for (int i = 0; i < this._cachedUsedArcherPositions.Count; i++)
				{
					bool flag = false;
					ArcherPosition archerPosition = this._cachedUsedArcherPositions[i];
					for (int j = 0; j < currentArcherPositions.Count; j++)
					{
						if (archerPosition == currentArcherPositions[j])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						Formation formation = archerPosition.GetLastAssignedFormation(base.Team.TeamIndex);
						if (formation != null && formation.IsSplittableByAI)
						{
							num += formation.CountOfUnits;
							list.Add(formation);
						}
					}
					else
					{
						Formation formation = archerPosition.GetLastAssignedFormation(base.Team.TeamIndex);
						if (formation != null)
						{
							num2 += formation.CountOfUnits;
							list2.Add(formation);
						}
					}
				}
				int num3 = MathF.Ceiling((float)(num + num2) / (float)list2.Count);
				for (int k = 0; k < list.Count; k++)
				{
					Formation formation2 = list[k];
					int num4 = formation2.CountOfUnits;
					for (int l = 0; l < list2.Count; l++)
					{
						Formation formation3 = list2[l];
						int num5 = num3 - formation3.CountOfUnits;
						if (num5 > 0)
						{
							int num6 = MathF.Min(num4, num5);
							num4 -= num6;
							formation2.TransferUnits(formation3, num6);
						}
					}
				}
				this._AIControlledFormationCount -= num;
			}
			this._cachedUsedArcherPositions = currentArcherPositions;
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x0003C87C File Offset: 0x0003AA7C
		protected override void ManageFormationCounts()
		{
			List<SiegeLane> list = this.DetermineCurrentLanes();
			if (this._indicators == null && base.Team.QuerySystem.EnemyUnitCount > 0)
			{
				this._indicators = new TacticBreachWalls.BreachWallsProgressIndicators(base.Team, list);
			}
			if (this._tacticState == TacticBreachWalls.TacticState.Retreating)
			{
				return;
			}
			int count = list.Count;
			if (this._tacticState == TacticBreachWalls.TacticState.AssaultUnderRangedCover)
			{
				int num = MathF.Min(this.DetermineCurrentArcherPositions(list).Count, 8 - count);
				base.ManageFormationCounts(count, num, 0, 0);
				this._meleeFormations = base.FormationsIncludingEmpty.Where<Formation>((Formation f) => f.QuerySystem.IsInfantryFormation && f.CountOfUnitsWithoutDetachedOnes > 0).ToList<Formation>();
				this._rangedFormations = base.FormationsIncludingEmpty.Where<Formation>((Formation f) => f.QuerySystem.IsRangedFormation && f.CountOfUnitsWithoutDetachedOnes > 0).ToList<Formation>();
				return;
			}
			if (this._tacticState == TacticBreachWalls.TacticState.TotalAttack)
			{
				base.SplitFormationClassIntoGivenNumber((Formation f) => true, count);
			}
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x0003C994 File Offset: 0x0003AB94
		private void CheckAndChangeState()
		{
			if (this._tacticState == TacticBreachWalls.TacticState.Retreating)
			{
				return;
			}
			bool isShockAssault = this._isShockAssault;
			List<SiegeLane> list = this.DetermineCurrentLanes();
			int num = 0;
			foreach (Formation formation in base.Team.FormationsIncludingEmpty)
			{
				if (formation.CountOfUnits > 0 && TeamAISiegeComponent.IsFormationInsideCastle(formation, true, 0.4f))
				{
					num++;
				}
			}
			if (this.ShouldRetreat(list, num))
			{
				this._tacticState = TacticBreachWalls.TacticState.Retreating;
				this.StartTacticalRetreat();
				this.IsTacticReapplyNeeded = false;
				return;
			}
			TacticBreachWalls.TacticState tacticState = TacticBreachWalls.TacticState.TotalAttack;
			List<ArcherPosition> list2 = null;
			if (this._tacticState != TacticBreachWalls.TacticState.TotalAttack)
			{
				list2 = this.DetermineCurrentArcherPositions(list);
				if (list2.Count > 0)
				{
					int num2 = MathF.Max(this._meleeFormations.Sum<Formation>((Formation mf) => mf.CountOfUnits), 1);
					int num3 = MathF.Max(this._rangedFormations.Sum<Formation>((Formation rf) => rf.CountOfUnits), 1);
					int num4 = num2 + num3;
					int num5 = num4 - base.Team.FormationsIncludingEmpty.Sum<Formation>((Formation f) => f.CountOfUnitsWithoutDetachedOnes);
					tacticState = (((float)num2 / (float)num3 > 0.5f && (float)num5 / (float)num4 < 0.2f) ? TacticBreachWalls.TacticState.AssaultUnderRangedCover : TacticBreachWalls.TacticState.TotalAttack);
				}
			}
			if (tacticState != this._tacticState || isShockAssault != this._isShockAssault)
			{
				if (tacticState == TacticBreachWalls.TacticState.AssaultUnderRangedCover)
				{
					this._tacticState = TacticBreachWalls.TacticState.AssaultUnderRangedCover;
					this.ManageFormationCounts();
					this.WellRoundedAssault(ref list, ref list2);
					this.IsTacticReapplyNeeded = false;
					return;
				}
				if (tacticState != TacticBreachWalls.TacticState.TotalAttack)
				{
					return;
				}
				this._tacticState = TacticBreachWalls.TacticState.TotalAttack;
				this.ManageFormationCounts();
				this.AllInAssault();
				this.IsTacticReapplyNeeded = false;
			}
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x0003CB74 File Offset: 0x0003AD74
		private List<SiegeLane> DetermineCurrentLanes()
		{
			List<SiegeLane> list = TeamAISiegeComponent.SiegeLanes.Where<SiegeLane>((SiegeLane sl) => sl.IsBreach).ToList<SiegeLane>();
			if (list.Count >= 2)
			{
				if (this._indicators == null || this._indicators.InitialUnitCount > 100)
				{
					return list;
				}
				if (!this._isShockAssault)
				{
					this.StopUsingAllMachines();
					this._isShockAssault = true;
				}
				return list.Take<SiegeLane>(1).ToList<SiegeLane>();
			}
			else
			{
				List<SiegeLane> list2 = TeamAISiegeComponent.SiegeLanes.Where<SiegeLane>((SiegeLane sl) => !sl.CalculateIsLaneUnusable()).ToList<SiegeLane>();
				if (list2.Count <= 0)
				{
					return TeamAISiegeComponent.SiegeLanes.Where<SiegeLane>((SiegeLane sl) => sl.HasGate).ToList<SiegeLane>();
				}
				if (this._indicators != null && this._indicators.InitialUnitCount <= 100)
				{
					List<SiegeLane> list3 = new List<SiegeLane>();
					list3.Add(list2.MaxBy<SiegeLane, float>((SiegeLane ul) => ul.CalculateLaneCapacity()));
					if (!this._isShockAssault)
					{
						this.StopUsingAllMachines();
						this._isShockAssault = true;
					}
					return list3;
				}
				if (list.Count >= 1)
				{
					return list2.Where<SiegeLane>(delegate(SiegeLane l)
					{
						if (!l.IsBreach)
						{
							return l.PrimarySiegeWeapons.Any<IPrimarySiegeWeapon>((IPrimarySiegeWeapon psw) => !(psw is SiegeLadder));
						}
						return true;
					}).ToList<SiegeLane>();
				}
				return list2;
			}
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x0003CCF8 File Offset: 0x0003AEF8
		private List<ArcherPosition> DetermineCurrentArcherPositions(List<SiegeLane> currentLanes)
		{
			return this._teamAISiegeAttacker.ArcherPositions.Where<ArcherPosition>((ArcherPosition ap) => currentLanes.Any<SiegeLane>((SiegeLane cl) => ap.IsArcherPositionRelatedToSide(cl.LaneSide))).ToList<ArcherPosition>();
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x0003CD34 File Offset: 0x0003AF34
		public override void TickOccasionally()
		{
			if (!base.AreFormationsCreated)
			{
				return;
			}
			this._meleeFormations.RemoveAll((Formation mf) => mf.CountOfUnitsWithoutDetachedOnes == 0);
			this._rangedFormations.RemoveAll((Formation rf) => rf.CountOfUnitsWithoutDetachedOnes == 0);
			List<SiegeLane> list = this.DetermineCurrentLanes();
			this.MergeFormationsIfLanesBecameUnavailable(ref list);
			bool flag = this.CheckAndSetAvailableFormationsChanged();
			if (this._indicators == null && base.Team.QuerySystem.EnemyUnitCount > 0)
			{
				this._indicators = new TacticBreachWalls.BreachWallsProgressIndicators(base.Team, list);
				this._indicators.StartingPowerRatio = base.Team.QuerySystem.TotalPowerRatio;
				this._indicators.InitialLaneCount = list.Count;
				this._indicators.InitialUnitCount = base.Team.QuerySystem.AllyUnitCount;
			}
			int num = 0;
			foreach (SiegeLane siegeLane in list)
			{
				num |= MathF.PowTwo32((int)siegeLane.LaneSide);
			}
			this.IsTacticReapplyNeeded = num != this._lanesInUse;
			this._lanesInUse = num;
			if (flag)
			{
				this.ManageFormationCounts();
			}
			this.CheckAndChangeState();
			switch (this._tacticState)
			{
			case TacticBreachWalls.TacticState.AssaultUnderRangedCover:
			{
				List<ArcherPosition> list2 = this.DetermineCurrentArcherPositions(list);
				if (flag || this.IsTacticReapplyNeeded)
				{
					this._cachedUsedArcherPositions = list2;
					this.WellRoundedAssault(ref list, ref list2);
					this.IsTacticReapplyNeeded = false;
				}
				else if (this._cachedUsedArcherPositions.Count != list2.Count)
				{
					this.MergeFormationsIfArcherPositionsBecameUnavailable(ref list2);
				}
				this.BalanceAssaultLanes(this._meleeFormations.Where<Formation>((Formation mf) => mf.IsAIControlled && mf.IsAITickedAfterSplit && (mf.AI.ActiveBehavior is BehaviorUseSiegeMachines || mf.AI.ActiveBehavior is BehaviorWaitForLadders)).ToList<Formation>());
				break;
			}
			case TacticBreachWalls.TacticState.TotalAttack:
				if (flag || this.IsTacticReapplyNeeded)
				{
					this.AllInAssault();
					this.IsTacticReapplyNeeded = false;
				}
				this.BalanceAssaultLanes(base.FormationsIncludingEmpty.Where<Formation>((Formation f) => f.CountOfUnits > 0 && f.IsAIControlled && f.IsAITickedAfterSplit && (f.AI.ActiveBehavior is BehaviorUseSiegeMachines || f.AI.ActiveBehavior is BehaviorWaitForLadders)).ToList<Formation>());
				break;
			case TacticBreachWalls.TacticState.Retreating:
				if (flag || this.IsTacticReapplyNeeded)
				{
					this.StartTacticalRetreat();
					this.IsTacticReapplyNeeded = false;
				}
				break;
			}
			TeamAISiegeComponent teamAISiegeAttacker = this._teamAISiegeAttacker;
			bool flag2;
			if (list.Count<SiegeLane>((SiegeLane l) => l.IsBreach) <= 1)
			{
				if (list.Any<SiegeLane>((SiegeLane l) => l.PrimarySiegeWeapons.Any<IPrimarySiegeWeapon>((IPrimarySiegeWeapon psw) => psw.HoldLadders)))
				{
					flag2 = list.Any<SiegeLane>((SiegeLane l) => l.PrimarySiegeWeapons.Any<IPrimarySiegeWeapon>((IPrimarySiegeWeapon psw) => psw.SendLadders));
					goto IL_02D5;
				}
			}
			flag2 = true;
			IL_02D5:
			teamAISiegeAttacker.SetAreLaddersReady(flag2);
			this.CheckAndSetAvailableFormationsChanged();
			base.TickOccasionally();
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x0003D038 File Offset: 0x0003B238
		protected internal override float GetTacticWeight()
		{
			return 10f;
		}

		// Token: 0x040004A7 RID: 1191
		public const float SameBehaviorFactor = 3f;

		// Token: 0x040004A8 RID: 1192
		public const float SameSideFactor = 5f;

		// Token: 0x040004A9 RID: 1193
		private const int ShockAssaultThresholdCount = 100;

		// Token: 0x040004AA RID: 1194
		private readonly TeamAISiegeAttacker _teamAISiegeAttacker;

		// Token: 0x040004AB RID: 1195
		private TacticBreachWalls.BreachWallsProgressIndicators _indicators;

		// Token: 0x040004AC RID: 1196
		private List<Formation> _meleeFormations;

		// Token: 0x040004AD RID: 1197
		private List<Formation> _rangedFormations;

		// Token: 0x040004AE RID: 1198
		private int _laneCount;

		// Token: 0x040004AF RID: 1199
		private List<SiegeLane> _cachedUsedSiegeLanes;

		// Token: 0x040004B0 RID: 1200
		private int _lanesInUse;

		// Token: 0x040004B1 RID: 1201
		private List<ArcherPosition> _cachedUsedArcherPositions;

		// Token: 0x040004B2 RID: 1202
		private TacticBreachWalls.TacticState _tacticState;

		// Token: 0x040004B3 RID: 1203
		private bool _isShockAssault;

		// Token: 0x0200048F RID: 1167
		private class BreachWallsProgressIndicators
		{
			// Token: 0x0600394B RID: 14667 RVA: 0x000E8F2C File Offset: 0x000E712C
			public BreachWallsProgressIndicators(Team team, List<SiegeLane> lanes)
			{
				this.StartingPowerRatio = team.QuerySystem.RemainingPowerRatio;
				this.InitialUnitCount = team.QuerySystem.AllyUnitCount;
				this.InitialLaneCount = ((this.InitialUnitCount > 100) ? lanes.Count : 1);
				this._insideFormationEffect = 1f / (float)this.InitialLaneCount;
				this._openLaneEffect = 0.7f / (float)this.InitialLaneCount;
				this._existingLaneEffect = 0.4f / (float)this.InitialLaneCount;
			}

			// Token: 0x0600394C RID: 14668 RVA: 0x000E8FB4 File Offset: 0x000E71B4
			public float GetRetreatThresholdRatio(List<SiegeLane> lanes, int insideFormationCount)
			{
				float num = 1f;
				num -= (float)insideFormationCount * this._insideFormationEffect;
				int num2 = lanes.Count<SiegeLane>((SiegeLane l) => !l.IsOpen);
				int num3 = lanes.Count - num2 - insideFormationCount;
				if (num3 > 0)
				{
					num -= (float)num3 * this._openLaneEffect;
				}
				return num - (float)num2 * this._existingLaneEffect;
			}

			// Token: 0x04001AC8 RID: 6856
			public float StartingPowerRatio;

			// Token: 0x04001AC9 RID: 6857
			public int InitialLaneCount;

			// Token: 0x04001ACA RID: 6858
			public int InitialUnitCount;

			// Token: 0x04001ACB RID: 6859
			private readonly float _insideFormationEffect;

			// Token: 0x04001ACC RID: 6860
			private readonly float _openLaneEffect;

			// Token: 0x04001ACD RID: 6861
			private readonly float _existingLaneEffect;
		}

		// Token: 0x02000490 RID: 1168
		private enum TacticState
		{
			// Token: 0x04001ACF RID: 6863
			Unset,
			// Token: 0x04001AD0 RID: 6864
			AssaultUnderRangedCover,
			// Token: 0x04001AD1 RID: 6865
			TotalAttack,
			// Token: 0x04001AD2 RID: 6866
			Retreating
		}
	}
}
