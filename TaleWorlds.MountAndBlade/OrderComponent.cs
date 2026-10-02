using System;
using System.Diagnostics;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000157 RID: 343
	public abstract class OrderComponent
	{
		// Token: 0x0600121E RID: 4638 RVA: 0x00039368 File Offset: 0x00037568
		public Vec2 GetDirection(Formation f)
		{
			Vec2 vec = this.Direction(f);
			if (f.IsAIControlled && vec.DotProduct(this._previousDirection) > 0.87f)
			{
				vec = this._previousDirection;
			}
			else
			{
				this._previousDirection = vec;
			}
			return vec;
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x000393AF File Offset: 0x000375AF
		protected void CopyPositionAndDirectionFrom(OrderComponent order)
		{
			this.Position = order.Position;
			this.Direction = order.Direction;
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x000393C9 File Offset: 0x000375C9
		protected OrderComponent(float tickTimerDuration = 0.5f)
		{
			this._tickTimer = new Timer(Mission.Current.CurrentTime, tickTimerDuration, true);
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06001221 RID: 4641
		public abstract OrderType OrderType { get; }

		// Token: 0x06001222 RID: 4642 RVA: 0x000393F3 File Offset: 0x000375F3
		internal bool Tick(Formation formation)
		{
			bool flag = this._tickTimer.Check(Mission.Current.CurrentTime);
			if (flag)
			{
				this.TickOccasionally(formation, this._tickTimer.PreviousDeltaTime);
			}
			return flag;
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x0003941F File Offset: 0x0003761F
		[Conditional("DEBUG")]
		protected virtual void TickDebug(Formation formation)
		{
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x00039421 File Offset: 0x00037621
		protected internal virtual void TickOccasionally(Formation formation, float dt)
		{
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x00039423 File Offset: 0x00037623
		protected internal virtual void OnApply(Formation formation)
		{
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x00039425 File Offset: 0x00037625
		protected internal virtual void OnCancel(Formation formation)
		{
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x00039427 File Offset: 0x00037627
		protected internal virtual void OnUnitJoinOrLeave(Agent unit, bool isJoining)
		{
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x00039429 File Offset: 0x00037629
		protected internal virtual bool IsApplicable(Formation formation)
		{
			return true;
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06001229 RID: 4649 RVA: 0x0003942C File Offset: 0x0003762C
		protected internal virtual bool CanStack
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x0600122A RID: 4650 RVA: 0x0003942F File Offset: 0x0003762F
		protected internal virtual bool CancelsPreviousDirectionOrder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x0600122B RID: 4651 RVA: 0x00039432 File Offset: 0x00037632
		protected internal virtual bool CancelsPreviousArrangementOrder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x00039435 File Offset: 0x00037635
		protected internal virtual MovementOrder GetSubstituteOrder(Formation formation)
		{
			return MovementOrder.MovementOrderCharge;
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x0003943C File Offset: 0x0003763C
		protected internal virtual void OnArrangementChanged(Formation formation)
		{
		}

		// Token: 0x0400046D RID: 1133
		private readonly Timer _tickTimer;

		// Token: 0x0400046E RID: 1134
		protected Func<Formation, Vec3> Position;

		// Token: 0x0400046F RID: 1135
		protected Func<Formation, Vec2> Direction;

		// Token: 0x04000470 RID: 1136
		private Vec2 _previousDirection = Vec2.Invalid;
	}
}
