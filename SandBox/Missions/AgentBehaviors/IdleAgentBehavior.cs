using System;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000AC RID: 172
	public class IdleAgentBehavior : AgentBehavior
	{
		// Token: 0x0600073C RID: 1852 RVA: 0x000316A5 File Offset: 0x0002F8A5
		public IdleAgentBehavior(AgentBehaviorGroup behaviorGroup)
			: base(behaviorGroup)
		{
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x000316AE File Offset: 0x0002F8AE
		public override float GetAvailability(bool isSimulation)
		{
			return 1f;
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x000316B8 File Offset: 0x0002F8B8
		protected override void OnActivate()
		{
			base.OwnerAgent.SetIsAIPaused(true);
			base.OwnerAgent.SetTargetPosition(base.OwnerAgent.GetWorldPosition().AsVec2);
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x000316EF File Offset: 0x0002F8EF
		protected override void OnDeactivate()
		{
			base.OwnerAgent.SetIsAIPaused(false);
			base.OwnerAgent.ClearTargetFrame();
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x00031708 File Offset: 0x0002F908
		public override string GetDebugInfo()
		{
			return "Idle Behavior";
		}
	}
}
