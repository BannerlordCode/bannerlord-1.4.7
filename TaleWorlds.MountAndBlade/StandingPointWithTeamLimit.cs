using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200035A RID: 858
	public class StandingPointWithTeamLimit : StandingPoint
	{
		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x06003139 RID: 12601 RVA: 0x000C7EC9 File Offset: 0x000C60C9
		// (set) Token: 0x0600313A RID: 12602 RVA: 0x000C7ED1 File Offset: 0x000C60D1
		public Team UsableTeam { get; set; }

		// Token: 0x0600313B RID: 12603 RVA: 0x000C7EDA File Offset: 0x000C60DA
		public override bool IsDisabledForAgent(Agent agent)
		{
			return agent.Team != this.UsableTeam || base.IsDisabledForAgent(agent);
		}

		// Token: 0x0600313C RID: 12604 RVA: 0x000C7EF3 File Offset: 0x000C60F3
		protected internal override bool IsUsableBySide(BattleSideEnum side)
		{
			return side == this.UsableTeam.Side && base.IsUsableBySide(side);
		}
	}
}
