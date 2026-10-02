using System;
using System.Linq;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200012F RID: 303
	public class BehaviorShootFromSiegeTower : BehaviorComponent
	{
		// Token: 0x06000ECF RID: 3791 RVA: 0x000255A7 File Offset: 0x000237A7
		public BehaviorShootFromSiegeTower(Formation formation)
			: base(formation)
		{
			this._behaviorSide = formation.AI.Side;
			this._siegeTower = Mission.Current.ActiveMissionObjects.FindAllWithType<SiegeTower>().FirstOrDefault<SiegeTower>((SiegeTower st) => st.WeaponSide == this._behaviorSide);
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x000255E8 File Offset: 0x000237E8
		public override void TickOccasionally()
		{
			base.TickOccasionally();
			if (base.Formation.AI.Side != this._behaviorSide)
			{
				this._behaviorSide = base.Formation.AI.Side;
				this._siegeTower = Mission.Current.ActiveMissionObjects.FindAllWithType<SiegeTower>().FirstOrDefault<SiegeTower>((SiegeTower st) => st.WeaponSide == this._behaviorSide);
			}
			if (this._siegeTower == null || this._siegeTower.IsDestroyed)
			{
				return;
			}
			base.Formation.SetMovementOrder(base.CurrentOrder);
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x00025676 File Offset: 0x00023876
		protected override float GetAiWeight()
		{
			return 0f;
		}

		// Token: 0x0400038E RID: 910
		private SiegeTower _siegeTower;
	}
}
