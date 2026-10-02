using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000168 RID: 360
	public abstract class TacticComponent
	{
		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x060012C6 RID: 4806 RVA: 0x0003D3FC File Offset: 0x0003B5FC
		// (set) Token: 0x060012C7 RID: 4807 RVA: 0x0003D404 File Offset: 0x0003B604
		public Team Team { get; protected set; }

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x060012C8 RID: 4808 RVA: 0x0003D40D File Offset: 0x0003B60D
		protected MBList<Formation> FormationsIncludingSpecialAndEmpty
		{
			get
			{
				return this.Team.FormationsIncludingSpecialAndEmpty;
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x060012C9 RID: 4809 RVA: 0x0003D41A File Offset: 0x0003B61A
		protected MBList<Formation> FormationsIncludingEmpty
		{
			get
			{
				return this.Team.FormationsIncludingEmpty;
			}
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x0003D427 File Offset: 0x0003B627
		protected TacticComponent(Team team)
		{
			this.Team = team;
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x0003D465 File Offset: 0x0003B665
		protected internal virtual void OnCancel()
		{
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x0003D467 File Offset: 0x0003B667
		protected internal virtual void OnApply()
		{
			this.IsTacticReapplyNeeded = true;
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x0003D470 File Offset: 0x0003B670
		public virtual void TickOccasionally()
		{
			TeamAIComponent teamAI = this.Team.TeamAI;
			if (teamAI.GetIsFirstTacticChosen)
			{
				teamAI.OnTacticAppliedForFirstTime();
			}
			if (Mission.Current.IsMissionEnding)
			{
				this.StopUsingAllMachines();
				if (teamAI.HasStrategicAreas)
				{
					teamAI.RemoveAllStrategicAreas();
				}
			}
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x0003D4B8 File Offset: 0x0003B6B8
		private static void GetUnitCountByAttackType(Formation formation, out int unitCount, out int rangedCount, out int semiRangedCount, out int nonRangedCount)
		{
			FormationQuerySystem querySystem = formation.QuerySystem;
			unitCount = formation.CountOfUnits;
			rangedCount = (int)(querySystem.RangedUnitRatio * (float)unitCount);
			semiRangedCount = 0;
			nonRangedCount = unitCount - rangedCount;
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x0003D4EC File Offset: 0x0003B6EC
		protected static float GetFormationGroupEffectivenessOverOrder(IEnumerable<Formation> formationGroup, OrderType orderType, IOrderable targetObject = null)
		{
			int num;
			int num2;
			int num3;
			int num4;
			TacticComponent.GetUnitCountByAttackType(formationGroup.FirstOrDefault<Formation>(), out num, out num2, out num3, out num4);
			if (orderType == OrderType.PointDefence)
			{
				float num5 = ((float)num4 * 0.1f + (float)num3 * 0.3f + (float)num2 * 1f) / (float)num;
				int num6 = (targetObject as IPointDefendable).DefencePoints.Count<DefencePoint>() * 3;
				float num7 = MathF.Min((float)num * 1f / (float)num6, 1f);
				return num5 * num7;
			}
			if (orderType == OrderType.Use)
			{
				float num8 = ((float)num4 * 1f + (float)num3 * 0.9f + (float)num2 * 0.3f) / (float)num;
				int maxUserCount = (targetObject as UsableMachine).MaxUserCount;
				float num9 = MathF.Min((float)num * 1f / (float)maxUserCount, 1f);
				return num8 * num9;
			}
			if (orderType == OrderType.Charge)
			{
				return ((float)num4 * 1f + (float)num3 * 0.9f + (float)num2 * 0.3f) / (float)num;
			}
			return 1f;
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x0003D5D0 File Offset: 0x0003B7D0
		protected static float GetFormationEffectivenessOverOrder(Formation formation, OrderType orderType, IOrderable targetObject = null)
		{
			int num;
			int num2;
			int num3;
			int num4;
			TacticComponent.GetUnitCountByAttackType(formation, out num, out num2, out num3, out num4);
			if (orderType == OrderType.PointDefence)
			{
				float num5 = ((float)num4 * 0.1f + (float)num3 * 0.3f + (float)num2 * 1f) / (float)num;
				int num6 = (targetObject as IPointDefendable).DefencePoints.Count<DefencePoint>() * 3;
				float num7 = MathF.Min((float)num * 1f / (float)num6, 1f);
				return num5 * num7;
			}
			if (orderType == OrderType.Use)
			{
				float minDistanceSquared = float.MaxValue;
				formation.ApplyActionOnEachUnit(delegate(Agent agent)
				{
					minDistanceSquared = MathF.Min(agent.Position.DistanceSquared((targetObject as UsableMachine).GameEntity.GlobalPosition), minDistanceSquared);
				}, null);
				return 1f / MBMath.ClampFloat(minDistanceSquared, 1f, float.MaxValue);
			}
			if (orderType == OrderType.Charge)
			{
				return ((float)num4 * 1f + (float)num3 * 0.9f + (float)num2 * 0.3f) / (float)num;
			}
			return 1f;
		}

		// Token: 0x060012D2 RID: 4818 RVA: 0x0003D6C9 File Offset: 0x0003B8C9
		[Conditional("DEBUG")]
		protected internal virtual void DebugTick(float dt)
		{
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x0003D6CC File Offset: 0x0003B8CC
		private static int GetAvailableUnitCount(Formation formation, IEnumerable<ValueTuple<Formation, UsableMachine, int>> appliedCombinations)
		{
			int num = appliedCombinations.Where<ValueTuple<Formation, UsableMachine, int>>((ValueTuple<Formation, UsableMachine, int> c) => c.Item1 == formation).Sum<ValueTuple<Formation, UsableMachine, int>>((ValueTuple<Formation, UsableMachine, int> c) => c.Item3);
			int num2 = 0;
			return formation.CountOfUnits - (num + num2);
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x0003D730 File Offset: 0x0003B930
		private static int GetVacantSlotCount(UsableMachine weapon, IEnumerable<ValueTuple<Formation, UsableMachine, int>> appliedCombinations)
		{
			int num = appliedCombinations.Where<ValueTuple<Formation, UsableMachine, int>>((ValueTuple<Formation, UsableMachine, int> c) => c.Item2 == weapon).Sum<ValueTuple<Formation, UsableMachine, int>>((ValueTuple<Formation, UsableMachine, int> c) => c.Item3);
			return weapon.MaxUserCount - num;
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x0003D790 File Offset: 0x0003B990
		protected List<Formation> ConsolidateFormations(List<Formation> formationsToBeConsolidated, int neededCount)
		{
			if (formationsToBeConsolidated.Count <= neededCount || neededCount <= 0)
			{
				return formationsToBeConsolidated;
			}
			List<Formation> list = formationsToBeConsolidated.OrderByDescending<Formation, int>((Formation f) => f.CountOfUnits + ((!f.IsAIControlled) ? 10000 : 0)).ToList<Formation>();
			List<Formation> list2 = list.Take<Formation>(neededCount).Reverse<Formation>().ToList<Formation>();
			list.RemoveRange(0, neededCount);
			Queue<Formation> queue = new Queue<Formation>(list2);
			List<Formation> list3 = new List<Formation>();
			foreach (Formation formation in list)
			{
				if (!formation.IsAIControlled)
				{
					list3.Add(formation);
				}
				else
				{
					if (queue.IsEmpty<Formation>())
					{
						queue = new Queue<Formation>(list2);
					}
					Formation formation2 = queue.Dequeue();
					formation.TransferUnits(formation2, formation.CountOfUnits);
				}
			}
			return list2.Concat<Formation>(list3).ToList<Formation>();
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x0003D880 File Offset: 0x0003BA80
		protected static float CalculateNotEngagingTacticalAdvantage(TeamQuerySystem team)
		{
			float num = team.CavalryRatio + team.RangedCavalryRatio;
			float num2 = team.EnemyCavalryRatio + team.EnemyRangedCavalryRatio;
			return MathF.Pow(MBMath.ClampFloat((num > 0f) ? (num2 / num) : 1.5f, 1f, 1.5f), 1.5f * MathF.Max(num, num2));
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x0003D8DC File Offset: 0x0003BADC
		protected void SplitFormationClassIntoGivenNumber(Func<Formation, bool> formationClass, int count)
		{
			List<Formation> list = new List<Formation>();
			List<int> list2 = new List<int>();
			int num = 0;
			List<int> list3 = new List<int>();
			int num2 = 0;
			int i = 0;
			int num3 = 0;
			foreach (Formation formation in this.Team.FormationsIncludingEmpty)
			{
				if (formation.CountOfUnits == 0)
				{
					list.Add(formation);
					i++;
				}
				else if (formationClass(formation))
				{
					list.Add(formation);
					if (formation.IsAIOwned)
					{
						list2.Add(i);
						num += formation.CountOfUnits;
						if (formation.IsConvenientForTransfer)
						{
							list3.Add(i);
							num2 += formation.CountOfUnits;
						}
					}
					else
					{
						num3++;
					}
					i++;
				}
			}
			int num4 = count - num3;
			int count2 = list3.Count;
			int count3 = list2.Count;
			if (count3 > 0)
			{
				List<int> list4 = new List<int>();
				if (num4 > count3 && count2 > 0)
				{
					int num5 = num4 - count3;
					float num6 = (float)num / (float)num4;
					double num7 = Math.Ceiling((double)((float)num2 / (float)(list3.Count + num5)));
					List<int> list5 = new List<int>();
					int num8 = 0;
					for (i = 0; i < count3; i++)
					{
						if (list[list2[i]].IsConvenientForTransfer || (double)list[list2[i]].CountOfUnits <= num7)
						{
							list5.Add(i);
							num8 += list[list2[i]].CountOfUnits;
						}
					}
					double num9 = Math.Ceiling((double)((float)num8 / (float)(list5.Count + num5)));
					List<int> list6 = new List<int>();
					for (i = 0; i < list.Count; i++)
					{
						if (list[i].CountOfUnits == 0 && list6.Count < num5)
						{
							list6.Add(i);
						}
					}
					for (i = 0; i < count2; i++)
					{
						Formation formation2 = list[list3[i]];
						int num10 = (int)((double)formation2.CountOfUnits - num9);
						int num11 = 0;
						while (num10 >= 1 && num11 < list5.Count)
						{
							Formation formation3 = list[list5[num11]];
							if (formation2 != formation3)
							{
								int num12 = (int)(num9 - (double)formation3.CountOfUnits);
								if (num12 >= 1)
								{
									int num13 = MathF.Min(num10, num12);
									formation2.TransferUnits(formation3, num13);
									if (!list4.Contains(list3[i]))
									{
										list4.Add(list3[i]);
									}
									if (!list4.Contains(list5[num11]))
									{
										list4.Add(list5[num11]);
									}
									num10 -= num13;
								}
							}
							num11++;
						}
						if (num10 >= 1)
						{
							int num14 = 0;
							while (num10 >= 1 && num14 < list6.Count)
							{
								Formation formation4 = list[list6[num14]];
								int num15 = (int)(num7 - (double)formation4.CountOfUnits);
								if (num15 >= 1)
								{
									int num16 = MathF.Min(num10, num15);
									formation2.TransferUnits(formation4, num16);
									if (!list4.Contains(list3[i]))
									{
										list4.Add(list3[i]);
									}
									if (!list4.Contains(list6[num14]))
									{
										list4.Add(list6[num14]);
									}
									num10 -= num16;
								}
								num14++;
							}
						}
					}
				}
				else if (num4 < count3 && count3 != 1)
				{
					if (num4 < 1)
					{
						num4 = 1;
					}
					float num17 = (float)num / (float)num4;
					int num18 = 0;
					List<int> list7 = new List<int>();
					int num19 = 0;
					int num20 = 0;
					for (i = 0; i < list2.Count; i++)
					{
						Formation formation5 = list[list2[i]];
						bool flag = false;
						if (!formation5.IsConvenientForTransfer)
						{
							num18++;
							if ((float)formation5.CountOfUnits < num17)
							{
								flag = true;
							}
							else
							{
								num20++;
							}
						}
						else
						{
							flag = true;
						}
						if (flag)
						{
							list7.Add(list2[i]);
							num19 += formation5.CountOfUnits;
						}
					}
					if (num4 < 1)
					{
						num4 = 1;
					}
					else if (num4 < num18)
					{
						num4 = num18;
					}
					float num21 = (float)num19 / (float)(num4 - num20);
					List<int> list8 = new List<int>();
					int num22 = count3 - num4;
					while (list8.Count < num22 && count2 > 0)
					{
						int num23 = int.MaxValue;
						int num24 = -1;
						for (i = 0; i < list3.Count; i++)
						{
							Formation formation6 = list[list3[i]];
							if (formation6.CountOfUnits < num23)
							{
								num23 = formation6.CountOfUnits;
								num24 = list3[i];
							}
						}
						list3.Remove(num24);
						list8.Add(num24);
					}
					for (i = 0; i < list8.Count + list3.Count; i++)
					{
						bool flag2 = i < list8.Count;
						int num25;
						if (flag2)
						{
							num25 = list8[i];
						}
						else
						{
							num25 = list3[i - list8.Count];
						}
						Formation formation7 = list[num25];
						int num26;
						if (flag2)
						{
							num26 = formation7.CountOfUnits;
						}
						else
						{
							num26 = (int)((float)formation7.CountOfUnits - num21);
						}
						int num27 = 0;
						while (num26 >= 1 && num27 < list7.Count)
						{
							Formation formation8 = list[list7[num27]];
							if (formation7 != formation8 && formation8.CountOfUnits != 0)
							{
								int num28 = (int)Math.Ceiling((double)(num21 - (float)formation8.CountOfUnits));
								if (num28 >= 1)
								{
									int num29 = MathF.Min(num26, num28);
									formation7.TransferUnits(formation8, num29);
									if (!list4.Contains(num25))
									{
										list4.Add(num25);
									}
									if (!list4.Contains(list7[num27]))
									{
										list4.Add(list7[num27]);
									}
									num26 -= num29;
								}
							}
							num27++;
						}
					}
				}
				if (num4 > 1 && list4.Count > 1)
				{
					List<Formation> list9 = new List<Formation>();
					for (i = 0; i < list4.Count; i++)
					{
						Formation formation9 = list[list4[i]];
						if (formation9.CountOfUnits > 0)
						{
							formation9.AI.Side = FormationAI.BehaviorSide.BehaviorSideNotSet;
							formation9.AI.IsMainFormation = false;
							formation9.AI.ResetBehaviorWeights();
							TacticComponent.SetDefaultBehaviorWeights(formation9);
							list9.Add(formation9);
						}
					}
					this.IsTacticReapplyNeeded = true;
				}
			}
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x0003DF54 File Offset: 0x0003C154
		protected virtual bool CheckAndSetAvailableFormationsChanged()
		{
			return false;
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x060012D9 RID: 4825 RVA: 0x0003DF58 File Offset: 0x0003C158
		protected bool AreFormationsCreated
		{
			get
			{
				if (this._areFormationsCreated)
				{
					return true;
				}
				if (this.FormationsIncludingEmpty.AnyQ<Formation>((Formation f) => f.CountOfUnits > 0))
				{
					this._areFormationsCreated = true;
					this.ManageFormationCounts();
					this.CheckAndSetAvailableFormationsChanged();
					this.IsTacticReapplyNeeded = true;
					return true;
				}
				return false;
			}
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x0003DFB9 File Offset: 0x0003C1B9
		public void ResetTactic()
		{
			this.ManageFormationCounts();
			this.CheckAndSetAvailableFormationsChanged();
			this.IsTacticReapplyNeeded = true;
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x0003DFD0 File Offset: 0x0003C1D0
		protected void AssignTacticFormations1121()
		{
			this.ManageFormationCounts(1, 1, 2, 1);
			this._mainInfantry = TacticComponent.ChooseAndSortByPriority(this.FormationsIncludingEmpty, (Formation f) => f.CountOfUnits > 0 && f.QuerySystem.IsInfantryFormation, (Formation f) => f.IsAIControlled, (Formation f) => f.QuerySystem.FormationPower).FirstOrDefault<Formation>();
			if (this._mainInfantry != null)
			{
				this._mainInfantry.AI.IsMainFormation = true;
				this._mainInfantry.AI.Side = FormationAI.BehaviorSide.Middle;
			}
			this._archers = TacticComponent.ChooseAndSortByPriority(this.FormationsIncludingEmpty, (Formation f) => f.CountOfUnits > 0 && f.QuerySystem.IsRangedFormation, (Formation f) => f.IsAIControlled, (Formation f) => f.QuerySystem.FormationPower).FirstOrDefault<Formation>();
			List<Formation> list = TacticComponent.ChooseAndSortByPriority(this.FormationsIncludingEmpty, (Formation f) => f.CountOfUnits > 0 && f.QuerySystem.IsCavalryFormation, (Formation f) => f.IsAIControlled, (Formation f) => f.QuerySystem.FormationPower);
			if (list.Count > 0)
			{
				this._leftCavalry = list[0];
				this._leftCavalry.AI.Side = FormationAI.BehaviorSide.Left;
				if (list.Count > 1)
				{
					this._rightCavalry = list[1];
					this._rightCavalry.AI.Side = FormationAI.BehaviorSide.Right;
				}
				else
				{
					this._rightCavalry = null;
				}
			}
			else
			{
				this._leftCavalry = null;
				this._rightCavalry = null;
			}
			this._rangedCavalry = TacticComponent.ChooseAndSortByPriority(this.FormationsIncludingEmpty, (Formation f) => f.CountOfUnits > 0 && f.QuerySystem.IsRangedCavalryFormation, (Formation f) => f.IsAIControlled, (Formation f) => f.QuerySystem.FormationPower).FirstOrDefault<Formation>();
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x0003E23C File Offset: 0x0003C43C
		protected static List<Formation> ChooseAndSortByPriority(IEnumerable<Formation> formations, Func<Formation, bool> isEligible, Func<Formation, bool> isPrioritized, Func<Formation, float> score)
		{
			formations = formations.Where<Formation>(isEligible);
			IOrderedEnumerable<Formation> orderedEnumerable = formations.Where<Formation>(isPrioritized).OrderByDescending<Formation, float>(score);
			IOrderedEnumerable<Formation> orderedEnumerable2 = formations.Except<Formation>(orderedEnumerable).OrderByDescending<Formation, float>(score);
			return orderedEnumerable.Concat<Formation>(orderedEnumerable2).ToList<Formation>();
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x0003E27A File Offset: 0x0003C47A
		protected virtual void ManageFormationCounts()
		{
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x0003E27C File Offset: 0x0003C47C
		protected void ManageFormationCounts(int infantryCount, int rangedCount, int cavalryCount, int rangedCavalryCount)
		{
			this.SplitFormationClassIntoGivenNumber((Formation f) => f.QuerySystem.IsInfantryFormation, infantryCount);
			this.SplitFormationClassIntoGivenNumber((Formation f) => f.QuerySystem.IsRangedFormation, rangedCount);
			this.SplitFormationClassIntoGivenNumber((Formation f) => f.QuerySystem.IsCavalryFormation, cavalryCount);
			this.SplitFormationClassIntoGivenNumber((Formation f) => f.QuerySystem.IsRangedCavalryFormation, rangedCavalryCount);
		}

		// Token: 0x060012DF RID: 4831 RVA: 0x0003E324 File Offset: 0x0003C524
		protected virtual void StopUsingAllMachines()
		{
			TeamAISiegeComponent teamAISiegeComponent;
			if ((teamAISiegeComponent = this.Team.TeamAI as TeamAISiegeComponent) != null)
			{
				foreach (SiegeWeapon siegeWeapon in teamAISiegeComponent.SceneSiegeWeapons)
				{
					if (siegeWeapon.Side == this.Team.Side)
					{
						siegeWeapon.SetForcedUse(false);
						for (int i = siegeWeapon.UserFormations.Count - 1; i >= 0; i--)
						{
							Formation formation = siegeWeapon.UserFormations[i];
							if (formation.Team == this.Team)
							{
								formation.StopUsingMachine(siegeWeapon, false);
							}
						}
					}
				}
			}
		}

		// Token: 0x060012E0 RID: 4832 RVA: 0x0003E3E0 File Offset: 0x0003C5E0
		protected void StopUsingAllRangedSiegeWeapons()
		{
			foreach (Formation formation in this.FormationsIncludingSpecialAndEmpty)
			{
				if (formation.CountOfUnits > 0)
				{
					for (int i = formation.Detachments.Count - 1; i >= 0; i--)
					{
						RangedSiegeWeapon rangedSiegeWeapon;
						if ((rangedSiegeWeapon = formation.Detachments[i] as RangedSiegeWeapon) != null)
						{
							formation.StopUsingMachine(rangedSiegeWeapon, false);
							rangedSiegeWeapon.SetForcedUse(false);
						}
					}
				}
			}
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x0003E474 File Offset: 0x0003C674
		protected void SoundTacticalHorn(int soundCode)
		{
			float currentTime = Mission.Current.CurrentTime;
			if (currentTime > this._hornCooldownExpireTime)
			{
				this._hornCooldownExpireTime = currentTime + 10f;
				SoundEvent.PlaySound2D(soundCode);
			}
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x0003E4AC File Offset: 0x0003C6AC
		public static void SetDefaultBehaviorWeights(Formation f)
		{
			f.AI.SetBehaviorWeight<BehaviorCharge>(1f);
			f.AI.SetBehaviorWeight<BehaviorPullBack>(1f);
			f.AI.SetBehaviorWeight<BehaviorStop>(1f);
			f.AI.SetBehaviorWeight<BehaviorReserve>(1f);
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x0003E4FD File Offset: 0x0003C6FD
		protected internal virtual float GetTacticWeight()
		{
			return 0f;
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x0003E504 File Offset: 0x0003C704
		protected bool CheckAndDetermineFormation(ref Formation formation, Func<Formation, bool> isEligible)
		{
			if (formation != null && formation.CountOfUnits != 0 && isEligible(formation))
			{
				return true;
			}
			List<Formation> list = this.FormationsIncludingEmpty.Where<Formation>(isEligible).ToList<Formation>();
			if (list.Count > 0)
			{
				formation = list.MaxBy<Formation, int>((Formation f) => f.CountOfUnits);
				this.IsTacticReapplyNeeded = true;
				return true;
			}
			if (formation != null)
			{
				formation = null;
				this.IsTacticReapplyNeeded = true;
			}
			return false;
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x0003E584 File Offset: 0x0003C784
		protected internal virtual bool ResetTacticalPositions()
		{
			return false;
		}

		// Token: 0x040004B4 RID: 1204
		public static readonly int MoveHornSoundIndex = SoundEvent.GetEventIdFromString("event:/ui/mission/horns/move");

		// Token: 0x040004B5 RID: 1205
		public static readonly int AttackHornSoundIndex = SoundEvent.GetEventIdFromString("event:/ui/mission/horns/attack");

		// Token: 0x040004B6 RID: 1206
		public static readonly int RetreatHornSoundIndex = SoundEvent.GetEventIdFromString("event:/ui/mission/horns/retreat");

		// Token: 0x040004B8 RID: 1208
		protected int _AIControlledFormationCount;

		// Token: 0x040004B9 RID: 1209
		protected bool IsTacticReapplyNeeded;

		// Token: 0x040004BA RID: 1210
		private bool _areFormationsCreated;

		// Token: 0x040004BB RID: 1211
		protected Formation _mainInfantry;

		// Token: 0x040004BC RID: 1212
		protected Formation _archers;

		// Token: 0x040004BD RID: 1213
		protected Formation _leftCavalry;

		// Token: 0x040004BE RID: 1214
		protected Formation _rightCavalry;

		// Token: 0x040004BF RID: 1215
		protected Formation _rangedCavalry;

		// Token: 0x040004C0 RID: 1216
		private float _hornCooldownExpireTime;

		// Token: 0x040004C1 RID: 1217
		private const float HornCooldownTime = 10f;
	}
}
