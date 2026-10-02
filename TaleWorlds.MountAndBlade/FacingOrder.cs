using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000152 RID: 338
	public struct FacingOrder
	{
		// Token: 0x060011C5 RID: 4549 RVA: 0x00036B7D File Offset: 0x00034D7D
		public static FacingOrder FacingOrderLookAtDirection(Vec2 direction)
		{
			return new FacingOrder(FacingOrder.FacingOrderEnum.LookAtDirection, direction);
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x00036B86 File Offset: 0x00034D86
		private FacingOrder(FacingOrder.FacingOrderEnum orderEnum, Vec2 direction)
		{
			this.OrderEnum = orderEnum;
			this._lookAtDirection = direction;
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x00036B96 File Offset: 0x00034D96
		private FacingOrder(FacingOrder.FacingOrderEnum orderEnum)
		{
			this.OrderEnum = orderEnum;
			this._lookAtDirection = Vec2.Invalid;
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x00036BAC File Offset: 0x00034DAC
		private Vec2 GetDirectionAux(Formation f, Agent targetAgent)
		{
			if (f.PhysicalClass.IsMounted() && targetAgent != null && targetAgent.Velocity.LengthSquared > targetAgent.GetMaximumForwardUnlimitedSpeed() * targetAgent.GetMaximumForwardUnlimitedSpeed() * 0.09f)
			{
				return targetAgent.Velocity.AsVec2.Normalized();
			}
			if (this.OrderEnum == FacingOrder.FacingOrderEnum.LookAtDirection)
			{
				return this._lookAtDirection;
			}
			if (f.Arrangement is CircularFormation || f.Arrangement is SquareFormation)
			{
				return f.Direction;
			}
			Vec2 currentPosition = f.CurrentPosition;
			Vec2 weightedAverageEnemyPosition = f.QuerySystem.WeightedAverageEnemyPosition;
			if (!weightedAverageEnemyPosition.IsValid)
			{
				return f.Direction;
			}
			Vec2 vec = (weightedAverageEnemyPosition - currentPosition).Normalized();
			float length = (weightedAverageEnemyPosition - currentPosition).Length;
			int enemyUnitCount = f.QuerySystem.Team.EnemyUnitCount;
			int countOfUnits = f.CountOfUnits;
			Vec2 vec2 = f.Direction;
			bool flag = length >= (float)countOfUnits * 0.2f;
			if (enemyUnitCount == 0 || countOfUnits == 0)
			{
				flag = false;
			}
			float num = ((!flag) ? 1f : (MBMath.ClampFloat((float)countOfUnits * 1f / (float)enemyUnitCount, 0.33333334f, 3f) * MBMath.ClampFloat(length / (float)countOfUnits, 0.33333334f, 3f)));
			if (flag && MathF.Abs(vec.AngleBetween(vec2)) > 0.17453292f * num)
			{
				vec2 = vec;
			}
			return vec2;
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x060011C9 RID: 4553 RVA: 0x00036D18 File Offset: 0x00034F18
		public OrderType OrderType
		{
			get
			{
				if (this.OrderEnum != FacingOrder.FacingOrderEnum.LookAtDirection)
				{
					return OrderType.LookAtEnemy;
				}
				return OrderType.LookAtDirection;
			}
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x00036D27 File Offset: 0x00034F27
		public Vec2 GetDirection(Formation f, Agent targetAgent = null)
		{
			return this.GetDirectionAux(f, targetAgent);
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x00036D34 File Offset: 0x00034F34
		public override bool Equals(object obj)
		{
			if (obj is FacingOrder)
			{
				FacingOrder facingOrder = (FacingOrder)obj;
				return facingOrder == this;
			}
			return false;
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x00036D60 File Offset: 0x00034F60
		public override int GetHashCode()
		{
			return (int)this.OrderEnum;
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x00036D68 File Offset: 0x00034F68
		public static bool operator !=(FacingOrder f1, FacingOrder f2)
		{
			return f1.OrderEnum != f2.OrderEnum;
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x00036D7B File Offset: 0x00034F7B
		public static bool operator ==(FacingOrder f1, FacingOrder f2)
		{
			return f1.OrderEnum == f2.OrderEnum;
		}

		// Token: 0x0400044C RID: 1100
		public readonly FacingOrder.FacingOrderEnum OrderEnum;

		// Token: 0x0400044D RID: 1101
		private readonly Vec2 _lookAtDirection;

		// Token: 0x0400044E RID: 1102
		public static readonly FacingOrder FacingOrderLookAtEnemy = new FacingOrder(FacingOrder.FacingOrderEnum.LookAtEnemy);

		// Token: 0x02000474 RID: 1140
		public enum FacingOrderEnum
		{
			// Token: 0x04001A70 RID: 6768
			LookAtDirection,
			// Token: 0x04001A71 RID: 6769
			LookAtEnemy
		}
	}
}
