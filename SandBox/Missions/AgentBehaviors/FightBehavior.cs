using System;
using SandBox.Missions.MissionLogics;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000A9 RID: 169
	public class FightBehavior : AgentBehavior
	{
		// Token: 0x06000718 RID: 1816 RVA: 0x00030053 File Offset: 0x0002E253
		public FightBehavior(AgentBehaviorGroup behaviorGroup)
			: base(behaviorGroup)
		{
			if (base.OwnerAgent.HumanAIComponent == null)
			{
				base.OwnerAgent.AddComponent(new HumanAIComponent(base.OwnerAgent));
			}
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x0003007F File Offset: 0x0002E27F
		public override float GetAvailability(bool isSimulation)
		{
			if (!MissionFightHandler.IsAgentAggressive(base.OwnerAgent))
			{
				return 0.1f;
			}
			return 1f;
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x0003009C File Offset: 0x0002E29C
		protected override void OnActivate()
		{
			TextObject textObject = new TextObject("{=!}{p0} {p1} activate alarmed behavior group.", null);
			textObject.SetTextVariable("p0", base.OwnerAgent.Name.ToString());
			textObject.SetTextVariable("p1", base.OwnerAgent.Index.ToString());
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x000300F0 File Offset: 0x0002E2F0
		protected override void OnDeactivate()
		{
			TextObject textObject = new TextObject("{=!}{p0} {p1} deactivate fight behavior.", null);
			textObject.SetTextVariable("p0", base.OwnerAgent.Name.ToString());
			textObject.SetTextVariable("p1", base.OwnerAgent.Index.ToString());
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00030142 File Offset: 0x0002E342
		public override string GetDebugInfo()
		{
			return "Fight";
		}
	}
}
