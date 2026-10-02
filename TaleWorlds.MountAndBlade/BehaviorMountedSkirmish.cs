using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200011C RID: 284
	public class BehaviorMountedSkirmish : BehaviorComponent
	{
		// Token: 0x06000E36 RID: 3638 RVA: 0x000204CD File Offset: 0x0001E6CD
		public BehaviorMountedSkirmish(Formation formation)
			: base(formation)
		{
			this.CalculateCurrentOrder();
			base.BehaviorCoherence = 0.5f;
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x000204F8 File Offset: 0x0001E6F8
		protected override void CalculateCurrentOrder()
		{
			WorldPosition cachedMedianPosition = base.Formation.CachedMedianPosition;
			this._isEnemyReachable = base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation != null && (!(base.Formation.Team.TeamAI is TeamAISiegeComponent) || !TeamAISiegeComponent.IsFormationInsideCastle(base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation, false, 0.4f));
			if (!this._isEnemyReachable)
			{
				cachedMedianPosition.SetVec2(base.Formation.CachedAveragePosition);
			}
			else
			{
				bool flag;
				if ((base.Formation.QuerySystem.AverageAllyPosition - base.Formation.Team.QuerySystem.AverageEnemyPosition).LengthSquared <= 3600f)
				{
					flag = true;
				}
				else if (!this._engaging)
				{
					flag = (base.Formation.CachedAveragePosition - base.Formation.QuerySystem.AverageAllyPosition).LengthSquared <= 3600f;
				}
				else
				{
					flag = base.Formation.QuerySystem.UnderRangedAttackRatio <= base.Formation.QuerySystem.MakingRangedAttackRatio && ((!base.Formation.QuerySystem.FastestSignificantlyLargeEnemyFormation.IsCavalryFormation && !base.Formation.QuerySystem.FastestSignificantlyLargeEnemyFormation.IsRangedCavalryFormation) || (base.Formation.CachedAveragePosition - base.Formation.QuerySystem.FastestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition.AsVec2).LengthSquared / (base.Formation.QuerySystem.FastestSignificantlyLargeEnemyFormation.Formation.CachedMovementSpeed * base.Formation.QuerySystem.FastestSignificantlyLargeEnemyFormation.Formation.CachedMovementSpeed) >= 16f);
				}
				this._engaging = flag;
				if (this._engaging)
				{
					Vec2 vec = (base.Formation.CachedAveragePosition - base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedAveragePosition).Normalized().LeftVec();
					FormationQuerySystem closestSignificantlyLargeEnemyFormation = base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation;
					float num = 50f + (base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.Width + base.Formation.Depth) * 0.5f;
					float num2 = 0f;
					Formation formation = base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation;
					foreach (Team team in Mission.Current.Teams)
					{
						if (team.IsEnemyOf(base.Formation.Team))
						{
							foreach (Formation formation2 in team.FormationsIncludingSpecialAndEmpty)
							{
								if (formation2.CountOfUnits > 0 && formation2.QuerySystem != closestSignificantlyLargeEnemyFormation)
								{
									Vec2 vec2 = formation2.CachedAveragePosition - closestSignificantlyLargeEnemyFormation.Formation.CachedAveragePosition;
									float num3 = vec2.Normalize();
									if (vec.DotProduct(vec2) > 0.8f && num3 < num && num3 > num2)
									{
										num2 = num3;
										formation = formation2;
									}
								}
							}
						}
					}
					if (!(base.Formation.Team.TeamAI is TeamAISiegeComponent) && base.Formation.QuerySystem.RangedCavalryUnitRatio > 0.95f && base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation == formation)
					{
						base.CurrentOrder = MovementOrder.MovementOrderCharge;
						return;
					}
					bool flag2 = formation.QuerySystem.IsCavalryFormation || formation.QuerySystem.IsRangedCavalryFormation;
					float num4 = (flag2 ? 35f : 20f);
					num4 += (formation.Depth + base.Formation.Width) * 0.5f;
					num4 = MathF.Min(num4, base.Formation.QuerySystem.MissileRangeAdjusted - base.Formation.Width * 0.5f);
					BehaviorMountedSkirmish.Ellipse ellipse = new BehaviorMountedSkirmish.Ellipse(formation.CachedMedianPosition.AsVec2, num4, formation.Width * 0.5f * (flag2 ? 1.5f : 1f), formation.Direction);
					cachedMedianPosition.SetVec2(ellipse.GetTargetPos(base.Formation.CachedAveragePosition, 20f));
				}
				else
				{
					cachedMedianPosition = new WorldPosition(Mission.Current.Scene, new Vec3(base.Formation.QuerySystem.AverageAllyPosition, base.Formation.Team.QuerySystem.MedianPosition.GetNavMeshZ() + 100f, -1f));
				}
			}
			base.CurrentOrder = MovementOrder.MovementOrderMove(cachedMedianPosition);
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x00020A20 File Offset: 0x0001EC20
		public override void TickOccasionally()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x00020A3C File Offset: 0x0001EC3C
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFacingOrder(FacingOrder.FacingOrderLookAtEnemy);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderDeep, true);
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x00020AA1 File Offset: 0x0001ECA1
		protected override float GetAiWeight()
		{
			if (!this._isEnemyReachable)
			{
				return 0.1f;
			}
			return 1f;
		}

		// Token: 0x04000368 RID: 872
		private bool _engaging = true;

		// Token: 0x04000369 RID: 873
		private bool _isEnemyReachable = true;

		// Token: 0x0200043D RID: 1085
		private struct Ellipse
		{
			// Token: 0x06003835 RID: 14389 RVA: 0x000E5F77 File Offset: 0x000E4177
			public Ellipse(Vec2 center, float radius, float halfLength, Vec2 direction)
			{
				this._center = center;
				this._radius = radius;
				this._halfLength = halfLength;
				this._direction = direction;
			}

			// Token: 0x06003836 RID: 14390 RVA: 0x000E5F98 File Offset: 0x000E4198
			public Vec2 GetTargetPos(Vec2 position, float distance)
			{
				Vec2 vec = this._direction.LeftVec();
				Vec2 vec2 = this._center + vec * this._halfLength;
				Vec2 vec3 = this._center - vec * this._halfLength;
				Vec2 vec4 = position - this._center;
				bool flag = vec4.Normalized().DotProduct(this._direction) > 0f;
				Vec2 vec5 = vec4.DotProduct(vec) * vec;
				bool flag2 = vec5.Length < this._halfLength;
				bool flag3 = true;
				if (flag2)
				{
					position = this._center + vec5 + this._direction * (this._radius * (float)(flag ? 1 : (-1)));
				}
				else
				{
					flag3 = vec5.DotProduct(vec) > 0f;
					Vec2 vec6 = (position - (flag3 ? vec2 : vec3)).Normalized();
					position = (flag3 ? vec2 : vec3) + vec6 * this._radius;
				}
				Vec2 vec7 = this._center + vec5;
				float num = 6.2831855f * this._radius;
				while (distance > 0f)
				{
					if (flag2 && flag)
					{
						float num2 = (((vec2 - vec7).Length < distance) ? (vec2 - vec7).Length : distance);
						position = vec7 + (vec2 - vec7).Normalized() * num2;
						position += this._direction * this._radius;
						distance -= num2;
						flag2 = false;
						flag3 = true;
					}
					else if (!flag2 && flag3)
					{
						Vec2 vec8 = (position - vec2).Normalized();
						float num3 = MathF.Acos(MBMath.ClampFloat(this._direction.DotProduct(vec8), -1f, 1f));
						float num4 = 6.2831855f * (distance / num);
						float num5 = ((num3 + num4 < 3.1415927f) ? (num3 + num4) : 3.1415927f);
						float num6 = (num5 - num3) / 3.1415927f * (num / 2f);
						Vec2 direction = this._direction;
						direction.RotateCCW(num5);
						position = vec2 + direction * this._radius;
						distance -= num6;
						flag2 = true;
						flag = false;
					}
					else if (flag2)
					{
						float num7 = (((vec3 - vec7).Length < distance) ? (vec3 - vec7).Length : distance);
						position = vec7 + (vec3 - vec7).Normalized() * num7;
						position -= this._direction * this._radius;
						distance -= num7;
						flag2 = false;
						flag3 = false;
					}
					else
					{
						Vec2 vec9 = (position - vec3).Normalized();
						float num8 = MathF.Acos(MBMath.ClampFloat(this._direction.DotProduct(vec9), -1f, 1f));
						float num9 = 6.2831855f * (distance / num);
						float num10 = ((num8 - num9 > 0f) ? (num8 - num9) : 0f);
						float num11 = num8 - num10;
						float num12 = num11 / 3.1415927f * (num / 2f);
						Vec2 vec10 = vec9;
						vec10.RotateCCW(num11);
						position = vec3 + vec10 * this._radius;
						distance -= num12;
						flag2 = true;
						flag = true;
					}
				}
				return position;
			}

			// Token: 0x04001968 RID: 6504
			private readonly Vec2 _center;

			// Token: 0x04001969 RID: 6505
			private readonly float _radius;

			// Token: 0x0400196A RID: 6506
			private readonly float _halfLength;

			// Token: 0x0400196B RID: 6507
			private readonly Vec2 _direction;
		}
	}
}
