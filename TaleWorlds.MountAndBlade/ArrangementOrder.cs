using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000151 RID: 337
	public struct ArrangementOrder
	{
		// Token: 0x060011A9 RID: 4521 RVA: 0x000361C2 File Offset: 0x000343C2
		public static int GetUnitSpacingOf(ArrangementOrder.ArrangementOrderEnum a)
		{
			switch (a)
			{
			case ArrangementOrder.ArrangementOrderEnum.Loose:
				return 6;
			case ArrangementOrder.ArrangementOrderEnum.ShieldWall:
			case ArrangementOrder.ArrangementOrderEnum.Square:
				return 0;
			}
			return 2;
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x000361E7 File Offset: 0x000343E7
		public static bool GetUnitLooseness(ArrangementOrder.ArrangementOrderEnum a)
		{
			return a != ArrangementOrder.ArrangementOrderEnum.ShieldWall;
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x000361F0 File Offset: 0x000343F0
		public ArrangementOrder(ArrangementOrder.ArrangementOrderEnum orderEnum)
		{
			this.OrderEnum = orderEnum;
			this._walkRestriction = null;
			switch (this.OrderEnum)
			{
			case ArrangementOrder.ArrangementOrderEnum.Circle:
				this._runRestriction = new float?(0.5f);
				goto IL_009A;
			case ArrangementOrder.ArrangementOrderEnum.Line:
				this._runRestriction = new float?(0.8f);
				goto IL_009A;
			case ArrangementOrder.ArrangementOrderEnum.Loose:
			case ArrangementOrder.ArrangementOrderEnum.Scatter:
			case ArrangementOrder.ArrangementOrderEnum.Skein:
				this._runRestriction = new float?(0.9f);
				goto IL_009A;
			case ArrangementOrder.ArrangementOrderEnum.ShieldWall:
			case ArrangementOrder.ArrangementOrderEnum.Square:
				this._runRestriction = new float?(0.3f);
				goto IL_009A;
			}
			this._runRestriction = new float?(1f);
			IL_009A:
			this._unitSpacing = ArrangementOrder.GetUnitSpacingOf(this.OrderEnum);
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x000362A8 File Offset: 0x000344A8
		public void GetMovementSpeedRestriction(out float? runRestriction, out float? walkRestriction)
		{
			runRestriction = this._runRestriction;
			walkRestriction = this._walkRestriction;
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x000362C4 File Offset: 0x000344C4
		public IFormationArrangement GetArrangement(Formation formation)
		{
			ArrangementOrder.ArrangementOrderEnum orderEnum = this.OrderEnum;
			if (orderEnum <= ArrangementOrder.ArrangementOrderEnum.Column)
			{
				if (orderEnum == ArrangementOrder.ArrangementOrderEnum.Circle)
				{
					return new CircularFormation(formation);
				}
				if (orderEnum == ArrangementOrder.ArrangementOrderEnum.Column)
				{
					return new ColumnFormation(formation, null, 1);
				}
			}
			else
			{
				if (orderEnum == ArrangementOrder.ArrangementOrderEnum.Skein)
				{
					return new SkeinFormation(formation);
				}
				if (orderEnum == ArrangementOrder.ArrangementOrderEnum.Square)
				{
					return new RectilinearSchiltronFormation(formation);
				}
			}
			return new LineFormation(formation, true);
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x00036314 File Offset: 0x00034514
		public unsafe void OnApply(Formation formation)
		{
			formation.SetPositioning(null, null, new int?(this.GetUnitSpacing()));
			this.Rearrange(formation);
			if (this.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Scatter)
			{
				this.TickOccasionally(formation);
				formation.ResetArrangementOrderTickTimer();
			}
			ArrangementOrder.ArrangementOrderEnum orderEnum = this.OrderEnum;
			formation.ApplyActionOnEachUnit(delegate(Agent agent)
			{
				if (agent.IsAIControlled)
				{
					Agent.UsageDirection shieldDirectionOfUnit = ArrangementOrder.GetShieldDirectionOfUnit(formation, agent, orderEnum);
					agent.EnforceShieldUsage(shieldDirectionOfUnit);
				}
				agent.UpdateAgentProperties();
				MovementOrder movementOrder = *formation.GetReadonlyMovementOrderReference();
				MovementOrder.MovementOrderEnum movementOrderEnum = movementOrder.OrderEnum;
				if ((movementOrderEnum == MovementOrder.MovementOrderEnum.Charge || movementOrderEnum == MovementOrder.MovementOrderEnum.ChargeToTarget) && movementOrder.GetPosition(formation).IsValid)
				{
					movementOrderEnum = MovementOrder.MovementOrderEnum.Move;
				}
				agent.RefreshBehaviorValues(movementOrderEnum, orderEnum);
			}, null);
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x000363A6 File Offset: 0x000345A6
		public void SoftUpdate(Formation formation)
		{
			if (this.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Scatter)
			{
				this.TickOccasionally(formation);
				formation.ResetArrangementOrderTickTimer();
			}
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x000363C0 File Offset: 0x000345C0
		public static Agent.UsageDirection GetShieldDirectionOfUnit(Formation formation, Agent unit, ArrangementOrder.ArrangementOrderEnum orderEnum)
		{
			Agent.UsageDirection usageDirection;
			if (unit.IsDetachedFromFormation)
			{
				usageDirection = Agent.UsageDirection.None;
			}
			else if (orderEnum == ArrangementOrder.ArrangementOrderEnum.ShieldWall)
			{
				if (((IFormationUnit)unit).FormationRankIndex == 0)
				{
					usageDirection = Agent.UsageDirection.DefendDown;
				}
				else if (formation.Arrangement.GetNeighborUnitOfLeftSide(unit) == null)
				{
					usageDirection = Agent.UsageDirection.DefendLeft;
				}
				else if (formation.Arrangement.GetNeighborUnitOfRightSide(unit) == null)
				{
					usageDirection = Agent.UsageDirection.DefendRight;
				}
				else
				{
					usageDirection = Agent.UsageDirection.AttackEnd;
				}
			}
			else if (orderEnum == ArrangementOrder.ArrangementOrderEnum.Circle || orderEnum == ArrangementOrder.ArrangementOrderEnum.Square)
			{
				if (((IFormationUnit)unit).IsShieldUsageEncouraged)
				{
					if (((IFormationUnit)unit).FormationRankIndex == 0)
					{
						usageDirection = Agent.UsageDirection.DefendDown;
					}
					else
					{
						usageDirection = Agent.UsageDirection.AttackEnd;
					}
				}
				else
				{
					usageDirection = Agent.UsageDirection.None;
				}
			}
			else
			{
				usageDirection = Agent.UsageDirection.None;
			}
			return usageDirection;
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x0003643B File Offset: 0x0003463B
		public int GetUnitSpacing()
		{
			return this._unitSpacing;
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x00036443 File Offset: 0x00034643
		public void Rearrange(Formation formation)
		{
			if (this.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Column)
			{
				this.RearrangeAux(formation, false);
				return;
			}
			formation.Rearrange(this.GetArrangement(formation));
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x00036464 File Offset: 0x00034664
		public void RearrangeAux(Formation formation, bool isDirectly)
		{
			if (!isDirectly)
			{
				ArrangementOrder.TransposeLineFormation(formation);
				formation.OnTick += formation.TickForColumnArrangementInitialPositioning;
				return;
			}
			formation.OnTick -= formation.TickForColumnArrangementInitialPositioning;
			formation.ReferencePosition = null;
			formation.Rearrange(this.GetArrangement(formation));
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x000364B8 File Offset: 0x000346B8
		public unsafe static void TransposeLineFormation(Formation formation)
		{
			formation.Rearrange(new TransposedLineFormation(formation));
			MovementOrder movementOrder = *formation.GetReadonlyMovementOrderReference();
			formation.SetPositioning(new WorldPosition?(movementOrder.CreateNewOrderWorldPositionMT(formation, WorldPosition.WorldPositionEnforcedCache.None)), null, null);
			formation.ReferencePosition = new Vec2?(formation.OrderPosition);
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x00036514 File Offset: 0x00034714
		public void OnCancel(Formation formation)
		{
			if (this.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Scatter)
			{
				Team team = formation.Team;
				if (((team != null) ? team.TeamAI : null) != null)
				{
					MBReadOnlyList<StrategicArea> strategicAreas = formation.Team.TeamAI.StrategicAreas;
					for (int i = formation.Detachments.Count - 1; i >= 0; i--)
					{
						IDetachment detachment = formation.Detachments[i];
						foreach (StrategicArea strategicArea in strategicAreas)
						{
							if (detachment == strategicArea)
							{
								formation.LeaveDetachment(detachment);
								break;
							}
						}
					}
				}
			}
			if (this.OrderEnum == ArrangementOrder.ArrangementOrderEnum.ShieldWall || this.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Circle || this.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Square)
			{
				formation.ApplyActionOnEachUnit(delegate(Agent agent)
				{
					if (agent.IsAIControlled)
					{
						agent.EnforceShieldUsage(Agent.UsageDirection.None);
					}
				}, null);
			}
			if (this.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Column)
			{
				formation.OnTick -= formation.TickForColumnArrangementInitialPositioning;
			}
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x0003661C File Offset: 0x0003481C
		private static StrategicArea CreateStrategicArea(Scene scene, WorldPosition position, Vec2 direction, float width, int capacity, BattleSideEnum side)
		{
			WorldFrame worldFrame = new WorldFrame(new Mat3
			{
				f = direction.ToVec3(0f),
				u = Vec3.Up
			}, position);
			GameEntity gameEntity = GameEntity.Instantiate(scene, "strategic_area_autogen", worldFrame.ToNavMeshMatrixFrame(), true);
			gameEntity.SetMobility(GameEntity.Mobility.Dynamic);
			StrategicArea firstScriptOfType = gameEntity.GetFirstScriptOfType<StrategicArea>();
			firstScriptOfType.InitializeAutogenerated(width, capacity, side);
			return firstScriptOfType;
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x00036684 File Offset: 0x00034884
		private static IEnumerable<StrategicArea> CreateStrategicAreas(Mission mission, int count, WorldPosition center, float distance, WorldPosition target, float width, int capacity, BattleSideEnum side)
		{
			Scene scene = mission.Scene;
			float distanceMultiplied = distance * 0.7f;
			Func<WorldPosition> func = delegate
			{
				WorldPosition center2 = center;
				float num2 = MBRandom.RandomFloat * 3.1415927f * 2f;
				center2.SetVec2(center.AsVec2 + Vec2.FromRotation(num2) * distanceMultiplied);
				return center2;
			};
			WorldPosition[] array = delegate
			{
				float num3 = MBRandom.RandomFloat * 3.1415927f * 2f;
				switch (count)
				{
				case 2:
				{
					WorldPosition center3 = center;
					center3.SetVec2(center.AsVec2 + Vec2.FromRotation(num3) * distanceMultiplied);
					WorldPosition center4 = center;
					center4.SetVec2(center.AsVec2 + Vec2.FromRotation(num3 + 3.1415927f) * distanceMultiplied);
					return new WorldPosition[] { center3, center4 };
				}
				case 3:
				{
					WorldPosition center5 = center;
					center5.SetVec2(center.AsVec2 + Vec2.FromRotation(num3 + 0f) * distanceMultiplied);
					WorldPosition center6 = center;
					center6.SetVec2(center.AsVec2 + Vec2.FromRotation(num3 + 2.0943952f) * distanceMultiplied);
					WorldPosition center7 = center;
					center7.SetVec2(center.AsVec2 + Vec2.FromRotation(num3 + 4.1887903f) * distanceMultiplied);
					return new WorldPosition[] { center5, center6, center7 };
				}
				case 4:
				{
					WorldPosition center8 = center;
					center8.SetVec2(center.AsVec2 + Vec2.FromRotation(num3 + 0f) * distanceMultiplied);
					WorldPosition center9 = center;
					center9.SetVec2(center.AsVec2 + Vec2.FromRotation(num3 + 1.5707964f) * distanceMultiplied);
					WorldPosition center10 = center;
					center10.SetVec2(center.AsVec2 + Vec2.FromRotation(num3 + 3.1415927f) * distanceMultiplied);
					WorldPosition center11 = center;
					center11.SetVec2(center.AsVec2 + Vec2.FromRotation(num3 + 4.712389f) * distanceMultiplied);
					return new WorldPosition[] { center8, center9, center10, center11 };
				}
				default:
					Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Orders\\ArrangementOrder.cs", "CreateStrategicAreas", 369);
					return new WorldPosition[0];
				}
			}();
			List<WorldPosition> positions = new List<WorldPosition>();
			WorldPosition[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				WorldPosition worldPosition = array2[i];
				WorldPosition worldPosition2 = worldPosition;
				WorldPosition position = mission.FindPositionWithBiggestSlopeTowardsDirectionInSquare(ref worldPosition2, distance * 0.25f, ref target);
				Func<WorldPosition, bool> func2 = delegate(WorldPosition p)
				{
					float num4;
					if (!positions.Any<WorldPosition>((WorldPosition wp) => wp.AsVec2.DistanceSquared(p.AsVec2) < 1f) && (scene.GetPathDistanceBetweenPositions(ref center, ref p, 0f, out num4) && num4 < center.AsVec2.Distance(p.AsVec2) * 2f))
					{
						positions.Add(position);
						return true;
					}
					return false;
				};
				if (!func2(position) && !func2(worldPosition))
				{
					int num = 0;
					while (num++ < 10 && !func2(func()))
					{
					}
					if (num >= 10)
					{
						positions.Add(center);
					}
				}
			}
			Vec2 direction = (target.AsVec2 - center.AsVec2).Normalized();
			foreach (WorldPosition worldPosition3 in positions)
			{
				yield return ArrangementOrder.CreateStrategicArea(scene, worldPosition3, direction, width, capacity, side);
			}
			List<WorldPosition>.Enumerator enumerator = default(List<WorldPosition>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x000366D4 File Offset: 0x000348D4
		private bool IsStrategicAreaClose(StrategicArea strategicArea, Formation formation)
		{
			if (formation.GetReadonlyMovementOrderReference().OrderEnum == MovementOrder.MovementOrderEnum.Charge || formation.GetReadonlyMovementOrderReference().OrderEnum == MovementOrder.MovementOrderEnum.ChargeToTarget || !strategicArea.IsUsableBy(formation.Team.Side))
			{
				return false;
			}
			if (strategicArea.IgnoreHeight)
			{
				return MathF.Abs(strategicArea.GameEntity.GlobalPosition.x - formation.OrderPosition.X) <= strategicArea.DistanceToCheck && MathF.Abs(strategicArea.GameEntity.GlobalPosition.y - formation.OrderPosition.Y) <= strategicArea.DistanceToCheck;
			}
			WorldPosition worldPosition = formation.CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache.None);
			Vec3 globalPosition = strategicArea.GameEntity.GlobalPosition;
			return worldPosition.DistanceSquaredWithLimit(in globalPosition, strategicArea.DistanceToCheck * strategicArea.DistanceToCheck + 1E-05f) < strategicArea.DistanceToCheck * strategicArea.DistanceToCheck;
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x000367CC File Offset: 0x000349CC
		public void TickOccasionally(Formation formation)
		{
			if (this.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Scatter)
			{
				Team team = formation.Team;
				if (((team != null) ? team.TeamAI : null) != null)
				{
					MBReadOnlyList<StrategicArea> strategicAreas = formation.Team.TeamAI.StrategicAreas;
					foreach (StrategicArea strategicArea in strategicAreas)
					{
						if (this.IsStrategicAreaClose(strategicArea, formation))
						{
							bool flag = false;
							foreach (IDetachment detachment in formation.Detachments)
							{
								if (strategicArea == detachment)
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								formation.JoinDetachment(strategicArea);
							}
						}
					}
					for (int i = formation.Detachments.Count - 1; i >= 0; i--)
					{
						IDetachment detachment2 = formation.Detachments[i];
						foreach (StrategicArea strategicArea2 in strategicAreas)
						{
							if (detachment2 == strategicArea2 && !this.IsStrategicAreaClose(strategicArea2, formation))
							{
								formation.LeaveDetachment(detachment2);
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x060011BA RID: 4538 RVA: 0x00036920 File Offset: 0x00034B20
		public OrderType OrderType
		{
			get
			{
				switch (this.OrderEnum)
				{
				case ArrangementOrder.ArrangementOrderEnum.Circle:
					return OrderType.ArrangementCircular;
				case ArrangementOrder.ArrangementOrderEnum.Column:
					return OrderType.ArrangementColumn;
				case ArrangementOrder.ArrangementOrderEnum.Line:
					return OrderType.ArrangementLine;
				case ArrangementOrder.ArrangementOrderEnum.Loose:
					return OrderType.ArrangementLoose;
				case ArrangementOrder.ArrangementOrderEnum.Scatter:
					return OrderType.ArrangementScatter;
				case ArrangementOrder.ArrangementOrderEnum.ShieldWall:
					return OrderType.ArrangementCloseOrder;
				case ArrangementOrder.ArrangementOrderEnum.Skein:
					return OrderType.ArrangementVee;
				case ArrangementOrder.ArrangementOrderEnum.Square:
					return OrderType.ArrangementSchiltron;
				default:
					return OrderType.ArrangementLine;
				}
			}
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x00036976 File Offset: 0x00034B76
		public ArrangementOrder.ArrangementOrderEnum GetNativeEnum()
		{
			return this.OrderEnum;
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x0003697E File Offset: 0x00034B7E
		public override bool Equals(object obj)
		{
			return obj is ArrangementOrder && (ArrangementOrder)obj == this;
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x0003699B File Offset: 0x00034B9B
		public override int GetHashCode()
		{
			return (int)this.OrderEnum;
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x000369A3 File Offset: 0x00034BA3
		public static bool operator !=(ArrangementOrder a1, ArrangementOrder a2)
		{
			return a1.OrderEnum != a2.OrderEnum;
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x000369B6 File Offset: 0x00034BB6
		public static bool operator ==(ArrangementOrder a1, ArrangementOrder a2)
		{
			return a1.OrderEnum == a2.OrderEnum;
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x000369C8 File Offset: 0x00034BC8
		public void OnOrderPositionChanged(Formation formation, Vec2 previousOrderPosition)
		{
			if (this.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Column && formation.Arrangement is TransposedLineFormation)
			{
				Vec2 direction = formation.Direction;
				Vec2 vec = (formation.OrderPosition - previousOrderPosition).Normalized();
				float num = direction.AngleBetween(vec);
				if ((num > 1.5707964f || num < -1.5707964f) && formation.CachedAveragePosition.DistanceSquared(formation.OrderPosition) < formation.Depth * formation.Depth / 10f)
				{
					formation.ReferencePosition = new Vec2?(formation.OrderPosition);
				}
			}
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x00036A5A File Offset: 0x00034C5A
		public static int GetArrangementOrderDefensiveness(ArrangementOrder.ArrangementOrderEnum orderEnum)
		{
			if (orderEnum == ArrangementOrder.ArrangementOrderEnum.Circle || orderEnum == ArrangementOrder.ArrangementOrderEnum.ShieldWall || orderEnum == ArrangementOrder.ArrangementOrderEnum.Square)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x00036A6A File Offset: 0x00034C6A
		public static int GetArrangementOrderDefensivenessChange(ArrangementOrder.ArrangementOrderEnum previousOrderEnum, ArrangementOrder.ArrangementOrderEnum nextOrderEnum)
		{
			if (previousOrderEnum == ArrangementOrder.ArrangementOrderEnum.Circle || previousOrderEnum == ArrangementOrder.ArrangementOrderEnum.ShieldWall || previousOrderEnum == ArrangementOrder.ArrangementOrderEnum.Square)
			{
				if (nextOrderEnum != ArrangementOrder.ArrangementOrderEnum.Circle && nextOrderEnum != ArrangementOrder.ArrangementOrderEnum.ShieldWall && nextOrderEnum != ArrangementOrder.ArrangementOrderEnum.Square)
				{
					return -1;
				}
				return 0;
			}
			else
			{
				if (nextOrderEnum == ArrangementOrder.ArrangementOrderEnum.Circle || nextOrderEnum == ArrangementOrder.ArrangementOrderEnum.ShieldWall || nextOrderEnum == ArrangementOrder.ArrangementOrderEnum.Square)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x00036A94 File Offset: 0x00034C94
		public float CalculateFormationDirectionEnforcingFactorForRank(int formationRankIndex, int rankCount)
		{
			if (this.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Circle || this.OrderEnum == ArrangementOrder.ArrangementOrderEnum.ShieldWall || this.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Square)
			{
				return 1f - MBMath.ClampFloat(((float)formationRankIndex + 1f) / ((float)rankCount * 2f), 0f, 1f);
			}
			if (this.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Column)
			{
				return 0f;
			}
			return 1f - MBMath.ClampFloat(((float)formationRankIndex + 1f) / ((float)rankCount * 0.5f), 0f, 1f);
		}

		// Token: 0x04000440 RID: 1088
		private float? _walkRestriction;

		// Token: 0x04000441 RID: 1089
		private float? _runRestriction;

		// Token: 0x04000442 RID: 1090
		private int _unitSpacing;

		// Token: 0x04000443 RID: 1091
		public readonly ArrangementOrder.ArrangementOrderEnum OrderEnum;

		// Token: 0x04000444 RID: 1092
		public static readonly ArrangementOrder ArrangementOrderCircle = new ArrangementOrder(ArrangementOrder.ArrangementOrderEnum.Circle);

		// Token: 0x04000445 RID: 1093
		public static readonly ArrangementOrder ArrangementOrderColumn = new ArrangementOrder(ArrangementOrder.ArrangementOrderEnum.Column);

		// Token: 0x04000446 RID: 1094
		public static readonly ArrangementOrder ArrangementOrderLine = new ArrangementOrder(ArrangementOrder.ArrangementOrderEnum.Line);

		// Token: 0x04000447 RID: 1095
		public static readonly ArrangementOrder ArrangementOrderLoose = new ArrangementOrder(ArrangementOrder.ArrangementOrderEnum.Loose);

		// Token: 0x04000448 RID: 1096
		public static readonly ArrangementOrder ArrangementOrderScatter = new ArrangementOrder(ArrangementOrder.ArrangementOrderEnum.Scatter);

		// Token: 0x04000449 RID: 1097
		public static readonly ArrangementOrder ArrangementOrderShieldWall = new ArrangementOrder(ArrangementOrder.ArrangementOrderEnum.ShieldWall);

		// Token: 0x0400044A RID: 1098
		public static readonly ArrangementOrder ArrangementOrderSkein = new ArrangementOrder(ArrangementOrder.ArrangementOrderEnum.Skein);

		// Token: 0x0400044B RID: 1099
		public static readonly ArrangementOrder ArrangementOrderSquare = new ArrangementOrder(ArrangementOrder.ArrangementOrderEnum.Square);

		// Token: 0x0200046D RID: 1133
		public enum ArrangementOrderEnum
		{
			// Token: 0x04001A45 RID: 6725
			Circle,
			// Token: 0x04001A46 RID: 6726
			Column,
			// Token: 0x04001A47 RID: 6727
			Line,
			// Token: 0x04001A48 RID: 6728
			Loose,
			// Token: 0x04001A49 RID: 6729
			Scatter,
			// Token: 0x04001A4A RID: 6730
			ShieldWall,
			// Token: 0x04001A4B RID: 6731
			Skein,
			// Token: 0x04001A4C RID: 6732
			Square
		}
	}
}
