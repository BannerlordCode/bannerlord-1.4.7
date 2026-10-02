using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000378 RID: 888
	public class ResetGravityExclusionAndEntityAttachmentOnStopUsageComponent : UsableMissionObjectComponent
	{
		// Token: 0x06003265 RID: 12901 RVA: 0x000CD4AC File Offset: 0x000CB6AC
		public ResetGravityExclusionAndEntityAttachmentOnStopUsageComponent(Action<Agent> onUseAction)
		{
			this.OnUseAction = onUseAction;
		}

		// Token: 0x06003266 RID: 12902 RVA: 0x000CD4BB File Offset: 0x000CB6BB
		protected internal override void OnUse(Agent userAgent)
		{
			this.OnUseAction(userAgent);
		}

		// Token: 0x06003267 RID: 12903 RVA: 0x000CD4C9 File Offset: 0x000CB6C9
		protected internal override void OnUseStopped(Agent userAgent, bool isSuccessful = true)
		{
			userAgent.SetExcludedFromGravity(false, false);
			userAgent.SetForceAttachedEntity(WeakGameEntity.Invalid);
		}

		// Token: 0x0400155C RID: 5468
		public Action<Agent> OnUseAction;
	}
}
