using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200035B RID: 859
	public class StandingPointWithVolumeBox : StandingPointWithWeaponRequirement
	{
		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x0600313E RID: 12606 RVA: 0x000C7F14 File Offset: 0x000C6114
		public override Agent.AIScriptedFrameFlags DisableScriptedFrameFlags
		{
			get
			{
				return Agent.AIScriptedFrameFlags.NoAttack;
			}
		}

		// Token: 0x0600313F RID: 12607 RVA: 0x000C7F18 File Offset: 0x000C6118
		public override bool IsDisabledForAgent(Agent agent)
		{
			return base.IsDisabledForAgent(agent) || MathF.Abs(agent.Position.z - base.GameEntity.GlobalPosition.z) > 2f || agent.Position.DistanceSquared(base.GameEntity.GlobalPosition) > 100f;
		}

		// Token: 0x06003140 RID: 12608 RVA: 0x000C7F7E File Offset: 0x000C617E
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			MBEditor.IsEntitySelected(base.GameEntity);
		}

		// Token: 0x040014A6 RID: 5286
		private const float MaxUserAgentDistance = 10f;

		// Token: 0x040014A7 RID: 5287
		private const float MaxUserAgentElevation = 2f;

		// Token: 0x040014A8 RID: 5288
		public string VolumeBoxTag = "volumebox";
	}
}
