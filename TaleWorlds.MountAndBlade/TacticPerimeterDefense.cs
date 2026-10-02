using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000171 RID: 369
	public class TacticPerimeterDefense : TacticComponent
	{
		// Token: 0x06001351 RID: 4945 RVA: 0x00045378 File Offset: 0x00043578
		public TacticPerimeterDefense(Team team)
			: base(team)
		{
			Scene scene = Mission.Current.Scene;
			FleePosition fleePosition = Mission.Current.GetFleePositionsForSide(BattleSideEnum.Defender).FirstOrDefault<FleePosition>((FleePosition fp) => fp.GetSide() == BattleSideEnum.Defender);
			if (fleePosition != null)
			{
				this._defendPosition = fleePosition.GameEntity.GlobalPosition.ToWorldPosition();
			}
			else
			{
				this._defendPosition = WorldPosition.Invalid;
			}
			this._enemyClusters = new List<TacticPerimeterDefense.EnemyCluster>();
			this._defenseFronts = new List<TacticPerimeterDefense.DefenseFront>();
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x00045408 File Offset: 0x00043608
		private void DetermineEnemyClusters()
		{
			List<Formation> list = new List<Formation>();
			float num = 0f;
			foreach (Team team in base.Team.Mission.Teams)
			{
				if (team.IsEnemyOf(base.Team))
				{
					num += team.QuerySystem.TeamPower;
				}
			}
			foreach (Team team2 in base.Team.Mission.Teams)
			{
				if (team2.IsEnemyOf(base.Team))
				{
					for (int i = 0; i < Math.Min(team2.FormationsIncludingSpecialAndEmpty.Count, 8); i++)
					{
						Formation enemyFormation = team2.FormationsIncludingSpecialAndEmpty[i];
						if (enemyFormation.CountOfUnits > 0 && enemyFormation.QuerySystem.FormationPower < MathF.Min(base.Team.QuerySystem.TeamPower, num) / 4f)
						{
							if (!this._enemyClusters.Any<TacticPerimeterDefense.EnemyCluster>((TacticPerimeterDefense.EnemyCluster ec) => ec.EnemyFormations.IndexOf(enemyFormation) >= 0))
							{
								list.Add(enemyFormation);
							}
						}
						else
						{
							TacticPerimeterDefense.EnemyCluster enemyCluster = this._enemyClusters.FirstOrDefault<TacticPerimeterDefense.EnemyCluster>((TacticPerimeterDefense.EnemyCluster ec) => ec.EnemyFormations.IndexOf(enemyFormation) >= 0);
							if (enemyCluster != null)
							{
								if ((double)(this._defendPosition.AsVec2 - enemyCluster.AggregatePosition).DotProduct(this._defendPosition.AsVec2 - enemyFormation.CachedAveragePosition) >= 0.70710678118)
								{
									goto IL_0211;
								}
								enemyCluster.RemoveFromCluster(enemyFormation);
							}
							List<TacticPerimeterDefense.EnemyCluster> list2 = this._enemyClusters.Where<TacticPerimeterDefense.EnemyCluster>((TacticPerimeterDefense.EnemyCluster c) => (double)(this._defendPosition.AsVec2 - c.AggregatePosition).DotProduct(this._defendPosition.AsVec2 - enemyFormation.CachedMedianPosition.AsVec2) >= 0.70710678118).ToList<TacticPerimeterDefense.EnemyCluster>();
							if (list2.Count > 0)
							{
								list2.MaxBy<TacticPerimeterDefense.EnemyCluster, float>((TacticPerimeterDefense.EnemyCluster ec) => (this._defendPosition.AsVec2 - ec.AggregatePosition).DotProduct(this._defendPosition.AsVec2 - enemyFormation.CachedMedianPosition.AsVec2)).AddToCluster(enemyFormation);
							}
							else
							{
								TacticPerimeterDefense.EnemyCluster enemyCluster2 = new TacticPerimeterDefense.EnemyCluster();
								enemyCluster2.AddToCluster(enemyFormation);
								this._enemyClusters.Add(enemyCluster2);
							}
						}
						IL_0211:;
					}
				}
			}
			if (this._enemyClusters.Count > 0)
			{
				using (List<Formation>.Enumerator enumerator2 = list.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Formation skippedFormation = enumerator2.Current;
						this._enemyClusters.MaxBy<TacticPerimeterDefense.EnemyCluster, float>((TacticPerimeterDefense.EnemyCluster ec) => (this._defendPosition.AsVec2 - ec.AggregatePosition).DotProduct(this._defendPosition.AsVec2 - skippedFormation.CachedMedianPosition.AsVec2)).AddToCluster(skippedFormation);
					}
				}
			}
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x00045720 File Offset: 0x00043920
		private bool MustRetreatToCastle()
		{
			return base.Team.QuerySystem.TotalPowerRatio / base.Team.QuerySystem.RemainingPowerRatio > 2f;
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x0004574C File Offset: 0x0004394C
		private void StartRetreatToKeep()
		{
			foreach (Formation formation in base.FormationsIncludingEmpty)
			{
				if (formation.CountOfUnits > 0)
				{
					formation.AI.ResetBehaviorWeights();
					TacticComponent.SetDefaultBehaviorWeights(formation);
					formation.AI.SetBehaviorWeight<BehaviorRetreatToKeep>(1f);
				}
			}
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x000457C4 File Offset: 0x000439C4
		private void CheckAndChangeState()
		{
			if (this.MustRetreatToCastle())
			{
				if (this._isRetreatingToKeep)
				{
					return;
				}
				this._isRetreatingToKeep = true;
				this.StartRetreatToKeep();
			}
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x000457E4 File Offset: 0x000439E4
		private void ArrangeDefenseFronts()
		{
			this._meleeFormations = base.FormationsIncludingEmpty.Where<Formation>((Formation f) => f.CountOfUnits > 0 && (f.QuerySystem.IsInfantryFormation || f.QuerySystem.IsCavalryFormation)).ToList<Formation>();
			this._rangedFormations = base.FormationsIncludingEmpty.Where<Formation>((Formation f) => f.CountOfUnits > 0 && (f.QuerySystem.IsRangedFormation || f.QuerySystem.IsRangedCavalryFormation)).ToList<Formation>();
			int num = MathF.Min(8 - this._rangedFormations.Count, this._enemyClusters.Count);
			if (this._meleeFormations.Count != num)
			{
				base.SplitFormationClassIntoGivenNumber((Formation f) => f.QuerySystem.IsInfantryFormation || f.QuerySystem.IsCavalryFormation, num);
				this._meleeFormations = base.FormationsIncludingEmpty.Where<Formation>((Formation f) => f.CountOfUnits > 0 && (f.QuerySystem.IsInfantryFormation || f.QuerySystem.IsCavalryFormation)).ToList<Formation>();
			}
			int num2 = MathF.Min(8 - num, this._enemyClusters.Count);
			if (this._rangedFormations.Count != num2)
			{
				base.SplitFormationClassIntoGivenNumber((Formation f) => f.QuerySystem.IsRangedFormation || f.QuerySystem.IsRangedCavalryFormation, num2);
				this._rangedFormations = base.FormationsIncludingEmpty.Where<Formation>((Formation f) => f.CountOfUnits > 0 && (f.QuerySystem.IsRangedFormation || f.QuerySystem.IsRangedCavalryFormation)).ToList<Formation>();
			}
			foreach (TacticPerimeterDefense.DefenseFront defenseFront in this._defenseFronts)
			{
				defenseFront.MatchedEnemyCluster.UpdateClusterData();
				BehaviorDefendKeyPosition behaviorDefendKeyPosition = defenseFront.MeleeFormation.AI.SetBehaviorWeight<BehaviorDefendKeyPosition>(1f);
				behaviorDefendKeyPosition.EnemyClusterPosition = defenseFront.MatchedEnemyCluster.MedianAggregatePosition;
				behaviorDefendKeyPosition.EnemyClusterPosition.SetVec2(defenseFront.MatchedEnemyCluster.AggregatePosition);
			}
			IEnumerable<TacticPerimeterDefense.EnemyCluster> enumerable = this._enemyClusters.Where<TacticPerimeterDefense.EnemyCluster>((TacticPerimeterDefense.EnemyCluster ec) => this._defenseFronts.All<TacticPerimeterDefense.DefenseFront>((TacticPerimeterDefense.DefenseFront df) => df.MatchedEnemyCluster != ec));
			List<Formation> list = this._meleeFormations.Where<Formation>((Formation mf) => this._defenseFronts.All<TacticPerimeterDefense.DefenseFront>((TacticPerimeterDefense.DefenseFront df) => df.MeleeFormation != mf)).ToList<Formation>();
			List<Formation> list2 = this._rangedFormations.Where<Formation>((Formation rf) => this._defenseFronts.All<TacticPerimeterDefense.DefenseFront>((TacticPerimeterDefense.DefenseFront df) => df.RangedFormation != rf)).ToList<Formation>();
			foreach (TacticPerimeterDefense.EnemyCluster enemyCluster in enumerable)
			{
				if (list.IsEmpty<Formation>())
				{
					break;
				}
				Formation formation = list[list.Count - 1];
				TacticPerimeterDefense.DefenseFront defenseFront2 = new TacticPerimeterDefense.DefenseFront(enemyCluster, formation);
				formation.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(formation);
				BehaviorDefendKeyPosition behaviorDefendKeyPosition2 = formation.AI.SetBehaviorWeight<BehaviorDefendKeyPosition>(1f);
				behaviorDefendKeyPosition2.DefensePosition = this._defendPosition;
				behaviorDefendKeyPosition2.EnemyClusterPosition = enemyCluster.MedianAggregatePosition;
				behaviorDefendKeyPosition2.EnemyClusterPosition.SetVec2(enemyCluster.AggregatePosition);
				list.Remove(formation);
				if (!list2.IsEmpty<Formation>())
				{
					Formation formation2 = list2[list2.Count - 1];
					formation2.AI.ResetBehaviorWeights();
					TacticComponent.SetDefaultBehaviorWeights(formation2);
					formation2.AI.SetBehaviorWeight<BehaviorSkirmishBehindFormation>(1f).ReferenceFormation = formation;
					defenseFront2.RangedFormation = formation2;
					list2.Remove(formation2);
					this._defenseFronts.Add(defenseFront2);
				}
			}
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x00045B58 File Offset: 0x00043D58
		public override void TickOccasionally()
		{
			if (!base.AreFormationsCreated)
			{
				return;
			}
			this.CheckAndChangeState();
			if (!this._isRetreatingToKeep)
			{
				this.DetermineEnemyClusters();
				this.ArrangeDefenseFronts();
			}
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x00045B7D File Offset: 0x00043D7D
		protected internal override float GetTacticWeight()
		{
			if (this._defendPosition.IsValid)
			{
				return 10f;
			}
			return 0f;
		}

		// Token: 0x040004E9 RID: 1257
		private WorldPosition _defendPosition;

		// Token: 0x040004EA RID: 1258
		private readonly List<TacticPerimeterDefense.EnemyCluster> _enemyClusters;

		// Token: 0x040004EB RID: 1259
		private readonly List<TacticPerimeterDefense.DefenseFront> _defenseFronts;

		// Token: 0x040004EC RID: 1260
		private const float RetreatThresholdValue = 2f;

		// Token: 0x040004ED RID: 1261
		private List<Formation> _meleeFormations;

		// Token: 0x040004EE RID: 1262
		private List<Formation> _rangedFormations;

		// Token: 0x040004EF RID: 1263
		private bool _isRetreatingToKeep;

		// Token: 0x020004B5 RID: 1205
		private class DefenseFront
		{
			// Token: 0x06003A13 RID: 14867 RVA: 0x000E9E67 File Offset: 0x000E8067
			public DefenseFront(TacticPerimeterDefense.EnemyCluster matchedEnemyCluster, Formation meleeFormation)
			{
				this.MatchedEnemyCluster = matchedEnemyCluster;
				this.MeleeFormation = meleeFormation;
				this.RangedFormation = null;
			}

			// Token: 0x04001B89 RID: 7049
			public Formation MeleeFormation;

			// Token: 0x04001B8A RID: 7050
			public Formation RangedFormation;

			// Token: 0x04001B8B RID: 7051
			public TacticPerimeterDefense.EnemyCluster MatchedEnemyCluster;
		}

		// Token: 0x020004B6 RID: 1206
		private class EnemyCluster
		{
			// Token: 0x17000A29 RID: 2601
			// (get) Token: 0x06003A14 RID: 14868 RVA: 0x000E9E84 File Offset: 0x000E8084
			// (set) Token: 0x06003A15 RID: 14869 RVA: 0x000E9E8C File Offset: 0x000E808C
			public Vec2 AggregatePosition { get; private set; }

			// Token: 0x17000A2A RID: 2602
			// (get) Token: 0x06003A16 RID: 14870 RVA: 0x000E9E95 File Offset: 0x000E8095
			// (set) Token: 0x06003A17 RID: 14871 RVA: 0x000E9E9D File Offset: 0x000E809D
			public WorldPosition MedianAggregatePosition { get; private set; }

			// Token: 0x17000A2B RID: 2603
			// (get) Token: 0x06003A18 RID: 14872 RVA: 0x000E9EA6 File Offset: 0x000E80A6
			public MBReadOnlyList<Formation> EnemyFormations
			{
				get
				{
					return this._enemyFormations;
				}
			}

			// Token: 0x06003A19 RID: 14873 RVA: 0x000E9EB0 File Offset: 0x000E80B0
			public void UpdateClusterData()
			{
				this._totalPower = this._enemyFormations.Sum<Formation>((Formation ef) => ef.QuerySystem.FormationPower);
				this.AggregatePosition = Vec2.Zero;
				foreach (Formation formation in this._enemyFormations)
				{
					this.AggregatePosition += formation.CachedAveragePosition * (formation.QuerySystem.FormationPower / this._totalPower);
				}
				this.UpdateMedianPosition();
			}

			// Token: 0x06003A1A RID: 14874 RVA: 0x000E9F6C File Offset: 0x000E816C
			public void AddToCluster(Formation formation)
			{
				this._enemyFormations.Add(formation);
				float totalPower = this._totalPower;
				this._totalPower += formation.QuerySystem.FormationPower;
				this.AggregatePosition = this.AggregatePosition * (totalPower / this._totalPower) + formation.CachedAveragePosition * (formation.QuerySystem.FormationPower / this._totalPower);
				this.UpdateMedianPosition();
			}

			// Token: 0x06003A1B RID: 14875 RVA: 0x000E9FE8 File Offset: 0x000E81E8
			public void RemoveFromCluster(Formation formation)
			{
				this._enemyFormations.Remove(formation);
				float totalPower = this._totalPower;
				this._totalPower -= formation.QuerySystem.FormationPower;
				this.AggregatePosition -= formation.CachedAveragePosition * (formation.QuerySystem.FormationPower / totalPower);
				this.AggregatePosition *= totalPower / this._totalPower;
				this.UpdateMedianPosition();
			}

			// Token: 0x06003A1C RID: 14876 RVA: 0x000EA06C File Offset: 0x000E826C
			private void UpdateMedianPosition()
			{
				float num = float.MaxValue;
				foreach (Formation formation in this._enemyFormations)
				{
					float num2 = formation.CachedMedianPosition.AsVec2.DistanceSquared(this.AggregatePosition);
					if (num2 < num)
					{
						num = num2;
						this.MedianAggregatePosition = formation.CachedMedianPosition;
					}
				}
			}

			// Token: 0x04001B8C RID: 7052
			private readonly MBList<Formation> _enemyFormations = new MBList<Formation>();

			// Token: 0x04001B8D RID: 7053
			private float _totalPower;
		}
	}
}
