using System;
using System.Linq;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000136 RID: 310
	public class BehaviorUseMurderHole : BehaviorComponent
	{
		// Token: 0x06000EFC RID: 3836 RVA: 0x00027B14 File Offset: 0x00025D14
		public BehaviorUseMurderHole(Formation formation)
			: base(formation)
		{
			this._behaviorSide = formation.AI.Side;
			WorldPosition worldPosition = new WorldPosition(base.Formation.Team.Mission.Scene, UIntPtr.Zero, (formation.Team.TeamAI as TeamAISiegeDefender).MurderHolePosition, false);
			this._outerGate = (formation.Team.TeamAI as TeamAISiegeDefender).OuterGate;
			this._innerGate = (formation.Team.TeamAI as TeamAISiegeDefender).InnerGate;
			this._batteringRam = base.Formation.Team.Mission.ActiveMissionObjects.FindAllWithType<BatteringRam>().FirstOrDefault<BatteringRam>();
			base.CurrentOrder = MovementOrder.MovementOrderMove(worldPosition);
			base.BehaviorCoherence = 0f;
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x00027BE2 File Offset: 0x00025DE2
		public override void TickOccasionally()
		{
			base.Formation.SetMovementOrder(base.CurrentOrder);
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x00027BF8 File Offset: 0x00025DF8
		public bool IsMurderHoleActive()
		{
			return (this._batteringRam != null && this._batteringRam.HasArrivedAtTarget && !this._innerGate.IsDestroyed) || (this._outerGate.IsDestroyed && !this._innerGate.IsDestroyed);
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x00027C46 File Offset: 0x00025E46
		protected override float GetAiWeight()
		{
			return 10f * (this.IsMurderHoleActive() ? 1f : 0f);
		}

		// Token: 0x040003A4 RID: 932
		private readonly CastleGate _outerGate;

		// Token: 0x040003A5 RID: 933
		private readonly CastleGate _innerGate;

		// Token: 0x040003A6 RID: 934
		private readonly BatteringRam _batteringRam;
	}
}
